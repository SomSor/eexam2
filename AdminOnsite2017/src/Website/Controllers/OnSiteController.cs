using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebSite.ViewModels.AdminOnsiteModels;
using System.Net;
using ModelBack = WebSite.ViewModels.AdminOnsiteModelsBack;
using Microsoft.Extensions.Options;

namespace WebSite.Controllers
{
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class OnSiteController : Controller
    {
        private serverconfig svconfig;

        public OnSiteController(IOptions<serverconfig> svconfig)
        {
            this.svconfig = svconfig.Value;
            //adminwebip = new Configuration().Get<string>("adminip:webip");
            //admindbip = new Configuration().Get<string>("adminip:dbip");
        }

        [HttpGet]
        [Route("GetCenterData")]
        public Center GetCenterData()
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/GetCenterData");
                    //string URL = string.Format("http:/10.93.77.199/localdb/api/OnSite/GetCenterData");

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Center>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

        }

        [HttpPost]
        [Route("Login")]
        public MessageRespone Login([FromBody] UserRequest userRequest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/Login/{0}/{1}/{2}", userRequest.User, userRequest.Pass, userRequest.CenterId);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageRespone>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("GetMainInfo/{centerid}")]
        public MainVM GetMainInfo(string centerid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/GetMainInfo/{0}", centerid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<MainVM>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("ListTesrRegistration/{centerid}")]
        public DisplayAllVM ListTesrRegistration(string centerid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/ListTestRegistration/{0}", centerid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DisplayAllVM>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        [Route("Active")]
        public void Active([FromBody] ActiveRequest activeRequest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(activeRequest);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData(this.svconfig.admindbip + "/api/OnSite/Active/", "POST", dataByte);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        [Route("EndTest")]
        public void EndTest([FromBody] ActionSheetRequest actionSheetRequest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(actionSheetRequest);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData(this.svconfig.admindbip + "/api/OnSite/EndTest/", "POST", dataByte);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        [Route("Resume")]
        public void Resume([FromBody] ActionSheetRequest actionSheetRequest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(actionSheetRequest);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData(this.svconfig.admindbip + "/api/OnSite/Resume/", "POST", dataByte);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        [Route("Cancel")]
        public void Cancel([FromBody] ActionSheetRequest actionSheetRequest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(actionSheetRequest);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData(this.svconfig.admindbip + "/api/OnSite/Cancel/", "POST", dataByte);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("SearchTestRegis/{txt}/{centerid}")]
        public DisplayAllVM SearchTestRegis(string txt, string centerid)
        {
            //using (var client = new WebClient())
            //{
            //    try
            //    {
            //        string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/SearchTestRegis/{0}/{1}", txt, centerid);

            //        var dataByte = client.DownloadData(URL);
            //        var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
            //        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DisplayAllVM>(responseString);

            //        return data;
            //    }
            //    catch (Exception e)
            //    {
            //        throw new Exception(e.Message);
            //    }
            //}

            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/ListTestRegistration/{0}", centerid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DisplayAllVM>(responseString);


                    var result = new DisplayAllVM
                    {
                        TestRegistrations = data.TestRegistrations.Where(x => x.CenterId == centerid && (x.PID == txt || x.FirstName.Contains(txt) || x.LastName.Contains(txt))).ToList(),

                    };
                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

        }

        [HttpGet]
        [Route("ListTesting/{centerid}")]
        public TestingVM ListTesting(string centerid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/ListTesting/{0}", centerid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<TestingVM>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("GetInfoForPrintQR/{pid}")]
        public PrintQRVM GetInfoForPrintQR(string pid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/GetInfoForPrintQR/{0}", pid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<PrintQRVM>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("GetResult/{pid}")]
        public CheckResultVM GetResult(string pid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/GetResult/{0}", pid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckResultVM>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("GetTestResult/{pid}/{sheetid}")]
        public Result GetTestResult(string pid, string sheetid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/GetResult/{0}/{1}", pid, sheetid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("ListExamData/{centerid}")]
        public ExamDataVM ListExamData(string centerid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/ListExamData/{0}", centerid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ExamDataVM>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("CloseExamData")]
        public void CloseExamData()
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/OnSite/CloseExamData");
                    //string URL = string.Format("http://localhost:9143/api/OnSite/CloseExamData");

                    var responseString = client.DownloadString(URL);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        [Route("ChangeLanguage")]
        public void ChangeLanguage([FromBody] TestRegistration testreg)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(testreg);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData(this.svconfig.adminwebip + "/api/OnSite/ChangeLanguage/", "POST", dataByte);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

    }
}