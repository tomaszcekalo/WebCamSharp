using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WebCamSharp.WpfExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage _image;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //WebCam.BitmapCapture del = DelegateCallback;
            //var cam = new WebCam(bitmapDelegate: del);

            WebCam.StreamCapture del = StreamDelegateCallback;
            var cam = new WebCam(streamDelegate: del);
        }

        public void StreamDelegateCallback(MemoryStream ms)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() =>
                {
                    _image = new BitmapImage();
                    _image.BeginInit();
                    ms.Seek(0, SeekOrigin.Begin);
                    _image.StreamSource = ms;
                    _image.EndInit();
                    this.WebCam.Source = _image;
                }

            ));
        }
    }
}