using Dyd.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Dyd
{
    /// <summary>
    /// Interaction logic for FullWindow.xaml
    /// </summary>
    public partial class FullWindow : Window
    {
        //定义一个播放对象
        private MediaElement myPlayer;

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //取得窗口的句柄
            IntPtr thisIntPtr = new WindowInteropHelper(this).Handle;

            Win32.User32.SetParent(thisIntPtr, WallpaperUtils.GetWorkerW());

            //播放
            myPlayer.Play();
        }

        /// <summary>
        /// 窗体全屏
        /// </summary>
        public FullWindow()
        {
            InitializeComponent();

            //不显示工具栏
            this.ShowInTaskbar = false;

            //绑定media
            myPlayer = media;
            //设置media的margin
            myPlayer.Margin = new Thickness(0, 0, 0, 0);

            //播放结束后，始终重新加载
            myPlayer.UnloadedBehavior = MediaState.Manual;

            this.GoFullscreen();

            this.Left = 0;
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        /// <summary>
        /// 视频资源变更
        /// </summary>
        /// <param name="uri">视频资源地址</param>
        public void ChangeSource(Uri uri)
        {
            myPlayer.Stop();
            myPlayer.Source = uri;
            myPlayer.Play();
        }

        /// <summary>
        /// 双击无反应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 窗体大小变换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ActualWidth此元素呈现宽度
            myPlayer.Width = ActualWidth;
            //ActualHeight此元素的呈现高度
            myPlayer.Height = ActualHeight;
        }

        /// <summary>
        ///使视频循环播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            //进度条设到开始
            myPlayer.Position = TimeSpan.Zero;
            //播放
            myPlayer.Play();
        }
    }
}
