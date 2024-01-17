using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace IdentityServer.Data;

public class LogSink
{
    public readonly TextWriter Receiver;
    public readonly ObservableCollection<LogLine> Lines = [];

    private readonly int MaxLines;

    public LogSink(int maxLines)
    {
        MaxLines = maxLines;
        Receiver = new Slicer(AddLine);
    }

    private void AddLine(string line)
    {
        // Don't write whitespace duplicates
        if (string.IsNullOrWhiteSpace(line) && Lines.Count > 0 && string.IsNullOrWhiteSpace(Lines[^1].Message))
            return;

        Lines.Add(new LogLine(line));
        var diff = Lines.Count - MaxLines;
        if (diff == 0)
            return;
        
        foreach (var removable in Lines.Take(diff).ToArray())
            Lines.Remove(removable);
    }

    private static readonly Regex colorMatcher = new(@"(?<=[0-9-:.\+ZztT\\\/]*\s*\[)(Fatal|Critical|Error|Warning|Information|Debug|Trace|Verbose)(?=\])", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200));

    public class LogLine
    {
        public string Message { get; }
        public string LightThemeColor { get; private set; }
        public string DarkThemeColor { get; private set; }

        public LogLine(string message)
        {
            Message = SetColors(message);
        }

        private string SetColors(string message)
        {
            var logLevel = colorMatcher.Match(message ?? string.Empty).Value;
            var (light, dark) = MapColor(logLevel);
            LightThemeColor = ToHex(light);
            DarkThemeColor = ToHex(dark);
            return message.Replace($"[{logLevel}]", string.Empty);
        }

        private static (Color light, Color dark) MapColor(string logLevel)
        {
            return Capitalize(logLevel ?? string.Empty) switch
            {
                "Fatal" or nameof(LogLevel.Critical) => (Color.RebeccaPurple, Color.Purple),
                nameof(LogLevel.Error) => (Color.Red, Color.Red),
                nameof(LogLevel.Warning) => (Color.DarkOliveGreen, Color.Yellow),
                nameof(LogLevel.Information) => (Color.Black, Color.White),
                nameof(LogLevel.Debug) => (Color.DarkGray, Color.Gray),
                _ => (Color.Gray, Color.DarkGray),
            };
        }
        private static string Capitalize(string value) => value.Length == 0 ? string.Empty : value[0].ToString().ToUpperInvariant() + value.ToLowerInvariant()[1..];
        private static string ToHex(Color color) => $"#{color.R:X2}{color.G:X2}{color.B:X2}";
 
    }

    private class Slicer(Action<string> writer) : TextWriter
    {
        private readonly SemaphoreSlim locker = new(1, 1);
        private readonly StringBuilder buffer = new(256);
        private int terminatorPosition;

        private void FlushLine()
        {
            try
            {
                locker.Wait();
                writer(buffer.ToString().TrimEnd('\r','\n'));
                buffer.Clear();
            }
            finally
            {
                if (locker.CurrentCount == 0)
                    locker.Release();
            }
        }

        public override void Write(char value)
        {
            buffer.Append(value);

            if (value == NewLine[terminatorPosition])
            {
                if (terminatorPosition == NewLine.Length - 1)
                {
                    FlushLine();
                    terminatorPosition = 0;
                    return;
                }
                terminatorPosition++;
            }
            else
            {
                terminatorPosition = 0;
            }
        }

        public override void Write(string value)
        {
            foreach (var @char in value)
                Write(@char);
        }

        public override Encoding Encoding { get; } = Encoding.Default;
    }

}
