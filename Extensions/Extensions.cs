namespace AGOT.Extensions;
public static class Extensions
{
    public static int ToInt (this object? i) => !int.TryParse((string?)i, out var a) ? 0 : a;

    // public static object? ToIntOrElseClean<T> (this T i , string path)
    // {
    //     if (int.TryParse(i?.ToString(), out var a))
    //     {
    //         return a;
    //     }
    //     File.WriteAllText(path, string.Empty);
    //     return default;
    // }
    
    public static string RemoveExtra (this string? txt) => txt.Replace(" ", "_").Replace("'", "").ToLower();


    public static bool IsEmptyOrNull (this string? myString) => myString is "" or null;

    public static string IfNullOrEmptyReplace (this string? myString, string? secondString) =>
        (myString is not "" or null ? myString : secondString) ?? string.Empty;

    public static string HexToRgb (this string? hex)
    {
        if (hex is { Length: < 6 }) hex = hex.PadLeft(6, '0');
        
        return $"{Convert.ToInt32(hex.Substring(0, 2), 16)} " +
               $"{Convert.ToInt32(hex.Substring(2, 2), 16)} " +
               $"{Convert.ToInt32(hex.Substring(4, 2), 16)}";
    }

    public static bool ListIsEmpty (this List<int> list) => list.Count <= 0;
}
