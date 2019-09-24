using Newtonsoft.Json;
using SmartCardReader.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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

namespace SmartCardReader
{
    public partial class MainWindow : Window
    {
        public PersonalCardInfo MyInfo = new PersonalCardInfo();
        System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();

        string centerid = "1";
        string pid = "1000000000001";

        string baseUrl = "http://localhost:7752/";
        string ListTestRegistrationApiUrl = "api/OnSite/ListTestRegistration/{centerid}";
        string GetInfoForPrintQRApiUrl = "api/OnSite/GetInfoForPrintQR/{pid}";
        string GetResultApiUrl = "api/OnSite/GetResult/{pid}";

        Models.DisplayAllVM MapWildcardVM = new Models.DisplayAllVM();
        Models.PrintQRVM QRVM = new Models.PrintQRVM();
        Models.CheckResultVM ResultVM = new Models.CheckResultVM();
        
        private string localdbip;
        private string OnSiteIP;

        public MainWindow()
        {
            InitializeComponent();
            localdbip = System.Configuration.ConfigurationSettings.AppSettings.Get("LocalIP");
            OnSiteIP = System.Configuration.ConfigurationSettings.AppSettings.Get("OnSiteIP");

            this.baseUrl = localdbip;

            var cts = new System.Threading.CancellationTokenSource();

            //get center data
            using (var client = new WebClient())
            {
                try
                {
                    //var responseString = client.DownloadString(URL);
                    var dataByte = client.DownloadData(this.localdbip + "api/onsite/getcenterdata");
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<CenterDataResponse>(responseString);

                    if (data != null)
                    {
                        this.centerid = data._id;                        
                    }
                    else
                    {
                        throw new Exception("ไม่พบข้อมูลโรงเรียน");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(" การเชื่อมการ กรม ขัดข้อง " + e.ToString());
                }
            }


            try
            {
                DoReadCard(cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridPerson.SelectedItem != null)
            {
                var data = (Models.TestRegistrationRespone)datagridPerson.SelectedItem;
                NewCode.Text = data.PID;
            }
            e.Handled = true;
        }

        private void txtMapSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = new Models.DisplayAllVM()
            {
                TestRegistrations = this.MapWildcardVM.TestRegistrations.Where(x => x.PID.Contains(txtMapSearch.Text) || (x.FullName).Contains(txtMapSearch.Text)).ToList(),
            };
            (this.TabControl1.Items[0] as TabItem).DataContext = filtered;
            e.Handled = true;
        }

        private void btnRefresh_click(object sender, RoutedEventArgs e)
        {
            (this.TabControl1.Items[0] as TabItem).DataContext = null;
            txtMapSearch.Text = string.Empty;

            var apiurl = new Uri(baseUrl + ListTestRegistrationApiUrl.Replace("{centerid}", centerid));
            using (var wc = new WebClient())
            {
                wc.DownloadDataAsync(apiurl);
                wc.DownloadDataCompleted += (s, ee) =>
                {
                    var dataString = Encoding.UTF8.GetString(ee.Result);
                    this.MapWildcardVM = JsonConvert.DeserializeObject<Models.DisplayAllVM>(dataString);
                    (this.TabControl1.Items[0] as TabItem).DataContext = this.MapWildcardVM;
                };
            }
            e.Handled = true;
        }

        private void TabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.TabControl1.SelectedIndex == 0))
            {
                var apiurl = new Uri(baseUrl + ListTestRegistrationApiUrl.Replace("{centerid}", centerid));
                using (var wc = new WebClient())
                {
                    wc.DownloadDataAsync(apiurl);
                    wc.DownloadDataCompleted += (s, ee) =>
                    {
                        try
                        {
                            var dataString = Encoding.UTF8.GetString(ee.Result);
                            this.MapWildcardVM = JsonConvert.DeserializeObject<Models.DisplayAllVM>(dataString);
                            (this.TabControl1.Items[0] as TabItem).DataContext = this.MapWildcardVM;
                        }
                        catch { }
                    };
                }
            }
            else if ((this.TabControl1.SelectedIndex == 1))
            {
                this.LoadTab2(MyInfo.PID);
            }
            else if ((this.TabControl1.SelectedIndex == 2))
            {
                this.LoadTab3(MyInfo.PID);
            }
            e.Handled = true;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void btn_map_click(object sender, RoutedEventArgs e)
        {
            var cts = new System.Threading.CancellationTokenSource();

            try
            {
                DoMapCard(cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void btnSearchQr_click(object sender, RoutedEventArgs e)
        {
            this.LoadTab2(txtSearchQr.Text);
        }

        private void btnSearchResult_click(object sender, RoutedEventArgs e)
        {
            this.LoadTab3(txtSearchResult.Text);
        }

        private async void DoReadCard(System.Threading.CancellationToken token)
        {
            //acr33u    "ACS ACR33U-A1 3SAM ICC Reader ICC 0"
            //acr38     "ACS CCID USB Reader 0"

            //using (var mgr = new SmartCardReaderManager(new TheS.SmartCard.ACOSx86.AcosNoCardDetectionFactory("ACS ACR33U-A1 3SAM ICC Reader ICC 0")))
            //using (var mgr = new SmartCardReaderManager(new TheS.SmartCard.ACOSx86.AcosNoCardDetectionFactory("ACS CCID USB Reader 0")))
            using (var mgr = new SmartCardReaderManager(new TheS.SmartCard.ACOSx86.AcosCardReaderFactory()))
            {
                mgr.CardRemoved += mgr_CardRemoved;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var cardReader = await mgr.ConnectToReaderWhenNextCardInserted(token);
                        if (cardReader == null)
                        {
                            continue;
                        }

                        var res = TheS.SmartCard.Formatters.ThaiIdCardFormatter.IsThaiIdCard(cardReader);
                        if (!res)
                        {
                            if (TheS.SmartCard.Formatters.SimpleMemoryCardFormatter.IsMemoryCard(cardReader))
                            {
                                using (var fmt = new TheS.SmartCard.Formatters.SimpleMemoryCardFormatter(cardReader))
                                {
                                    if (MyInfo == null) MyInfo = new PersonalCardInfo();
                                    MyInfo.PID = fmt.ReadString();
                                }
                            }
                            else
                            {
                                using (var fmt = new TheS.SmartCard.Formatters.SimpleCardBinaryFileFormatter(cardReader))
                                {
                                    if (MyInfo == null) MyInfo = new PersonalCardInfo();
                                    MyInfo.PID = fmt.ReadString();
                                }
                            }
                        }
                        else
                        {
                            using (var thaiCard = new TheS.SmartCard.Formatters.ThaiIdCardFormatter(cardReader))
                            {
                                MyInfo = await thaiCard.GetPersonalInfo();
                                if (MyInfo == null) MyInfo = new PersonalCardInfo();

                                //var imgData = await thaiCard.GetPictureData();
                                //using (var ms = new System.IO.MemoryStream(imgData))
                                //{
                                //    //var decoder = new JpegBitmapDecoder(ms,
                                //    //                                BitmapCreateOptions.PreservePixelFormat,
                                //    //                                BitmapCacheOption.OnLoad);

                                //    //WriteableBitmap writable = new WriteableBitmap(decoder.Frames.Single());
                                //    //writable.Freeze();
                                //}
                            }
                        }
                        OldCode.Text = MyInfo.PID;
                        this.LoadTab2(MyInfo.PID);
                        this.LoadTab3(MyInfo.PID);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                //mgr.CardRemoved -= mgr_CardRemoved;
            }
        }

        private async void DoMapCard(System.Threading.CancellationToken token)
        {
            //acr33u    "ACS ACR33U-A1 3SAM ICC Reader ICC 0"
            //acr38     "ACS CCID USB Reader 0"

            //using (var mgr = new SmartCardReaderManager(new TheS.SmartCard.ACOSx86.AcosNoCardDetectionFactory("ACS ACR33U-A1 3SAM ICC Reader ICC 0")))
            //using (var mgr = new SmartCardReaderManager(new TheS.SmartCard.ACOSx86.AcosNoCardDetectionFactory("ACS CCID USB Reader 0")))
            using (var mgr = new SmartCardReaderManager(new TheS.SmartCard.ACOSx86.AcosCardReaderFactory()))
            {
                mgr.CardRemoved += mgr_CardRemoved;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var cardReader = await mgr.ConnectToReaderWhenNextCardInserted(token);
                        if (cardReader == null)
                        {
                            continue;
                        }

                        var res = TheS.SmartCard.Formatters.ThaiIdCardFormatter.IsThaiIdCard(cardReader);
                        if (!res)
                        {
                            string pid = string.Empty;
                            if (TheS.SmartCard.Formatters.SimpleMemoryCardFormatter.IsMemoryCard(cardReader))
                            {
                                using (var fmt = new TheS.SmartCard.Formatters.SimpleMemoryCardFormatter(cardReader))
                                {
                                    pid = fmt.ReadString();
                                    fmt.WriteString(NewCode.Text);
                                }
                            }
                            else
                            {
                                using (var fmt = new TheS.SmartCard.Formatters.SimpleCardBinaryFileFormatter(cardReader))
                                {
                                    pid = fmt.ReadString();
                                    fmt.WriteString(NewCode.Text);
                                }
                            }

                            OldCode.Text = NewCode.Text;
                            this.LoadTab2(NewCode.Text);
                            this.LoadTab3(NewCode.Text);
                            MessageBox.Show("ระบบอัพเดท Wild Card    " + pid + "   To   " + NewCode.Text);

                            cts.Cancel();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        private void mgr_CardRemoved(object sender, CardRemovedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    MyInfo = new PersonalCardInfo();
                    OldCode.Text = "";
                    //this.QRVM = null;
                    //this.ResultVM = null;
                    PanelOne.Children.Clear();
                    (this.TabControl1.Items[2] as TabItem).DataContext = null;
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadTab2(string pid)
        {
            var apiurl = new Uri(baseUrl + GetInfoForPrintQRApiUrl.Replace("{pid}", pid));
            using (var wc = new WebClient())
            {
                wc.DownloadDataAsync(apiurl);
                wc.DownloadDataCompleted += (s, ee) =>
                {
                    try
                    {
                        var dataString = Encoding.UTF8.GetString(ee.Result);
                        this.QRVM = JsonConvert.DeserializeObject<Models.PrintQRVM>(dataString);
                        (this.TabControl1.Items[1] as TabItem).DataContext = this.QRVM;

                        var obj = new QRPrintUI { DataContext = this.QRVM };
                        PanelOne.Children.Clear();
                        PanelOne.Children.Add(obj);
                    }
                    catch { }
                };
            }
        }

        private void LoadTab3(string pid)
        {
            var apiurl = new Uri(baseUrl + GetResultApiUrl.Replace("{pid}", pid));
            using (var wc = new WebClient())
            {
                wc.DownloadDataAsync(apiurl);
                wc.DownloadDataCompleted += (s, ee) =>
                {
                    try
                    {
                        var dataString = Encoding.UTF8.GetString(ee.Result);
                        this.ResultVM = JsonConvert.DeserializeObject<Models.CheckResultVM>(dataString);
                        (this.TabControl1.Items[2] as TabItem).DataContext = this.ResultVM;
                    }
                    catch { }
                };
            }
        }

        private void btnPrintQr_click(object sender, RoutedEventArgs e)
        {
            //TODO: print qr
            if (PanelOne.Children.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกรายการที่จะพิมพ์");
                return;
            }
            System.Diagnostics.Process.Start(OnSiteIP + "print.html#!/qrcode/" + ((PanelOne.Children[0] as QRPrintUI).DataContext as Models.PrintQRVM).TestRegistrations.PID);
            //MessageBox.Show("พิมพ์รายการ : " + (listBoxResult.SelectedItem as Models.Result)._id);
        }

        private void btnPrintResult_click(object sender, RoutedEventArgs e)
        {
            //TODO: print result
            if (listBoxResult.SelectedItem == null)
            {
                MessageBox.Show("กรุณาเลือกรายการที่จะพิมพ์");
                return;
            }
            System.Diagnostics.Process.Start(OnSiteIP + "print.html#!/testresult/" + (listBoxResult.SelectedItem as Models.Result).ExamNumber + "/" + (listBoxResult.SelectedItem as Models.Result)._id);
            //MessageBox.Show("พิมพ์รายการ : " + (listBoxResult.SelectedItem as Models.Result)._id);
        }
    }
}
