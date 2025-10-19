namespace Enceladus.Core.Utils
{
    public static class LinqExtensions
    {
        public static List<T> AsList<T>(this T item) => new List<T> { item };
    }
}
