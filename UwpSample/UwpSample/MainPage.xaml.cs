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

        private async void button_send_Click(object sender, RoutedEventArgs e)
        {
            string utterance = textBox_utterance.Text;
            listView_messages.Items.Insert(0,"あなた: "+utterance);

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

            listView_messages.Items.Insert(0,"システム: "+responseParam.utt);
        }
    }
}
