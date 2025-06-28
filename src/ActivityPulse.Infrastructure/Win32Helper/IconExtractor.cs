using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ActivityPulse.Infrastructure
{
    [SupportedOSPlatform("windows")]
    public static class IconExtractor
    {
        private static readonly string _iconCachePath = System.IO.Path.Combine(
            InfraHelper.GetDatabaseDirectory(),
            "Icons");

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes,
            ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;

        public static string? ExtractIcon(string processPath)
        {
            try
            {
                if (!File.Exists(processPath))
                    return null;

                Directory.CreateDirectory(_iconCachePath);
                var fileName = Path.GetFileNameWithoutExtension(processPath);
                var iconFilePath = Path.Combine(_iconCachePath, $"{fileName}.png");

                if (File.Exists(iconFilePath))
                    return iconFilePath;

                var shfi = new SHFILEINFO();
                var hImgLarge = SHGetFileInfo(processPath, 0, ref shfi,
                    (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_LARGEICON);

                if (shfi.hIcon != IntPtr.Zero)
                {
                    using var icon = Icon.FromHandle(shfi.hIcon);
                    using var bitmap = icon.ToBitmap();
                    bitmap.Save(iconFilePath, ImageFormat.Png);
                    DestroyIcon(shfi.hIcon);
                    return iconFilePath;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
