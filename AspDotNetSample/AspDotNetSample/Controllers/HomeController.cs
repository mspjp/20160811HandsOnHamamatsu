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
            // Docomo雑談API用の鍵
            string apiKey = "6863673132504d57574b32583564796867516a4b624630386d61486e4163646c5442446438787731746c37";
            // apiKeyを含んだREST API用URL
            string url = "https://api.apigw.smt.docomo.ne.jp/dialogue/v1/dialogue?APIKEY=" + apiKey;
            // URLに送るPOSTパラメータクラスのインスタンス化
            DialogRequest requestParam = new DialogRequest();
            // 前回のデータをcontext変数に代入
            requestParam.context = previousContext;
            // 送る会話の内容をutt変数に代入
            requestParam.utt = utterance;

            // 作成したデータをstring型にシリアライズ(変換)
            string requestJson = JsonConvert.SerializeObject(requestParam);
            // 作成したデータをエンコード
            StringContent requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // HttpClient(ネットコネクション)をインスタンス化
            HttpClient client = new HttpClient();
            // 非同期通信を行いデータを送信し,DocomoAPIの返り値を取得
            var response = client.PostAsync(url, requestContent).Result;

            // 返り値の内容を読み込む
            var responseJson = response.Content.ReadAsStringAsync().Result;
            // 読み込んだデータをクラス型に変換
            DialogResponse responseParam = JsonConvert.DeserializeObject<DialogResponse>(responseJson);
            // Viewで使えるようJsonを用意
            JsonResult result = new JsonResult();
            // JsonにDocomoから帰ってきたデータのうち uttを格納
            result.Data = new { utterance = responseParam.utt };
            // Viewでgetで取れるように(セキュリティ)変更
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            // グローバル変数に今回の通信内容を保存
            previousContext = responseParam.context;
            // Jsonを返す
            return result;
        }
    }
}