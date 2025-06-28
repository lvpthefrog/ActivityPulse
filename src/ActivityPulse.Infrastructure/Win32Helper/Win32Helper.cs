using ActivityPulse.Application;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ActivityPulse.Infrastructure
{
    public static class Win32Helper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static ActiveWindowInfo GetActiveWindowInfo()
        {
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero) return new ActiveWindowInfo();

            // Lấy title
            var windowText = new StringBuilder(256);
            GetWindowText(hwnd, windowText, windowText.Capacity);

            // Lấy process id
            GetWindowThreadProcessId(hwnd, out uint processId);

            try
            {
                using var proc = Process.GetProcessById((int)processId);
                string processName = proc.ProcessName ?? "";
                string processPath = "";
                try { processPath = proc.MainModule?.FileName ?? ""; }
                catch { }
                return new ActiveWindowInfo
                {
                    ProcessName = processName,
                    ProcessPath = processPath,
                    WindowTitle = windowText.ToString()
                };
            }
            catch
            {
                return new ActiveWindowInfo
                {
                    ProcessName = "",
                    ProcessPath = "",
                    WindowTitle = windowText.ToString()
                };
            }
        }

        public static TimeSpan GetIdleTime()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            if (!GetLastInputInfo(ref lastInputInfo)) return TimeSpan.Zero;
            uint idleTime = ((uint)Environment.TickCount - lastInputInfo.dwTime);
            return TimeSpan.FromMilliseconds(idleTime);
        }
    }
}
