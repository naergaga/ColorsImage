using ColorsImage.Data;
using ColorsImage.Util;
using ColorsImage.Util.Logger;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
                _readingTask = reader.GetFrameAsync(path, startTime, fps, folderName, cancellationToken);
            });
            task.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CloseReading();
            do
            {
                Task.Delay(100).GetAwaiter().GetResult();
            } while (!_readingTask?.IsCanceled == true);
        }

        public void Log(string message)
        {
            Dispatcher.Invoke(() => this.TbInfo.Text += message + '\n');
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
            _cts?.Cancel();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CloseReading();
        }
    }
}
