namespace IdentityServer.Services;

public class HostAddressService
{
    private string _hostAddress;

    public string HostAddress
    {
        get => _hostAddress ?? string.Empty;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            if (string.IsNullOrWhiteSpace(_hostAddress))
            {
                _hostAddress = value;
            }
        }
    }
}
