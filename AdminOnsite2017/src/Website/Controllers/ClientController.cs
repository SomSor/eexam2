using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebSite.ViewModels.AdminOnsiteModelsForClient;
using System.Net;
using ModelBack = WebSite.ViewModels.AdminOnsiteModelsBack;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Options;

namespace WebSite.Controllers
{

    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class ClientController : Controller
    {
        private serverconfig svconfig;
        private IHostingEnvironment _environment;

        public ClientController(IOptions<serverconfig> svconfig, IHostingEnvironment environment)
        {
            this.svconfig = svconfig.Value;
            this._environment = environment;
        }

        [HttpGet]
        [Route("CheckExam/{pid}")]
        public PreExamRespone CheckExam(string pid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/Client/GetExamRegis/{0}", pid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<PreExamRespone>(responseString);
                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("CheckActive")]
        public ActiveRespone CheckActive()
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/Client/GetLastActive");

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ActiveRespone>(responseString);

                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }


        }

        [HttpGet]
        [Route("GetSheet/{pid}/{subjectCode}/{clientid}")]
        public ExamSheetRespone GetSheet(string pid, string subjectCode, string clientid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/Client/GetSheet/{0}/{1}/{2}", pid, subjectCode, clientid);
                    //string URL = string.Format("http://localhost:59684/api/Client/GetSheet/{0}/{1}/{2}", pid, subjectCode, clientid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ExamSheetRespone>(responseString);
                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

        }

        [HttpGet]
        [Route("GetClientId/{uuid}")]
        public WebSite.ViewModels.AdminOnsiteModels.ClientMapResponse GetClientId(string uuid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/Client/GetClientId/{0}", uuid);
                    //string URL = string.Format("http://localhost:59684/api/Client/GetClientId/{0}", pid, subjectCode, clientid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<WebSite.ViewModels.AdminOnsiteModels.ClientMapResponse>(responseString);
                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }

        }

        [HttpPost]
        [Route("Answer")]
        public ViewModels.AdminOnsiteModels.MessageRespone Answer([FromBody] AnswerRequest answerRequest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(answerRequest);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData(this.svconfig.admindbip + "/api/Client/Answer/", "POST", dataByte);
                    var st = System.Text.Encoding.UTF8.GetString(dataByte);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewModels.AdminOnsiteModels.MessageRespone>(st);

                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        [HttpGet]
        [Route("SendExam/{sheetId}/{clientid}")]
        public ResultRespone SendExam(string sheetId, string clientid)
        {
            using (var client = new WebClient())
            {
                try
                {
                    string URL = string.Format(this.svconfig.admindbip + "/api/Client/SendExam/{0}/{1}", sheetId, clientid);

                    var dataByte = client.DownloadData(URL);
                    var responseString = System.Text.Encoding.UTF8.GetString(dataByte);
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultRespone>(responseString);
                    return data;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        [Route("StartExam")]
        public void StartExam([FromBody]ClientSheetRequest clientSheetRequest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(clientSheetRequest);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData(this.svconfig.admindbip + "/api/Client/StartExam/", "POST", dataByte);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpPost]
        [Route("EndExam")]
        public void EndExam([FromBody]ClientSheetRequest clientSheetRequest)
        {
            using (var client = new WebClient())
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(clientSheetRequest);
                    var dataByte = System.Text.Encoding.UTF8.GetBytes(json);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    dataByte = client.UploadData(this.svconfig.admindbip + "/api/Client/EndExam/", "POST", dataByte);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("TestAlert")]
        public void TestAlert()
        {
            //Messagebox.

            //Alert
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="picRequest"></param>
        /// <returns></returns>
        [Route("{sheetid}/{file}/photo")]
        [HttpPost]
        public async System.Threading.Tasks.Task<object> PostProfilePhoto(string sheetid, [FromBody]IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            var url = Path.Combine(uploads, sheetid + ".jpg");
            var fs = new FileStream(url, FileMode.Create);
            if (file.Length > 0)
            {
                await file.CopyToAsync(fs);
            }
            return url;
        }



        [Route("SavePic")]
        [HttpPost]
        public PicResponse SavePic([FromBody]PicRequest picrequest)
        {
            try
            {
                string serverPath = @"C:\exampic\";

                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                var profilePicFileName = string.Format("{0}", picrequest.FileName);
                var path = Path.Combine(serverPath, profilePicFileName);
                System.IO.File.WriteAllBytes(path, picrequest.bytes);
                return new PicResponse { PicUrl = path };
            }
            catch (Exception)
            {
                throw;
            }

        }



        ///// <summary>
        ///// upload photo and Return Photo URL
        ///// </summary>
        ///// <param name="id">รหัสรถ</param>
        ///// <param name="topicid"> รหัส Topic</param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("{id}/{topicid}/photo")]
        //public async System.Threading.Tasks.Task<object> PostPhoto(string id, string topicid)
        //{
        //    // Check if the request contains multipart/form-data.
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    string root = System.Web.HttpContext.Current.Server.MapPath("~/CheckedImg");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        System.Text.StringBuilder sb = new System.Text.StringBuilder(); // Holds the response body

        //        // Read the form data and return an async task.
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        // This illustrates how to get the form data.
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            foreach (var val in provider.FormData.GetValues(key))
        //            {
        //                sb.Append(string.Format("{0}: {1}\n", key, val));
        //            }
        //        }

        //        var localfileURL = string.Empty;
        //        var serverfileURL = string.Empty;
        //        // This illustrates how to get the file names for uploaded files.
        //        foreach (var file in provider.FileData)
        //        {
        //            System.IO.FileInfo fileInfo = new System.IO.FileInfo(file.LocalFileName);
        //            sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));

        //            var fileName = Guid.NewGuid().ToString() + ".jpg";

        //            localfileURL = System.Web.HttpContext.Current.Server.MapPath("~/CheckedImg/Img/" + fileName);


        //            fileInfo.MoveTo(localfileURL);

        //            //Fix URL
        //            serverfileURL = new StringBuilder().Append("http://echecker-vanlek.azurewebsites.net").Append("/CheckedImg/Img/").Append(fileName).ToString();
        //        }


        //        return new { PhotoUrl = serverfileURL };
        //        //return new HttpResponseMessage()
        //        //{
        //        //    Content = new StringContent(sb.ToString())
        //        //};
        //    }
        //    catch (System.Exception)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.InternalServerError);
        //    }
        //}
    }
}