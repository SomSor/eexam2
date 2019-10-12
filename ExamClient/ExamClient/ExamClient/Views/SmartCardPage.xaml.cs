using ExamClient.ViewModel;
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
using TheS.SmartCard;

namespace ExamClient.Views
{
    /// <summary>
    /// Interaction logic for SmartCardPage.xaml
    /// </summary>
    public partial class SmartCardPage : Page
    {
        private System.Threading.CancellationTokenSource cts = null;

        public SmartCardViewModel ViewModel
        {
            get
            {
                return this.DataContext as ViewModel.SmartCardViewModel;
            }
        }
        public Action navigateMethod;
        public SmartCardPage()
        {
            InitializeComponent();

            navigateMethod = NavigateToCamera;

            ViewModel.CheckExam("1234567890123", navigateMethod);

            //IsVisibleChanged += VerifyPopupUI_IsVisibleChanged;
        }



        void VerifyPopupUI_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                this.cts = new System.Threading.CancellationTokenSource();
                try
                {
                    DoReadCard(this.cts.Token);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                this.cts.Cancel();
            }
        }

        private async void DoReadCard(System.Threading.CancellationToken token)
        {
            //acr33u    "ACS ACR33U-A1 3SAM ICC Reader ICC 0"
            //acr38     "ACS CCID USB Reader 0"

            using (var mgr = new SmartCardReaderManager(new TheS.SmartCard.ACOSx86.AcosNoCardDetectionFactory("ACS CCID USB Reader 0")))
            //using (var mgr = new SmartCardReaderManager(new TheS.SmartCard.ACOSx86.AcosCardReaderFactory()))
            {
                mgr.CardRemoved += mgr_CardRemoved;
                while (true != this.cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var cardReader = await mgr.ConnectToReaderWhenNextCardInserted(token);

                        if (cardReader == null)
                        {
                            continue;
                        }

                        var res = TheS.SmartCard.Formatters.ThaiIdCardFormatter.IsThaiIdCard(cardReader);
                        Console.WriteLine(res);
                        if (!res)
                        {

                            if (TheS.SmartCard.Formatters.SimpleMemoryCardFormatter.IsMemoryCard(cardReader))
                            {
                                using (var fmt = new TheS.SmartCard.Formatters.SimpleMemoryCardFormatter(cardReader))
                                {
                                    var pid = fmt.ReadString();
                                    ViewModel.CheckExam(pid , navigateMethod);
                                    //this.LicenList.Focus();
                                    fmt.Dispose();
                                    //this.LicenList.Focus();
                                }
                            }
                            else
                            {
                                using (var fmt = new TheS.SmartCard.Formatters.SimpleCardBinaryFileFormatter(cardReader))
                                {
                                    var pid = fmt.ReadString();

                                    ViewModel.CheckExam(pid , navigateMethod);

                                    this.cts.Cancel();
                                    fmt.Dispose();
                                    //this.LicenList.Focus();
                                }
                            }

                        }
                        else
                        {
                            using (var thaiCard = new TheS.SmartCard.Formatters.ThaiIdCardFormatter(cardReader))
                            {
                                var info = await thaiCard.GetPersonalInfo();

                                var imgData = await thaiCard.GetPictureData();
                                using (var ms = new System.IO.MemoryStream(imgData))
                                {
                                    var decoder = new JpegBitmapDecoder(ms,
                                                                    BitmapCreateOptions.PreservePixelFormat,
                                                                    BitmapCacheOption.OnLoad);

                                    WriteableBitmap writable = new WriteableBitmap(decoder.Frames.Single());
                                    writable.Freeze();

                                    profileImg.Source = writable;
                                }
                                //Console.WriteLine(info.PID);

                                ViewModel.CheckExam(info.PID , navigateMethod);
                                //this.LicenList.Focus();

                                this.cts.Cancel();
                                thaiCard.Dispose();


                            }


                        }
                    }

                    catch (Exception ex)
                    {

                    }
                }
                mgr.CardRemoved -= mgr_CardRemoved;
            }

        }

        private void mgr_CardRemoved(object sender, CardRemovedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                this.cts.Cancel();
                //var vm = MyGrid.DataContext as TheS.eXam.Examination.Manager.ViewModels.VertifyViewModel;
                //vm.Clear();


            });
        }

        private void backtoLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", UriKind.RelativeOrAbsolute));

        }

        void NavigateToCamera()
        {
            NavigationService.Navigate(new Uri("/Views/CameraPage.xaml", UriKind.RelativeOrAbsolute));
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new Uri("/Views/CameraPage.xaml", UriKind.RelativeOrAbsolute));

        //}
    }
}
