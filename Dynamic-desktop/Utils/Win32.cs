/*----------------------------------------------------------------
// 文件名：
// 文件功能描述：
//
//
// 创建标识：
//
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Shapes;
using Rectangle = System.Drawing.Rectangle;

namespace Dyd.Utils
{
    public class Win32
    {
        public class User32
        {
            /// <summary>
            ///  查找顶级窗口句柄
            /// </summary>
            /// <param name="className">类名</param>
            /// <param name="titleName">标题、窗口名</param>
            /// <returns>执行成功，则返回值是拥有指定窗口类名或窗口名的窗口的句柄。如果函数执行失败，则返回值为 NULL</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string className, string titleName);

            /// <summary>
            /// 查找子窗口句柄
            /// </summary>
            /// <param name="hwndParent">要查找窗口的父句柄</param>
            /// <param name="hwndChildAfter">从这个窗口后开始查找</param>
            /// <param name="className">窗口类名</param>
            /// <param name="title">窗口标题</param>
            /// <returns>找到返回窗口句柄，没找到返回0</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string title);

            /// <summary>
            /// 枚举所有屏幕上的顶层窗口，并将窗口句柄传送给应用程序定义的回调函数
            /// </summary>
            /// <param name="lpEnumFunc">指向一个应用程序定义的回调函数指针</param>
            /// <param name="lParam">传递给回调函数的应用程序定义值</param>
            /// <returns>如果函数成功，返回值为非零；如果函数失败，返回值为零</returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]//arshalAs属性指示如何在托管代码和非托管代码之间封送数据。
            public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
            //该函数是一个与EnumWindows或EnumDesktopWindows一起使用的应用程序定义的回调函数。它接收顶层窗口句柄。WNDENUMPROC定义一个指向这个回调函数的指针。EnumWindowsProc是应用程序定义函数名的位置标志符。
            public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

            /// <summary>
            /// 改变指定子窗口的父窗口
            /// </summary>
            /// <param name="hwndChild">子窗口句柄</param>
            /// <param name="newParent">新的父窗口句柄 如果该参数是NULL，则桌面窗口就成为新的父窗口</param>
            /// <returns>如果函数成功，返回值为子窗口的原父窗口句柄；如果函数失败，返回值为NULL</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr SetParent(IntPtr hwndChild, IntPtr newParent);

            /// <summary>
            /// 显示窗口异步
            /// </summary>
            /// <param name="hWnd">窗口句柄</param>
            /// <param name="cmdShow">指定窗口显示方式</param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);//该函数设置由不同线程产生的窗口的显示状态;函数向给定窗口的消息队列发送show-window事件。应用程序可以使用这个函数避免在等待一个挂起的应用程序完成处理show-window事件时也被挂起
            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);//该函数设置指定窗口的显示状态。
            //SW_SHOW：在窗口原来的位置以原来的尺寸激活和显示窗口。nCmdShow=5。
            public const int SW_SHOW = 5;
            //SW_HIDE：隐藏窗口并激活其他窗口。nCmdShow=0。
            public const int SW_HIDE = 0;
            //SW_SHOWNORMAL：激活并显示一个窗口。如果窗口被最小化或最大化，系统将其恢复到原来的尺寸和大小。应用程序在第一次显示窗口的时候应该指定此标志。nCmdShow=1。
            public const int WS_SHOWNORMAL = 1;

            /// <summary>
            /// 激活窗口
            /// 将创建指定窗口的线程设置到前台，并且激活该窗口。
            /// </summary>
            /// <param name="hWnd">激活窗口句柄</param>
            /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns>
            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            /// <summary>
            /// 销毁一个窗口
            /// 销毁指定的窗口。这个函数通过发送WM_DESTROY 消息和 WM_NCDESTROY 消息使窗口无效并移除其键盘焦点。这个函数还销毁窗口的菜单，清空线程的消息队列，销毁与窗口过程相关的定时器，解除窗口对剪贴板的拥有权，打断剪贴板器的查看链。
            /// </summary>
            /// <param name="hWnd">窗口句柄</param>
            /// <returns>如果函数成功，返回值为非零：如果函数失败，返回值为零。</returns>
            [DllImport("user32.dll")]
            public static extern int DestroyWindow(IntPtr hWnd);

            /// <summary>
            /// 该函数返回指定窗口的边框矩形的尺寸。该尺寸以相对于屏幕坐标左上角的屏幕坐标给出
            /// </summary>
            /// <param name="hwnd">窗口句柄</param>
            /// <param name="rect">指向一个RECT结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
            [DllImport("user32.dll")]
            public static extern void GetWindowRect(IntPtr hwnd, ref Rectangle rect);

            /// <summary>
            /// 改变一个子窗口，弹出式窗口或顶层窗口的尺寸，位置和Z序。子窗口，弹出式窗口，及顶层窗口根据它们在屏幕上出现的顺序排序、顶层窗口设置的级别最高，并且被设置为Z序的第一个窗口。
            /// </summary>
            /// <param name="hWnd">在z序中的位于被置位的窗口前的窗口句柄。该参数必须为一个窗口句柄</param>
            /// <param name="hWndlnsertAfter">用于标识在z-顺序的此 CWnd 对象之前的 CWnd 对象</param>
            /// <param name="x">以客户坐标指定窗口新位置的左边界。</param>
            /// <param name="y">以客户坐标指定窗口新位置的顶边界。</param>
            /// <param name="cx">以像素指定窗口的新的宽度。</param>
            /// <param name="cy">以像素指定窗口的新的高度。</param>
            /// <param name="flag">窗口尺寸和定位的标志</param>
            [DllImport("user32.dll")]
            public static extern void SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int x, int y, int cx, int cy, uint flag);
            public const int HWND_TOP = 0; // 在前面
            public const int HWND_BOTTOM = 1; // 在后面
            public const int HWND_TOPMOST = -1; // 在前面, 位于任何顶部窗口的前面
            public const int HWND_NOTOPMOST = -2; // 在前面, 位于其他顶部窗口的后面

            /// <summary>
            /// 将指定的消息发送到一个或多个窗口。
            /// </summary>
            /// <param name="windowHandle">窗口程序将接收消息的窗口的句柄</param>
            /// <param name="Msg">被发送的消息</param>
            /// <param name="wParam">指定附加的消息指定信息</param>
            /// <param name="lParam">指定附加的消息指定信息</param>
            /// <param name="flags">指定如何发送消息</param>
            /// <param name="timeout">为超时周期指定以毫秒为单位的持续时间。如果该消息是一个广播消息，每个窗口可使用全超时周期。例如，如果指定5秒的超时周期，有3个顶层窗回未能处理消息，可以有最多15秒的延迟。</param>
            /// <param name="result">指定消息处理的结果，依赖于所发送的消息</param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

            /// <summary>
            ///如何发送消息的枚举
            /// </summary>
            [Flags]
            public enum SendMessageTimeoutFlags : uint
            {
                SMTO_ABORTIFHUNG = 2,
                SMTO_BLOCK = 1,
                SMTO_ERRORONEXIT = 0x20,
                SMTO_NORMAL = 0,
                SMTO_NOTIMEOUTIFNOTHUNG = 8
            }

            public class Winmm
            {
                /// <summary>
                ///  mciSendString是用来播放多媒体文件的API指令，可以播放MPEG,AVI,WAV,MP3,等等
                /// </summary>
                /// <param name="lpszCommand">要发送的命令字符串。字符串结构是:[命令][设备别名][命令参数]</param>
                /// <param name="lpszReturnString">返回信息的缓冲区,为一指定了大小的字符串变量</param>
                /// <param name="cchReturn">缓冲区的大小,就是字符变量的长度</param>
                /// <param name="hwndCallback">回调方式，一般设为零</param>
                /// <returns>函数执行成功返回零，否则返回错误代码</returns>
                [DllImport(("winmm.dll"), EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
                public static extern int mciSendString(string lpszCommand, string lpszReturnString,
                            uint cchReturn, int hwndCallback);
            }

            public class Kernel32
            {
                /// <summary>
                /// 获取短路径
                /// </summary>
                /// <param name="path">路径</param>
                /// <param name="short_path">返回短路径的缓冲区</param>
                /// <param name="short_len">缓冲区长度</param>
                /// <returns></returns>
                [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
                public static extern int GetShortPathName(
                    [MarshalAs(UnmanagedType.LPTStr)]string path,
                    [MarshalAs(UnmanagedType.LPTStr)]StringBuilder short_path,
                    int short_len
                    );
            }
        }
    }
}
