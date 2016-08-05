using AspDotNetSample.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AspDotNetSample.Controllers
{
    public class HomeController : Controller
    {
        static string previousContext = "";
        
        // /Home/Index にアクセスが来たとき
        public ActionResult Index()
        {
            // /Views/Home/Index.cshtmlを返す
            return View();
        }

        // /Home/Api にGetアクセスが来たとき
        [HttpGet]
        public ActionResult Api(string utterance)
        {
            string apiKey = "6863673132504d57574b32583564796867516a4b624630386d61486e4163646c5442446438787731746c37";
            string url = "https://api.apigw.smt.docomo.ne.jp/dialogue/v1/dialogue?APIKEY="+apiKey;
            DialogRequest requestParam = new DialogRequest();
            requestParam.context = previousContext;
            requestParam.utt = utterance;

            string requestJson = JsonConvert.SerializeObject(requestParam);
            StringContent requestContent = new StringContent(requestJson, Encoding.UTF8,"application/json");

            HttpClient client = new HttpClient();
            var response = client.PostAsync(url, requestContent).Result;

            var responseJson = response.Content.ReadAsStringAsync().Result;
            DialogResponse responseParam = JsonConvert.DeserializeObject<DialogResponse>(responseJson);
            JsonResult result = new JsonResult();
            result.Data = new { utterance = responseParam.utt };
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            previousContext = responseParam.context;
            return result;
        }
    }
}