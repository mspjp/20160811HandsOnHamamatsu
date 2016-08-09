using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Dialog;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static string previousContext = "";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void button_send_Click(object sender, RoutedEventArgs e)
        {
            string utterance = textBox_utterance.Text;
            listView_messages.Items.Insert(0,"あなた: "+utterance);


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

            previousContext = responseParam.context;

            listView_messages.Items.Insert(0,"システム: "+responseParam.utt);
            textBox_utterance.Text = "";
        }
    }
}
