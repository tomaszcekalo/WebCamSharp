using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.IO;
using System.Threading;

namespace WebCamSharp
{
    public class WebCam : IWebCam
    {
        /// <summary>
        /// Initializes camera feed
        /// </summary>
        /// <param name="autoActivate">If set to false this disables the camera from starting, you must call the Initialize method.</param>
        /// <param name="bitmapDelegate">delegate to be called when the image is captured</param>
        public WebCam(bool autoActivate = true,
            BitmapCapture bitmapDelegate = null,
            StreamCapture streamDelegate = null
            )
        {
            _bitmapDelegate = bitmapDelegate;
            _streamDelegate = streamDelegate;
            if (autoActivate)
            {
                Initialize();
            }
        }

        // Create class-level accessible variables
        private static VideoCapture _capture;

        private static Mat _frame;
        private static Thread _camera;
        private static bool _isCameraRunning = false;
        private readonly BitmapCapture _bitmapDelegate;
        private readonly StreamCapture _streamDelegate;

        /// <summary>
        /// Starts the camera feed
        /// </summary>
        public void Initialize()
        {
            _camera = new Thread(CaptureCameraCallback)
            {
                IsBackground = true
            };
            _camera.Start();
            _isCameraRunning = true;
        }

        public delegate void BitmapCapture(Bitmap bitmap);

        public delegate void StreamCapture(MemoryStream stream);

        private void CaptureCameraCallback()
        {
            if (!_isCameraRunning)
            {
                return;
            }
            _frame = new Mat();
            _capture = new VideoCapture(0);
            _capture.Open(0);

            if (_capture.IsOpened())
            {
                while (_isCameraRunning)
                {
                    _capture.Read(_frame);

                    _bitmapDelegate?.Invoke(_frame.ToBitmap());
                    _streamDelegate?.Invoke(_frame.ToMemoryStream());
                }
            }
        }

        /// <summary>
        /// Deinitializes the camera. Can be reinitialized.
        /// </summary>
        public void Deinitialize()
        {
            _camera.Abort();
            _capture.Release();
            _isCameraRunning = false;
        }

        /// <summary>
        /// Destroys the camera
        /// </summary>
        ~WebCam()
        {
            Deinitialize();
            _capture.Dispose();
            _frame.Dispose();
        }
    }
}