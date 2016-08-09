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
using Dialog;
using System.Net.Http;
using Newtonsoft.Json;

namespace WpfSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        static string previousContext = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_send_Click(object sender, RoutedEventArgs e)
        {
            string utterance = textBox_utterance.Text;
            listView_messages.Items.Insert(0, "あなた: " + utterance);

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
            // グローバル変数に今回の通信内容を保存
            previousContext = responseParam.context;

            listView_messages.Items.Insert(0, "システム: " + responseParam.utt);
            textBox_utterance.Text = "";
        }
    }
}
