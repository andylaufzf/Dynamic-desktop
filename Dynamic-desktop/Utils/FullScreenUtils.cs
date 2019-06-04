/*----------------------------------------------------------------
// 文件名：FullScreenUtils.cs
// 文件功能描述：Wpf应用程序全屏辅助类
//
//
// 创建标识：星寒starcold
//
// 修改标识：
// 修改描述：
//
//----------------------------------------------------------------*/
using Dyd.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Dyd.Utils
{
    public static class FullScreenUtils
    {
        //窗体对象
        private static Window _fullWindow;
        //窗体状态
        private static WindowState _windowState;
        //窗体样式
        private static WindowStyle _windowStyle;
        //是否为最顶层元素
        private static bool _windowTopMost;
        //是否可以调整窗口大小
        private static ResizeMode _windowResizeMode;
        //用一个矩形保存窗口边缘信息
        private static Rect _windowRect;


        /// <summary>
        /// 进入全屏
        /// </summary>
        /// <param name="window"></param>
        public static void GoFullscreen(this Window window)
        {
            //已经是全屏
            if (window.IsFullscreen()) return;

            //存储窗体信息
            _windowState = window.WindowState;
            _windowStyle = window.WindowStyle;
            _windowTopMost = window.Topmost;
            _windowResizeMode = window.ResizeMode;
            _windowRect.X = window.Left;
            _windowRect.Y = window.Top;
            _windowRect.Width = window.Width;
            _windowRect.Height = window.Height;

           //假如已经是Maximized，就不能进入全屏，所以这里先调整状态
            window.WindowState = WindowState.Normal;
            //变成无边窗体
            window.WindowStyle = WindowStyle.None;
            //设定为不能调整大小
            window.ResizeMode = ResizeMode.NoResize;
            //最大化后总是在最上面
            window.Topmost = true;

            //获取窗口句柄 
            var handle = new WindowInteropHelper(window).Handle;

            //获取当前显示器屏幕
            Screen screen = Screen.FromHandle(handle);

            //调整窗口最大化,全屏的关键代码就是下面3句
            window.MaxWidth = screen.Bounds.Width;
            window.MaxHeight = screen.Bounds.Height;
            window.WindowState = WindowState.Maximized;

            //解决切换应用程序的问题
            window.Activated += new EventHandler(window_Activated);//Activated事件在窗口成为前台窗口时发生
            window.Deactivated += new EventHandler(window_Deactivated);//Deactivated事件在窗口成为后台窗口时发生。

            //记住成功最大化的窗体
            _fullWindow = window;
        }

        /// <summary>
        /// 前台窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void window_Activated(object sender, EventArgs e)
        {
            var window = sender as Window;
            //窗口最顶层
            window.Topmost = true;
        }

        //后台窗口时事件
        static void window_Deactivated(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Topmost = false;
        }

        /// <summary>
        /// 退出全屏
        /// </summary>
        /// <param name="window"></param>
        public static void ExitFullscreen(this Window window)
        {
            //已经不是全屏无操作
            if (!window.IsFullscreen()) return;

            //恢复窗口先前信息，这样就退出了全屏
            window.Topmost = _windowTopMost;
            window.WindowStyle = _windowStyle;
            //设置为可调整窗体大小
            window.ResizeMode = ResizeMode.CanResize;
            window.Left = _windowRect.Left;
            window.Width = _windowRect.Width;
            window.Top = _windowRect.Top;
            window.Height = _windowRect.Height;
            //恢复窗口状态信息
            window.WindowState = _windowState;
            //恢复窗口可调整信息
            window.ResizeMode = _windowResizeMode;

            //移除不需要的事件
            window.Activated -= window_Activated;
            window.Deactivated -= window_Deactivated;

            _fullWindow = null;
        }
        /// <summary>
        /// 窗体是否在全屏状态
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static bool IsFullscreen(this Window window)
        {
            //若果窗口不存在
            if (window == null)
            {
                //抛出ArgumentNullException异常
                throw new ArgumentNullException("window不存在！");
            }
            return _fullWindow == window;
        }
    }
}
