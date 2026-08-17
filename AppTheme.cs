using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace FiloYonetimi;

public static class AppTheme
{
    public static readonly Color Navy = Color.FromArgb(4, 45, 88);
    public static readonly Color Blue = Color.FromArgb(20, 120, 220);
    public static readonly Color Cyan = Color.FromArgb(16, 174, 190);
    public static readonly Color Green = Color.FromArgb(52, 168, 83);
    public static readonly Color Orange = Color.FromArgb(245, 166, 35);
    public static readonly Color Red = Color.FromArgb(232, 69, 69);
    public static readonly Color Bg = Color.FromArgb(246, 248, 251);
    public static readonly Color Text = Color.FromArgb(33, 43, 54);
    public static readonly Color Muted = Color.FromArgb(105, 117, 130);

    public static Bitmap? LoadLogo()
    {
        using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("FiloYonetimi.Assets.turk-telekom.png");
        if (s == null) return null;
        using var temp = new MemoryStream();
        s.CopyTo(temp);
        temp.Position = 0;
        return new Bitmap(temp);
    }
}
