using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;   //安装MahApps.Metro: PM> Install-Package MahApps.Metro
using System.Windows.Forms;
using ContextMenu = System.Windows.Forms.ContextMenu; //获取或设置与控件关联的快捷菜单
using MenuItem = System.Windows.Forms.MenuItem; //MenuItem类 提供了使您可以配置的外观和功能的菜单项的属性
namespace Dyd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //设置media未加载行为；
            media.UnloadedBehavior = MediaState.Manual;//Manual: 用于手动控制 MediaElement 的状态。 可以使用交互式方法（如 Play 和 Pause）;

            fullWindow = new FullWindow();
        }

        //壁纸窗口
        private FullWindow fullWindow;

        //当前视频源
        private string currentAudioPath;

        //托盘图标
        private static NotifyIcon trayIcon; //NotifyIcon类 指定可在通知区域创建图标的组件。 NotifyIcon类不能被继承。

        /// <summary>
        /// 移除通知栏图标，释放资源
        /// </summary>
        private void RemoveTrayIcon()
        {
            //判断通知栏图标是否存在
            if (trayIcon != null)
            {
                //隐藏图标
                trayIcon.Visible = false;
                //释放由 Component 使用的所有资源
                trayIcon.Dispose();
                //释放NotifyIcon对象
                trayIcon = null;
            }
        }

        /// <summary>
        /// 将图标添加到通知区域，即系统托盘
        /// </summary>
        private void AddTrayIcon()
        {
            //若果图标已存在于通知区域则返回。
            if (trayIcon != null)
            {
                return;
            }

            //新建通知图标对象
            trayIcon = new NotifyIcon
            {
                Icon = Dynamic_desktop.Properties.Resources.Dyd,
                Text = "Dynamic desktop"
            };

            //通知栏图标可见
            trayIcon.Visible = true;

            //注册鼠标单击事件
            trayIcon.MouseClick += TrayIcon_MouseClick;

            //新建快捷菜单对象
            ContextMenu menu = new ContextMenu();

            //新建关闭菜单项
            MenuItem closeItem = new MenuItem();

            //新建关于菜单项
            MenuItem aboutItem = new MenuItem();

            //设置关闭菜单项文字
            closeItem.Text = "关闭";

            //设置关于菜单项文字
            aboutItem.Text = "关于我们";

            //关闭菜单项单击事件实现
            closeItem.Click += new EventHandler(delegate
            {
                //移除通知栏图标
                RemoveTrayIcon();

                //终止此进程，并将退出代码返回到操作系统
                Environment.Exit(0); //Environment类 提供有关当前环境和平台的信息以及操作它们的方法。 无法继承此类。
            });

            menu.MenuItems.Add(closeItem);

            trayIcon.ContextMenu = menu;    //设置NotifyIcon的右键弹出菜单
        }
        /// <summary>
        /// 窗体加载处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //添加图标到通知栏
            AddTrayIcon();
        }

        /**********************************************
         * 事件处理
         **********************************************/
         /// <summary>
         /// 单击通知栏图标
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void TrayIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ///将当前窗体设为可见
            this.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 点击关闭按钮发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //取消关闭事件
            e.Cancel = true;
            media.Close();
            //隐藏当前窗口
            this.Visibility = Visibility.Hidden;
        }

        //单击视频
        private void media_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //判断是否可以暂停
            if (media.CanPause)
            {
                //暂停播放
                media.Pause();
            }
            else
            {
                media.Play();
            }
        }

        //设为壁纸
        private void SetWallpaper(object sender, RoutedEventArgs e)
        {
            //如果当前视频源不为空，更换fullWindow视频源；
            if (currentAudioPath != null)
                fullWindow.ChangeSource(new Uri(currentAudioPath));
            //展示更换fullWindow视频源;
            fullWindow.Show();
            media.Close();
            //隐藏自身;
            //this.Visibility = Visibility.Hidden;

        }

        //选择视频
        private void SelectVideo(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog打开文件对话框控件;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //设置当前文件名筛选器字符串;
            openFileDialog.Filter = "视频|*.mp4;*.wmv";
            //指示对话框是否允许选择多个文件;
            openFileDialog.Multiselect = false;

            //DialogResult指定标识符来指示对话框中的返回值; ShowDialog()用默认的所有者运行通用对话框;
            DialogResult dialogResult = openFileDialog.ShowDialog();
            //如果返回值是OK;
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //设置当前视频源
                currentAudioPath = openFileDialog.FileName;
                //media停止播放替换视频源;
                media.Stop();
                media.Source = new Uri(currentAudioPath);
                media.Play();
            }
        }

        /***********************开关事件*************************/
        private void btnFull_Checked(object sender, RoutedEventArgs e)
        {
            //如果存在fullWindow，则显示壁纸
            if (fullWindow != null)
            {
                //播放动画
                fullWindow.media.Play();
                //将fullWindow窗口设为可见;
                fullWindow.Visibility = Visibility.Visible;
            }
                
        }

        private void btnFull_Unchecked(object sender, RoutedEventArgs e)
        {
            //将fullWindow窗口设为隐藏;
            fullWindow.Visibility = Visibility.Hidden;
            //暂停动画
            fullWindow.media.Pause();
        }
    }

}
