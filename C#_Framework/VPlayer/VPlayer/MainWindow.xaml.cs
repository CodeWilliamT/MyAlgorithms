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
using System.Windows.Forms;
using System.IO;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using Utils;
using System.Threading;

namespace WpfVideoPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : VcreditWindowBehindCode
    {
        public class FileRecord
        {
            public FileInfo FileInfo;
            public int Position;
            public FileRecord(string filename)
            {
                FileInfo = new FileInfo(filename);
                Position = 0;
            }
        }
        string title = "VPlayer By WilliamT";
        double minWidth = 630;
        double minHeight = 250;

        double preWidth = 630;
        double preHeight = 250;

        bool isWindowMax;
        float scaleX;
        bool playFileFlag;
        bool playingFlag;
        bool mourseVisible = true;
        int mourseVisibleDelay, mourseVisibleMax=6;
        int sec_logDelay, sec_logDelayMax=6;//1为500毫秒
        DateTime time_LastMouseDown;
        POINT pi;
        DispatcherTimer timer_Process;
        DispatcherTimer timer_Time;
        DispatcherTimer timer_Sub;
        List<FileInfo> List_MediaFiles;
        List<FileInfo> List_SubFiles;
        Dictionary<string,int> List_MediaFileNames;
        List<SubtitlesParser.Classes.SubtitleItem> subItems;
        List<string> supportedVideos = new List<string>()
        { ".asf", ".avi", ".wm", ".wmp", ".wmv",
            ".ram",".rm",".rmvb",".rpm",".rt",".smil",".scm",".m1v",".m2v",".m2p",".m2ts",".mp2v",".mpe",".mpeg",".mpeg1",".mpeg2",".mpg",".mpv2",".pva",".tp",
            ".tpr",".ts",".m4b",".m4r",".m4p",".m4v",".mp4",".mpeg4",".3g2",".3gp",".3gp2",".3gpp",".mov",".qt",".flv",".f4v",".swf",".hlv",".vob",
            ".amv",".csf",".divx",".evo",".mkv",".mod",".pmp",".vp6",".bik",".mts",".xlmv",".ogm",".ogv",".ogx",
            ".aac",".ac3",".acc",".aiff",".ape",".au",".cda",".dts",".flac",".m1a",".m2a",".m4a",".mka",".mp2",".mp3",".mpa",".mpc",".ra",".tta",".wav",".wma",".wv",".mid",".midi",
            ".ogg",".oga",".dvd",".vqf"};

        List<string> supportedSubs = new List<string>()
        { ".srt", ".ass", ".sub", ".vtt", ".wmv",".ram"
        };
        public string defaultDirctory;
        public int defaultVoice=50;
        public string nowFileName="";
        public string nowSubName = "";
        public int nowPosition=0;
        public int nowIdx;
        public Size defaultSize;

        public MainWindow()
        {
            InitializeComponent();
            SetupUI();
            pi = new POINT();
        }
        public double defaultWidth
        {
            get
            {
                return defaultSize.Width;
            }
            set
            {
                defaultSize = new Size(value, defaultHeight);
            }
        }
        public double defaultHeight
        {
            get
            {
                return defaultSize.Height;
            }
            set
            {
                defaultSize = new Size(defaultWidth, value);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        public void SetupUI()
        {
            ListBox_File.Margin = new Thickness(ListBox_File.Margin.Left, Canvas_Top.Height, ListBox_File.Margin.Right,  Gird_Menu.Height);
            //异常处理
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            preWidth = formWindow.Width;
            preHeight = formWindow.Height;
            Title = title;
            sec_logDelay = 0;
            isWindowMax = false;
            mourseVisible = true;
            mourseVisibleDelay = 0;
            ShowList(false);
            ShowVoiceCofig(false);
            SetTop(false);
            playFileFlag = false;
            playingFlag = false;
            time_LastMouseDown = DateTime.Now;
            defaultSize = new Size(Width, Height);
            btnOpenFolder.Visibility = Visibility.Hidden;
            Slider_Process.Visibility = Visibility.Hidden;
            btnLeft.Visibility = Visibility.Hidden;
            btnRight.Visibility = Visibility.Hidden;
            Label_Process.Visibility = Visibility.Hidden;
            timer_Process = new DispatcherTimer();
            timer_Process.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer_Process.Tick += timer_Process_tick;
            timer_Sub = new DispatcherTimer();
            timer_Sub.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer_Sub.Tick += timer_Sub_tick;
            timer_Time = new DispatcherTimer();
            timer_Time.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer_Time.Tick += timer_Time_tick;
            timer_Time.Start();
            btnLast.IsEnabled = false;
            btnNext.IsEnabled = false;
            btnStop.IsEnabled = false;
            AppConfigHelper.LoadObj(this);
            if (defaultVoice == 0)
            {
                defaultVoice = 50;
                Slider_Voice.Value = 50;
            }
            defaultDirctory = defaultDirctory.Replace("\0", "");
            List_MediaFiles=new List<FileInfo>();
            List_SubFiles = new List<FileInfo>();
            List_MediaFileNames =new Dictionary<string, int>();
            RefreshList(defaultDirctory);
        }
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            if (ex != null)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

        }

        private void timer_Process_tick(object sender, EventArgs e)
        {
            if (Player.Source == null) return;
            Slider_Process.Value = (int)Player.Position.TotalSeconds;
            Label_Process.Content = Player.Position.ToString("hh\\:mm\\:ss") + "/" + Player.NaturalDuration.TimeSpan.ToString("hh\\:mm\\:ss");
        }

        private void timer_Sub_tick(object sender, EventArgs e)
        {
            if (Player.Source == null) return;
            if (subItems == null || subItems.Count() == 0) return;
            double msec = Player.Position.TotalMilliseconds;
            StringBuilder s = new StringBuilder();
            foreach (var subItem in subItems)
            {
                if (msec > subItem.StartTime && msec < subItem.EndTime)
                {
                    s.Append(subItem.PlaintextLines[0]);
                    for(int i= 1;i < subItem.PlaintextLines.Count();i++)
                    {
                        s.Append("\n"+subItem.PlaintextLines[i]);
                    }
                }
            }
            tbSub.Text = s.ToString();
        }
        private void timer_Time_tick(object sender, EventArgs e)
        {
            label_time.Content = DateTime.Now.ToString("HH:mm:ss");
            if (sec_logDelay == sec_logDelayMax + 3) return;
            else if(sec_logDelay < sec_logDelayMax) sec_logDelay++;
            else
            {
                textBlock_Log.Text = "";
            }
            if (!mourseVisible) return;
            else if (mourseVisibleDelay < mourseVisibleMax) mourseVisibleDelay++;
            else
            {
                ShowCursor(0);
                mourseVisible = false;
            }
        }
        #region 隐藏鼠标的方法 0/1 隐藏/显示
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public static extern void ShowCursor(int status);
        #endregion
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if(btnOpenFolder.Visibility==Visibility.Hidden)
            {
                btnOpenFolder.Visibility = Visibility.Visible;
            }
            else
            {
                btnOpenFolder.Visibility = Visibility.Hidden;
            }
        }

        private void formWindow_ContentRendered(object sender, EventArgs e)
        {
            if (Program.startStr != null)
                if (Program.startStr.Length > 0)
                {
                    OpenMediaFile(Program.startStr[0]);
                }
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            timer_Time.Stop();
            OpenFileDialog ofd = new OpenFileDialog();
            if (Directory.Exists(defaultDirctory)) ofd.InitialDirectory = defaultDirctory;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenMediaFile(ofd.FileName);
            }
            timer_Time.Start();
        }
        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            timer_Time.Stop();
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            if (Directory.Exists(defaultDirctory)) Fbd.SelectedPath = defaultDirctory;
            if (Fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenFolder(Fbd.SelectedPath);
            }
            timer_Time.Start();
        }

        private void btnSubOpen_Click(object sender, RoutedEventArgs e)
        {
            timer_Time.Stop();
            btnSubOpen.Visibility = Visibility.Hidden;
            btnSubVisible.Visibility = Visibility.Hidden;
            OpenFileDialog ofd = new OpenFileDialog();
            if (Directory.Exists(defaultDirctory)) ofd.InitialDirectory = defaultDirctory;
            
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenSubFile(ofd.FileName);
            }
            timer_Time.Start();
        }
        private void btnSubVisible_Click(object sender, RoutedEventArgs e)
        {
            if (tbSub.Visibility == Visibility.Visible)
            {
                LogInfo("已隐藏字幕");
                tbSub.Text = "显示字幕";
                menuSubVisible.IsChecked = true;
                tbSub.Visibility = Visibility.Hidden;
            }
            else
            {
                LogInfo("已隐藏字幕");
                tbSub.Text = "隐藏字幕";
                menuSubVisible.IsChecked = false;
                tbSub.Visibility = Visibility.Visible;
            }
            btnSubOpen.Visibility = Visibility.Hidden;
            btnSubVisible.Visibility = Visibility.Hidden;
        }
        
        private void Control_MouseEnter(object sender, EventArgs e)
        {
            ((System.Windows.Controls.Control)sender).Opacity = 1;

        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            ((System.Windows.Controls.Control)sender).Opacity = 0;

        }

        private void Menu_MouseEnter(object sender, EventArgs e)
        {
            Canvas_Top.Opacity = 1;
            Gird_Menu.Opacity = 1;
            btnShowList.Opacity = 1;
            //panel.IsEnabled = true;

        }

        private void Menu_MouseLeave(object sender, EventArgs e)
        {
            Canvas_Top.Opacity = 0;
            Gird_Menu.Opacity = 0;
            if(ListBox_File.Visibility == Visibility.Hidden)
                btnShowList.Opacity = 0;
            Task.Run(new Action(() =>
            {
                Thread.Sleep(5000);
                Dispatcher.Invoke(() =>
                {
                    btnSubOpen.Visibility = Visibility.Hidden;
                    btnSubVisible.Visibility = Visibility.Hidden;
                });
            }));
            //panel.IsEnabled = false;

        }

        private void btnShowList_Click(object sender, RoutedEventArgs e)
        {
            ShowList(btnShowList.Content.ToString() == "<");
        }

        private void btnVoice_Click(object sender, RoutedEventArgs e)
        {
            ShowVoiceCofig(!((bool)btnVoice.Tag));
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            SetWindowMax();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            formWindow.Close();
        }

        private void btnTop_Click(object sender, RoutedEventArgs e)
        {
            SetTop(!Topmost);
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Source == null)
            {
                if (List_MediaFiles == null) return;
                if (List_MediaFiles.Count == 0) return;
                OpenMediaFile(nowFileName);
                return;
            }
            SetVideoMode(!playingFlag);
        }

        private void Player_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnStart_Click(null, null);
        }
        

        private void Slider_Process_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            SetVideoMode(false);
        }
        

        private void Slider_Process_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Player.Source == null) return;
            Player.Position = TimeSpan.FromSeconds(Slider_Process.Value);
            SetVideoMode(true);
        }

        private void Slider_Vioce_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Player.Source == null) return;
            Player.Volume = 1.0 * Slider_Voice.Value / Slider_Voice.Maximum;
            if(Slider_Voice.Value>0) defaultVoice = (int)Slider_Voice.Value;
            LogInfo("音量:" + Player.Volume.ToString("P0"));
        }
        

        private void Slider_Process_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SetVideoMode(false);
            }
        }

        private void Slider_Process_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Player.Source == null) return;
            Player.Position = TimeSpan.FromSeconds(Slider_Process.Value);
            LogInfo("跳至  " + Player.Position.ToString("hh\\:mm\\:ss") + "(" + (Slider_Process.Value / Slider_Process.Maximum).ToString("P2") + ")");
            SetVideoMode(true);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Source == null) return;
            tbSub.Text = "";
            SetVideoMode(false);
            if (Player.Source != null)
                if (Player.NaturalDuration.HasTimeSpan)
                {
                    AppConfigHelper.UpdateKey("FileRecord-" + nowFileName, Player.Position.TotalMilliseconds.ToString());
                    if(subItems != null && subItems.Count()>0)AppConfigHelper.UpdateKey("FileRecordSub-" + nowFileName, nowSubName);
                    
                }
            Player.Stop();
            Player.Source = null;
            if (subItems != null)
            {
                subItems.Clear();
                tbSub.Text = "";
            }
            Label_Process.Content = "--:--:--/ --:--:--";
            label_NowFile.Content = "VPlayer";
            Slider_Process.Value = 0;
            playFileFlag = false;
            playingFlag = false;
            Player.Close();
            Slider_Process.Visibility = Visibility.Hidden;
            btnLeft.Visibility = Visibility.Hidden;
            btnRight.Visibility = Visibility.Hidden;
            Label_Process.Visibility = Visibility.Hidden;
            btnShowList.Opacity = 1;
            btnShowList.MouseEnter -= Control_MouseEnter;
            btnShowList.MouseLeave -= Control_MouseLeave;
            SetPanelMotionVisible(Canvas_Top, false);
            SetPanelMotionVisible(Gird_Menu, false);
            Canvas_File.Visibility = Visibility.Visible;
            btnLast.IsEnabled = false;
            btnNext.IsEnabled = false;
            ListBox_File.Margin = new Thickness(ListBox_File.Margin.Left, Canvas_Top.Height, ListBox_File.Margin.Right, Gird_Menu.Height);
        }

        private void formWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.S:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                        btnStop_Click(null, null);
                    break;
                case Key.M:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                        SetVideoMax(true);
                    break;
                case Key.Space:
                        btnStart_Click(null, null);
                    break;
                case Key.Up:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                        Slider_Speed.Value += Slider_Speed.Interval;
                    else
                        Slider_Voice.Value += Slider_Voice.Interval;
                    break;
                case Key.Down:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                        Slider_Speed.Value -= Slider_Speed.Interval;
                    else
                        Slider_Voice.Value -= Slider_Voice.Interval;
                    break;
                case Key.Left:
                    btnLeft_Click(null, null);
                    break;
                case Key.Right:
                    btnRight_Click(null, null);
                    break; ;
                case Key.Escape:
                    SetVideoMax(false);
                    break;
                case Key.PageUp:
                    btnLast_Click(null,null);
                    break;
                case Key.PageDown:
                    btnNext_Click(null, null);
                    break;
                    //default:
                    //    break;
            }
        }

        private void formWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Player.Source != null)
                if (Player.NaturalDuration.HasTimeSpan)
                {
                    AppConfigHelper.UpdateKey("FileRecord-" + nowFileName, Player.Position.TotalMilliseconds.ToString());
                    if (subItems != null && subItems.Count() > 0) AppConfigHelper.UpdateKey("FileRecordSub-" + nowFileName, nowSubName);
                }
                AppConfigHelper.SaveObj(this);
        }

        private void ListBox_File_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBox_File.SelectedIndex == -1) return;

            OpenMediaFile(List_MediaFiles[ListBox_File.SelectedIndex].FullName);
        }

        private void btnScreen_Click(object sender, RoutedEventArgs e)
        {
            SetVideoMax(WindowState==WindowState.Normal);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            SetVideoMode(false);
            Slider_Process.Value += Slider_Process.Interval;
            Player.Position = TimeSpan.FromSeconds(Slider_Process.Value);
            LogInfo("快进  " + Player.Position.ToString("hh\\:mm\\:ss")+" (" + (Slider_Process.Value / Slider_Process.Maximum).ToString("P2") + ")");
            SetVideoMode(true);
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            SetVideoMode(false);
            Slider_Process.Value -= Slider_Process.Interval;
            Player.Position = TimeSpan.FromSeconds(Slider_Process.Value);
            LogInfo("快退  " + Player.Position.ToString("hh\\:mm\\:ss") +" (" + (Slider_Process.Value / Slider_Process.Maximum).ToString("P2") + ")");
            SetVideoMode(true);
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (nowIdx < 1) return;
            OpenMediaFile(List_MediaFiles[nowIdx - 1].FullName);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (nowIdx > List_MediaFiles.Count-2) return;
            OpenMediaFile(List_MediaFiles[nowIdx + 1].FullName);
        }
        

        private void btn_Silence_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Source == null) return;
            if(Slider_Voice.Value==0)
            {
                Slider_Voice.Value = defaultVoice;
                btn_Silence.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Sound.png")));
            }
            else
            {
                LogInfo("已静音");
                Slider_Voice.Value = 0;
                btn_Silence.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Silence.png")));
            }
        }

        private void Slider_Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Player == null) return;
            if (Player.Source == null) return;
            Player.SpeedRatio = (double)(Slider_Speed.Value/10);
            textBlock_speed.Text = ((double)Slider_Speed.Value/10).ToString("F1");
            LogInfo("倍速：  " + Player.SpeedRatio.ToString("F1")+"倍速");
        }


        private void formWindow_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {
            //if (Mouse.LeftButton == MouseButtonState.Pressed) return;
            string path = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).GetValue(0).ToString();
            FileInfo fileInfo = new FileInfo(path);
            if ((fileInfo.Attributes & FileAttributes.Directory) == 0)
            {
                if (supportedVideos.Contains(fileInfo.Extension.ToLower()))
                    OpenMediaFile(path);
                if (supportedSubs.Contains(fileInfo.Extension.ToLower()))
                    OpenSubFile(path);
            }
            else
            {
                OpenFolder(path);
            }
        }

        private void btnSub_Click(object sender, RoutedEventArgs e)
        {
            if (btnSubOpen.Visibility == Visibility.Hidden)
            {
                btnSubOpen.Visibility = Visibility.Visible;
                btnSubVisible.Visibility = Visibility.Visible;
            }
            else
            {
                btnSubOpen.Visibility = Visibility.Hidden;
                btnSubVisible.Visibility = Visibility.Hidden;
            }
        }


        private void formWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (formWindow.Width < minWidth) formWindow.Width = minWidth;
            if (formWindow.Height < minHeight) formWindow.Height = minHeight;
            double sizePercent = formWindow.Height / preHeight;

            tbSub.FontSize = sizePercent * tbSub.FontSize;
            preWidth = formWindow.Width;
            preHeight = formWindow.Height;
            if (Player == null) return;
            if (Player.Source == null) return;
            double width_Percent = Player.ActualWidth / Player.NaturalVideoWidth;
            double height_Percent = Player.ActualHeight / Player.NaturalVideoHeight;
            double video_Percent = width_Percent * height_Percent;
            int mediaWidth = (int)Player.ActualWidth, mediaHeight = (int)Player.ActualHeight;
            string str = mediaWidth.ToString() + "×" + mediaHeight.ToString() + "(" + video_Percent.ToString("p") + ")";
            LogInfo(str);
        }

        private void Grid_Form_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DateTime.Now.Subtract(time_LastMouseDown).TotalMilliseconds > 300)
            {
                time_LastMouseDown = DateTime.Now;
            }
            else
            {
                SetVideoMax(WindowState == WindowState.Normal);
            }
            if (e.LeftButton == MouseButtonState.Pressed&&WindowState== WindowState.Normal)
            {
                this.DragMove();
            }
        }


        private void Grid_Form_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!mourseVisible)
            {
                ShowCursor(1);
                mourseVisible = true;
                mourseVisibleDelay = 0;
            }
        }

        #region UI Functions

        /// <summary>
        /// 刷新文件列表
        /// </summary>
        /// <param name="DirctoryName"></param>
        private void RefreshList(string DirctoryName)
        {
            try
            {
                DirectoryInfo Path = new DirectoryInfo(DirctoryName);
                FileInfo[] Dir = Path.GetFiles("*", SearchOption.AllDirectories);

                List_SubFiles.Clear();
                List_MediaFiles.Clear();
                List_MediaFileNames.Clear();
                ListBox_File.Items.Clear();
                int i = 0;
                foreach (FileInfo d in Dir)
                {
                    if (supportedVideos.Contains(d.Extension.ToLower()))
                    {
                        ListBox_File.Items.Add(d.Extension.Replace(".", "").ToLower() + "-" + d.Name);
                        List_MediaFiles.Add(d);
                        List_MediaFileNames.Add(d.FullName, i);
                        i++;
                    }
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// 打印Log信息
        /// </summary>
        /// <param name="str"></param>
        private void LogInfo(string str)
        {
            textBlock_Log.Text = str;
            sec_logDelay = 0;
        }
        /// <summary>
        /// 播放媒体文件
        /// </summary>
        /// <param name="fileName"></param>
        private void OpenMediaFile(string fileName)
        {
            try
            {
                if (fileName==null||fileName == "") return;
                if (!File.Exists(fileName)) return;
                if (playFileFlag)
                {
                    if (Player.Source != null)
                        if (Player.NaturalDuration.HasTimeSpan)
                        {
                            AppConfigHelper.UpdateKey("FileRecord-" + nowFileName, Player.Position.TotalMilliseconds.ToString());
                            if (subItems != null && subItems.Count() > 0) AppConfigHelper.UpdateKey("FileRecordSub-" + nowFileName, nowSubName);
                        }
                    SetVideoMode(false);
                    Player.Stop();
                    Player.Close();
                }
                FileInfo fileInfo = new FileInfo(fileName);
                if (!supportedVideos.Contains(fileInfo.Extension.ToLower()))
                {
                    return;
                }
                //更新媒体目录
                if (!List_MediaFileNames.ContainsKey(fileInfo.FullName))
                {
                    defaultDirctory = fileInfo.DirectoryName;
                    RefreshList(defaultDirctory);
                }
                label_NowFile.Content = fileInfo.Name;
                nowFileName = fileInfo.FullName;
                nowPosition = (int)AppConfigHelper.LoadDouble("FileRecord-"+nowFileName);
                nowIdx = List_MediaFileNames[fileInfo.FullName];
                Player.LoadedBehavior = MediaState.Manual;
                Player.Source = new Uri(fileName);
                Slider_Voice.Value = defaultVoice;
                Player.Volume = 1.0 * Slider_Voice.Value / Slider_Voice.Maximum;

                if (subItems != null)
                {
                    subItems.Clear();
                    tbSub.Text = "";
                }
                string subfile = AppConfigHelper.LoadKey("FileRecordSub-" + nowFileName);
                if (subfile != "")
                    OpenSubFile(subfile);
                else
                {
                    string fileNoExPath = fileName.Remove(fileName.Length - fileInfo.Extension.Length, fileInfo.Extension.Length);
                    foreach (var subEx in supportedSubs)
                    {
                        if (OpenSubFile(fileNoExPath + subEx))
                            break;
                    }
                }
                SetVideoMode(true);
                playFileFlag = true;
                DateTime t = DateTime.Now;
                while (!Player.NaturalDuration.HasTimeSpan)//等待媒体开始播放
                {
                    System.Threading.Thread.Sleep(50);
                    if (DateTime.Now.Subtract(t).TotalMilliseconds > 5000)
                    {
                        btnStop_Click(null, null);
                        System.Windows.MessageBox.Show("文件损坏，或者不支持此种视频格式");
                        return;
                    }
                }
                Player.Position = new TimeSpan(0, 0, 0, 0, nowPosition);
                //formWindow.Width = Player.NaturalVideoWidth+14;
                //formWindow.Height = Player.NaturalVideoHeight+14;
                Slider_Process.Maximum = (int)Player.NaturalDuration.TimeSpan.TotalSeconds;
                Slider_Process.Visibility = Visibility.Visible;
                btnLeft.Visibility = Visibility.Visible;
                btnRight.Visibility = Visibility.Visible;
                Label_Process.Visibility = Visibility.Visible;

                if (ListBox_File.Visibility == Visibility.Hidden)
                {
                    btnShowList.Opacity = 0;
                    btnShowList.MouseEnter += Control_MouseEnter;
                    btnShowList.MouseLeave += Control_MouseLeave;
                }
                else
                {
                    Canvas_Top.Visibility = Visibility.Hidden;
                    Gird_Menu.Visibility = Visibility.Hidden;
                }
                SetPanelMotionVisible(Canvas_Top, true);
                SetPanelMotionVisible(Gird_Menu, true);
                ShowList(false);
                Canvas_File.Visibility = Visibility.Hidden;
                btnStop.IsEnabled = true;
                if (nowIdx != 0)
                    btnLast.IsEnabled = true;
                if (nowIdx != List_MediaFiles.Count - 1)
                    btnNext.IsEnabled = true;
                ListBox_File.Margin = new Thickness(ListBox_File.Margin.Left, 0, ListBox_File.Margin.Right, 0);
                //panel1.Visible = true;
                //btn_OpenFile.Visible = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }


        private void OpenFolder(string folderName)
        {
            try
            {
                DirectoryInfo Path = new DirectoryInfo(folderName);
                FileInfo[] Dir = Path.GetFiles("*", SearchOption.AllDirectories);

                List<FileInfo> List_MediaFilesTmp = new List<FileInfo>();
                List_SubFiles.Clear();

                foreach (FileInfo d in Dir)
                {
                    if (supportedVideos.Contains(d.Extension.ToLower()))
                    {
                        List_MediaFilesTmp.Add(d);
                    }
                    if (supportedSubs.Contains(d.Extension.ToLower()))
                    {
                        List_SubFiles.Add(d);
                    }
                }
                if (List_MediaFilesTmp.Count == 0)
                {
                    if (List_SubFiles.Count > 0)
                        OpenSubFile(List_SubFiles.First().FullName);
                }
                else
                {
                    RefreshList(defaultDirctory);
                    defaultDirctory = folderName;
                    if (List_MediaFileNames.Count() > 0)
                        OpenMediaFile(List_MediaFileNames.First().Key);
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// 载入字幕文件
        /// </summary>
        /// <param name="fileName"></param>
        private bool OpenSubFile(string fileName)
        {
            if (Player.Source == null) return false;
            if (fileName == null || fileName == "") return false;
            if (!File.Exists(fileName)) return false;
            nowSubName= fileName;
            FileInfo fileInfo = new FileInfo(fileName);
            if (!supportedSubs.Contains(fileInfo.Extension.ToLower()))
            {
                return false;
            }
            try
            {
                if (subItems != null) 
                    subItems.Clear();
                var subParse = new SubtitlesParser.Classes.Parsers.SubParser();
                using (StreamReader sr = new StreamReader(fileName)) {
                    subItems = subParse.ParseStream(sr.BaseStream);
                }
                LogInfo("已加载字幕" + fileInfo.Name);
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 使得容器控件悬浮可见或长期可见
        /// </summary>
        /// <param name="panel">容器</param>
        /// <param name="flag">true移动至容器表面可见，false恢复至长期可见</param>
        private void SetPanelMotionVisible(System.Windows.Controls.Panel panel, bool flag)
        {
            if (flag)
            {
                panel.Opacity = 0;
                panel.MouseEnter += Menu_MouseEnter;
                panel.MouseLeave += Menu_MouseLeave;
            }
            else
            {
                panel.Opacity = 1;
                panel.MouseEnter -= Menu_MouseEnter;
                panel.MouseLeave -= Menu_MouseLeave;
            }
        }
        /// <summary>
        /// 显示文件列表
        /// </summary>
        /// <param name="flag"></param>
        private void ShowList(bool flag)
        {

            if (flag)
            {
                ListBox_File.Visibility = Visibility.Visible;
                btnShowList.Margin = new Thickness(btnShowList.Margin.Left,
                    btnShowList.Margin.Top, ListBox_File.Width + ListBox_File.Margin.Right, btnShowList.Margin.Bottom);
                btnShowList.Content = ">";

                btnShowList.Opacity = 1;
                btnShowList.MouseEnter -= Control_MouseEnter;
                btnShowList.MouseLeave -= Control_MouseLeave;
                if (playFileFlag)
                {
                    Canvas_Top.Visibility = Visibility.Hidden;
                    Gird_Menu.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                if (playFileFlag)
                {
                    btnShowList.Opacity = 0;
                    btnShowList.MouseEnter += Control_MouseEnter;
                    btnShowList.MouseLeave += Control_MouseLeave;
                    Canvas_Top.Visibility = Visibility.Visible;
                    Gird_Menu.Visibility = Visibility.Visible;
                }
                ListBox_File.Visibility = Visibility.Hidden;
                btnShowList.Margin = new Thickness(btnShowList.Margin.Left,
                    btnShowList.Margin.Top, 0, btnShowList.Margin.Bottom);
                btnShowList.Content = "<";
            }
        }

        private void Player_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int val = (int)Slider_Voice.Value+e.Delta/24;
            Slider_Voice.Value = val < 0 ? 0 : val > 100 ? 100 : val;
        }

        /// <summary>
        /// 显示音量控制器
        /// </summary>
        /// <param name="flag"></param>
        private void ShowVoiceCofig(bool flag)
        {

            if (flag)
            {
                Canvas_Voice.Visibility = Visibility.Visible;
                btnVoice.Tag = true;
            }
            else
            {
                Canvas_Voice.Visibility = Visibility.Hidden;
                btnVoice.Tag = false;
            }
        }
        private void SetVideoMax(bool flag)
        {
            ControlTemplate customWindowTemplate = App.Current.Resources["CustomWindowTemplete"] as ControlTemplate;
            var grid = customWindowTemplate.FindName("ResizingGrid", this) as Grid;
            if (flag)
            {
                grid.RowDefinitions[0].Height = new GridLength(0);
                grid.RowDefinitions[2].Height = new GridLength(0);
                grid.ColumnDefinitions[0].Width = new GridLength(0);
                grid.ColumnDefinitions[2].Width = new GridLength(0);
                WindowState = WindowState.Maximized;
                btnScreen.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Return.png")));
                menuScreen.Header = "退出全屏";
            }
            else
            {
                grid.RowDefinitions[0].Height = new GridLength(1);
                grid.RowDefinitions[2].Height = new GridLength(1);
                grid.ColumnDefinitions[0].Width = new GridLength(1);
                grid.ColumnDefinitions[2].Width = new GridLength(1);
                WindowState = WindowState.Normal;
                btnScreen.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Screen.png")));
                menuScreen.Header = "全屏";
            }
        }
        private void SetWindowMax()
        {
            ControlTemplate customWindowTemplate = App.Current.Resources["CustomWindowTemplete"] as ControlTemplate;
            var grid = customWindowTemplate.FindName("ResizingGrid", this) as Grid;
            Rect rect = SystemParameters.WorkArea;
            if (!isWindowMax)
            {
                btnMax.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Return.png")));
                grid.RowDefinitions[0].Height = new GridLength(0);
                grid.RowDefinitions[2].Height = new GridLength(0);
                grid.ColumnDefinitions[0].Width = new GridLength(0);
                grid.ColumnDefinitions[2].Width = new GridLength(0);
                WindowState = WindowState.Normal;
                this.Width = rect.Width+22;
                this.Height = rect.Height+22;
                this.Top = -11;
                this.Left = -11;
            }
            else
            {
                btnMax.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Max.png")));
                grid.RowDefinitions[0].Height = new GridLength(1);
                grid.RowDefinitions[2].Height = new GridLength(1);
                grid.ColumnDefinitions[0].Width = new GridLength(1);
                grid.ColumnDefinitions[2].Width = new GridLength(1);
                WindowState = WindowState.Normal;
                this.Width = defaultWidth;
                this.Height = defaultHeight;
                this.Top = (rect.Height - defaultHeight) / 2;
                this.Left = (rect.Width - defaultWidth) / 2;

            }
            btnScreen.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Screen.png")));
            isWindowMax = !isWindowMax;
        }

        /// <summary>
        /// 配置是否显示最前
        /// </summary>
        /// <param name="flag"></param>
        private void SetTop(bool flag)
        {

            Topmost = flag;
            menuTop.IsChecked = flag;
            if (flag)
            {
                btnTop.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Top.png")));
            }
            else
            {
                btnTop.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/NotTop.png")));
            }
        }
        /// <summary>
        /// 使得视频播放或暂停
        /// </summary>
        /// <param name="flag"></param>
        private void SetVideoMode(bool flag)
        {

            if (flag)
            {
                Player.Play();
                timer_Process.Start();
                timer_Sub.Start();
                btnStart.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Pause.png")));
                playingFlag = true;
            }
            else
            {
                timer_Process.Stop();
                timer_Sub.Stop();
                Player.Pause();
                btnStart.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Start.png")));
                playingFlag = false;
            }
        }
        #endregion
    }
}
