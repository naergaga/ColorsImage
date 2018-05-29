using ColorsImage.Data;
using ColorsImage.Util;
using ColorsImage.Util.Logger;
using ColorsImage.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ColorsImage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private CancellationTokenSource _cts;
        private Task _readingTask;
        public MainViewModel Model { get; set; } = MainViewModel.GetDefault();

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ClearLog();
            var reader = new ImageReader();
            reader.Logger = new Logger1(Log);
            var path = TbPath.Text;
            var startTime = TimeSpan.Parse(TbStartTime.Text);
            var fps = TbFPS.Text;
            var folderName = TbFolderName.Text;
            _cts = new CancellationTokenSource();
            CancellationToken cancellationToken = _cts.Token;
            var task = new Task(() =>
            {
                Log("开始");
                var dir = $"{this.Model.BasePath}/{folderName}";
                _readingTask = reader.GetFrameAsync(path, startTime, fps, dir, cancellationToken);
            });
            task.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CloseReading();
            do
            {
                Task.Delay(100).GetAwaiter().GetResult();
            } while (!_readingTask?.IsCompleted == true);
        }

        public void Log(string message)
        {
            Dispatcher.Invoke(() => this.TbInfo.Text += message + '\n');
        }

        private void ClearLog()
        {
            this.TbInfo.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "视频文件(*.mp4,*.avi,*.mkv)|*.mp4;*.avi;*.mkv|所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                TbPath.Text = dialog.FileName;
                SetFolderName(dialog.FileName);
            }
        }

        private void SetFolderName(string fileName)
        {
            TbFolderName.Text = System.IO.Path.GetFileNameWithoutExtension(fileName);
        }

        private void CloseReading()
        {
            if (_readingTask?.IsCompleted == false)
                _cts?.Cancel();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            CloseReading();
        }


        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            var basePath = System.IO.Path.GetFullPath(Model.BasePath);

            var text = basePath+"/" + TbFolderName.Text;
            if (!Directory.Exists(text))
            {
                text = basePath;
            }
            Process.Start(new ProcessStartInfo(text));
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            var flyout1 = this.Flyouts.Items[0] as Flyout;
            if (flyout1 == null)
            {
                return;
            }
            flyout1.IsOpen = !flyout1.IsOpen;
        }

        private async void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            var dir = this.TbBaseImagePath.Text;
            try { 
            if (Directory.CreateDirectory(dir).Exists)
            {
                Model.BasePath = dir;
                Model.WriteConfig();
            }
            }
            catch
            {
                await this.ShowMessageAsync("提示","输入路径不合法");
            }
        }

        private void BtnFolderOpen_Click(object sender, RoutedEventArgs e)
        {
            var d1 = new CommonOpenFileDialog();
            d1.DefaultFileName = Model.BasePath;
            d1.IsFolderPicker = true;
            CommonFileDialogResult result = d1.ShowDialog();
            if(result== CommonFileDialogResult.Ok)
            {
                this.TbBaseImagePath.Text = d1.FileName;
            }
        }
    }
}
