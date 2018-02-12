using ColorsImage.Data;
using ColorsImage.Util;
using ColorsImage.Util.Logger;
using Microsoft.Win32;
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

namespace ColorsImage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var reader = new ImageReader();
            reader.Logger = new Logger1(Log);
            var path = TbPath.Text;
            var mode =int.Parse( this.TbFrame.Text);
            Task task = new Task(() => {
                Log("开始");
                reader.ReadAndWrite(path, mode, System.IO.Path.GetFileNameWithoutExtension(path));
                Log("结束");
            });
            task.Start();
        }

        public void Log(string message)
        {
            Dispatcher.Invoke(()=>this.TbInfo.Text += message + '\n');
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "视频文件(*.mp4,*.avi)|*.mp4;*.avi|所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                TbPath.Text= dialog.FileName;
            }
        }
    }
}
