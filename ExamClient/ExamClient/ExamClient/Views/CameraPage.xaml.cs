using ExamClient.Models;
using ExamClient.Services;
using ExamClient.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamClient.Views
{
    /// <summary>
    /// Interaction logic for CameraPage.xaml
    /// </summary>
    public partial class CameraPage : Page
    {
        WebCam webcam;
        //String PICTURE_FILEFORMAT = "picture{0}.jpg";
        bool _isCameraOn = true;
        Bitmap ProfilePhoto;

        public CameraPage()
        {
            InitializeComponent();

            alertDlg.Visibility = Visibility.Collapsed;
            ProfilePhoto = null;
            webcam = new WebCam();
            webcam.InitializeWebCam(ref profileImg);
            webcam.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bs = (BitmapSource)profileImg.Source;

            const string cameraSB = "CameraStoryboard";
            const string nextSB = "NextStoryboard";

            if (bs != null && _isCameraOn)
            {
                try
                {
                    SavePic(bs);
                    bs = null;

                    var storyboard = this.FindResource(nextSB) as Storyboard;
                    storyboard.Begin();

                    storyboard = this.FindResource(cameraSB) as Storyboard;
                    storyboard.Stop();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                bs = null;
                ClearPic();

                var storyboard = this.FindResource(cameraSB) as Storyboard;
                storyboard.Begin();

                storyboard = this.FindResource(nextSB) as Storyboard;
                storyboard.Stop();
            }
        }

        private void SavePic(BitmapSource bitmap)
        {
            TestingData.ProfilePhoto = new BitmapImage();

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.QualityLevel = 100;

            //HACK
            TestingData.SubjectCode = "11";

            var PICTURE_FILENAME = string.Format("{0}.jpg", TestingData.sheet._id);
            //var PICTURE_FILENAME = "";
            string serverPath = @"C:\PCTest\pic\";
            var picPath = System.IO.Path.Combine(serverPath, PICTURE_FILENAME);

            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            using (var fstream = new System.IO.FileStream(picPath, FileMode.Create))
            {
                encoder.Save(fstream);
                fstream.Flush();
                fstream.Close();
                fstream.Dispose();

                webcam.Stop();
                _isCameraOn = false;

            }

            var profileBitmap = BitmapFromSource(bitmap);
            ProfilePhoto = profileBitmap;


            TestingData.ProfilePhoto = new BitmapImage();
            Uri uri = new Uri(picPath, UriKind.RelativeOrAbsolute);
            TestingData.ProfilePhoto.BeginInit();
            TestingData.ProfilePhoto.UriSource = uri;
            TestingData.ProfilePhoto.CacheOption = BitmapCacheOption.OnLoad;
            TestingData.ProfilePhoto.EndInit();

            //save image to server     
            byte[] bit = new byte[0];

            using (var stream = new System.IO.MemoryStream())
            {
                JpegBitmapEncoder encoder2 = new JpegBitmapEncoder();
                encoder2.Frames.Add(BitmapFrame.Create(bitmap));
                encoder2.QualityLevel = 100;
                encoder2.Save(stream);
                bit = stream.ToArray();

                OnsiteServices svc = new OnsiteServices();

                PicRequest picre = new PicRequest
                {
                    FileName = PICTURE_FILENAME,
                    bytes = bit
                };
                svc.SavePic(picre);

                stream.Flush();
                stream.Close();
                stream.Dispose();
            }
        }

        private void ClearPic()
        {
            TestingData.ProfilePhoto = new BitmapImage();
            webcam = new WebCam();
            webcam.InitializeWebCam(ref profileImg);
            webcam.Start();
            _isCameraOn = true;
        }

        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);

                outStream.Flush();
                outStream.Close();
                outStream.Dispose();

            }
            return bitmap;
        }

        protected virtual bool IsFileinUse(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // check ว่า ถ่ายรูปรึยัง
            if (_isCameraOn == false)
            {
                //ClearPic();
                NavigationService.Navigate(new Uri("/Views/TutorialPage.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                alertDlg.Visibility = Visibility.Visible;
            }
        }

        private void closeDlg(object sender, RoutedEventArgs e)
        {
            alertDlg.Visibility = Visibility.Collapsed;
        }
    }
}
