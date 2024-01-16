using System.Collections.Generic;

namespace IdentityServer.Services;

public class GenericComparer<T> : EqualityComparer<T>
{
    private readonly GenericComparerEquals<T> equals;
    private readonly GenericComparerHash<T> hash;

    public GenericComparer(GenericComparerEquals<T> equals, GenericComparerHash<T> hash)
    {
        this.equals = equals ?? DefaultEqualityComparer;
        this.hash = hash ?? DefaultHashComparer;
    }

    public GenericComparer(GenericComparerEquals<T> equals) : this(equals, DefaultHashComparer) { }

    public override bool Equals(T? x, T? y)
    {
        return Default.Equals(x, default)
                ? Default.Equals(y, default)
                : y != null && equals(x, y);
    }

    public override int GetHashCode(T obj) => Default.Equals(obj, default) ? 0 : hash(obj);

    public static readonly GenericComparerEquals<T> DefaultEqualityComparer = new((x, y) => x is null ? y is null : x.Equals(y));
    public static readonly GenericComparerHash<T> DefaultHashComparer = new(h => h is null ? 0 : h.GetHashCode());
    public static GenericComparer<T> Create(GenericComparerEquals<T> equals, GenericComparerHash<T> hash) => new(equals, hash);
    public static GenericComparer<T> Create(GenericComparerEquals<T> equals) => new(equals);
    public static GenericComparer<T> CreateHash(GenericComparerHash<T> hash) => new(DefaultEqualityComparer, hash);


    public delegate bool GenericComparerEquals<V>(V? obj1, V? obj2);
    public delegate int GenericComparerHash<V>(V? obj);
}
