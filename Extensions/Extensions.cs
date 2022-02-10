using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AGOT.Extensions;
public static class Extensions
{
    public static int ToInt (this object? i) => !int.TryParse((string?)i, out var a) ? 0 : a;
    
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

    public static UIElement StatusTextBox(bool success, string txt, string reason = "")
    {
        var textBox = new TextBox()
        {
            Text = $"{txt} {(success ? "successfully generated." : "failed to generate.")} {reason}", 
            Foreground = success ? Brushes.Green : Brushes.DarkRed, Name = "txtB", Background = Brushes.DarkGray, TextWrapping = TextWrapping.Wrap,
        };
        return textBox;
    }
}
