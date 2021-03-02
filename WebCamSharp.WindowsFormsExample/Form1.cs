using System;
using System.Drawing;
using System.Windows.Forms;

namespace WebCamSharp.WindowsFormsExample
{
    public partial class Form1 : Form
    {
        private WebCam _cam;

        public Form1()
        {
            InitializeComponent();
            WebCam.BitmapCapture del = DelegateCallback;
            _cam = new WebCam(bitmapDelegate: del);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        public void DelegateCallback(Bitmap bitmap)
        {
            pictureBox1.Image = bitmap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}