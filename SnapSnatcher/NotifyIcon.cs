using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SnapSnatcher
{
    public class NotifyIcon
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct NotifyIconData
        {
            public System.UInt32 cbSize; // DWORD
            public System.IntPtr hWnd; // HWND
            public System.UInt32 uID; // UINT
            public NotifyFlags uFlags; // UINT
            public System.UInt32 uCallbackMessage; // UINT
            public System.IntPtr hIcon; // HICON
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public System.String szTip; // char[128]
            public System.UInt32 dwState; // DWORD
            public System.UInt32 dwStateMask; // DWORD
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public System.String szInfo; // char[256]
            public System.UInt32 uTimeoutOrVersion; // UINT
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public System.String szInfoTitle; // char[64]
            public System.UInt32 dwInfoFlags; // DWORD
            public Guid guidItem;
            public System.IntPtr hBalloonIcon;
        }

        public enum NotifyCommand
        {
            Add = 0, Modify = 1, Delete = 2, SetFocus = 3,
            SetVersion = 4
        }
        public enum NotifyFlags
        {
            Message = 1, Icon = 2, Tip = 4, State = 8, Info = 16,
            Guid = 32
        }

        [DllImport("shell32.Dll")]
        public static extern System.Int32 Shell_NotifyIcon(NotifyCommand cmd,
                                                           ref NotifyIconData data);

        [DllImport("Kernel32.Dll")]
        public static extern System.UInt32 GetCurrentThreadId();

        public delegate System.Int32 EnumThreadWndProc(System.IntPtr hWnd,
                                                       System.UInt32 lParam);

        [DllImport("user32.Dll")]
        public static extern System.Int32 EnumThreadWindows(System.UInt32 threadId,
                                            EnumThreadWndProc callback,
                                            System.UInt32 param);

        [DllImport("user32.Dll")]
        public static extern System.Int32 GetClassName(System.IntPtr hWnd,
                                                      System.Text.StringBuilder className,
                                                      System.Int32 maxCount);

        private System.IntPtr m_notifyWindow;
        private bool m_foundNotifyWindow;

        // Win32 Callback Function
        private System.Int32 FindNotifyWindowCallback(System.IntPtr hWnd,
                                                      System.UInt32 lParam)
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(256);
            GetClassName(hWnd, buffer, buffer.Capacity);

            // but what if this changes?  - anybody got a better idea?
            Console.WriteLine(buffer.ToString());
            if (buffer.ToString().StartsWith("WindowsForms10.Window.0.app", StringComparison.InvariantCultureIgnoreCase))
            {
                m_notifyWindow = hWnd;
                m_foundNotifyWindow = true;
                return 0; // stop searching
            }
            return 1;
        }

        public void ShowBalloon(uint iconId, IntPtr icon, string title, string text, uint timeout)
        {
            // find notify window
            uint threadId = GetCurrentThreadId();
            EnumThreadWndProc cb = new EnumThreadWndProc(FindNotifyWindowCallback);
            m_foundNotifyWindow = false;
            EnumThreadWindows(threadId, cb, 0);
            if (m_foundNotifyWindow)
            {
                // show the balloon
                NotifyIconData data = new NotifyIconData();
                data.cbSize = (System.UInt32)
                              System.Runtime.InteropServices.Marshal.SizeOf(
                                                                    typeof(NotifyIconData));
                data.hWnd = m_notifyWindow;
                data.uID = iconId;
                //data.hIcon = icon;
                if (icon != IntPtr.Zero)
                {
                    data.hBalloonIcon = icon;
                }
                data.uFlags = NotifyFlags.Info;
                data.uTimeoutOrVersion = 15000;
                data.szInfo = text;
                data.szInfoTitle = title;
                data.dwInfoFlags = 0x04 | 0x20;
                Shell_NotifyIcon(NotifyCommand.Modify, ref data);
            }
        }
    }
}
