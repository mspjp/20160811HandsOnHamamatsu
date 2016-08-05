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

            string apiKey = "6863673132504d57574b32583564796867516a4b624630386d61486e4163646c5442446438787731746c37";
            string url = "https://api.apigw.smt.docomo.ne.jp/dialogue/v1/dialogue?APIKEY=" + apiKey;
            DialogRequest requestParam = new DialogRequest();
            requestParam.context = previousContext;
            requestParam.utt = utterance;

            string requestJson = JsonConvert.SerializeObject(requestParam);
            StringContent requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(url, requestContent);

            var responseJson = await response.Content.ReadAsStringAsync();
            DialogResponse responseParam = JsonConvert.DeserializeObject<DialogResponse>(responseJson);

            previousContext = responseParam.context;

            listView_messages.Items.Insert(0, "システム: " + responseParam.utt);
        }
    }
}
