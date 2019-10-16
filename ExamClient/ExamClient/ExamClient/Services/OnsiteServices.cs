using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamClient.Models;
using RestSharp;
using Newtonsoft.Json;

namespace ExamClient.Services
{
    public class OnsiteServices : IOnsiteServices
    {
        //private readonly string serviceUrl = "http://10.93.77.199:8080/api";
        //private readonly string serviceUrl = "http://192.168.5.88:8080/api";
        //private readonly string serviceUrl = "http://localhost:58589/api";
        private readonly string serviceUrl = "http://150.95.27.173:8082/api";
        //private readonly string serviceUrl = "http://58.97.18.64:8082/api";

        public void Answer(AnswerRequest answer)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Client/Answer", Method.POST);
            request.AddParameter("application/json", JsonConvert.SerializeObject(answer), ParameterType.RequestBody);
            var response = client.Execute(request);
        }

        public ActiveResponse CheckActive()
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Client/CheckActive", Method.GET);
            request.AddParameter("application/json", null, ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ActiveResponse>(response.Content);

        }

        public PreExamResponse CheckExam(string pid)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Client/CheckExam/" + pid, Method.GET);
            request.AddParameter("application/json", null, ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<PreExamResponse>(response.Content);
        }

        public void EndExam(ClientSheetRequest clientsheet)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Client/EndExam", Method.POST);
            request.AddParameter("application/json", JsonConvert.SerializeObject(clientsheet), ParameterType.RequestBody);
            var response = client.Execute(request);
        }

        public ExamSheetResponse GetSheet(string pid, string subjectCode, string clientid)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Client/GetSheet/" + pid + "/" + subjectCode + "/" + clientid, Method.GET);
            request.AddParameter("application/json", null, ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ExamSheetResponse>(response.Content);
        }

        public PicResponse SavePic(PicRequest picrequest)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Client/SavePic", Method.POST);
            request.AddParameter("application/json", JsonConvert.SerializeObject(picrequest), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<PicResponse>(response.Content);
        }

        public ResultResponse SendExam(string sheetid, string clientid)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Client/SendExam/" + sheetid + "/" + clientid, Method.GET);
            request.AddParameter("application/json", null, ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ResultResponse>(response.Content);
        }

        public void StartExam(ClientSheetRequest clientsheet)
        {
            var client = new RestClient(serviceUrl);
            var request = new RestRequest("Client/StartExam", Method.POST);
            request.AddParameter("application/json", JsonConvert.SerializeObject(clientsheet), ParameterType.RequestBody);
            var response = client.Execute(request);
        }
    }
}
