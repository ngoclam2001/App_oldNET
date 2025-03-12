using ClassDataLand;
using Microsoft.Web.WebView2.Core;
using RazorLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly RazorLightEngine engine;
        private readonly string basePath;

        public Form1()
        {
            InitializeComponent();

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Views");
            basePath = Path.GetFullPath(basePath);

            engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(basePath)
                .UseMemoryCachingProvider()
                .Build();
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await web_1.EnsureCoreWebView2Async(null);

            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Views", "index.html");

            if (File.Exists(htmlPath))
            {
                web_1.CoreWebView2.NavigateToString(File.ReadAllText(htmlPath));

                web_1.NavigationCompleted += WebView2_NavigationCompleted;
            }
            else
            {
                MessageBox.Show(htmlPath);
            }
        }
        private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                SendDataToWebView();
            }
        }

        //Gui data qua web view
        private void SendDataToWebView()
        {
            var dataLand = new DataLand
            {
                Id = "AA 0987654",
                Muc1 = new Dictionary<int, Person>
                        {
                            { 1, new Person { PersonName = "Nguyễn Văn A", PersonCCCD = "231414141" } },
                            { 2, new Person { PersonName = "Nguyễn Văn B", PersonCCCD = "231414142" } },
                            { 3, new Person { PersonName = "Nguyễn Văn C", PersonCCCD = "231414143" } }
                        },
                Muc2 = new Dictionary<int, Land>
                        {
                            { 1, new Land { LandNumber = "01", LandMapNumber = "100m2", LandArea = 100, LandClass = "abc", LandUseDate = "abc", LandUse = "abc", LandAddress = "abc", LandPurpose = "abc" } },
                            { 2, new Land { LandNumber = "02", LandMapNumber = "200m2", LandArea = 100, LandClass = "abc", LandUseDate = "abc", LandUse = "abc", LandAddress = "abc", LandPurpose = "abc" } }
                        },
                Muc3 = new Dictionary<int, Asset>
                        {
                            { 1, new Asset { AssetName = "01", AssetArea = "100m2", AssetAreaUse = "01", AssetNumberFloor = "100m2", AssetStructure = "01", AssetLevel = "100m2", AssetUse = "01", AssetUseTime = "100m2", AssetAddress = "abc" } },
                            { 2, new Asset { AssetName = "02", AssetArea = "200m2", AssetAreaUse = "02", AssetNumberFloor = "200m2", AssetStructure = "02", AssetLevel = "200m2", AssetUse = "02", AssetUseTime = "200m2", AssetAddress = "abc" } }
                        },
                Muc4 = new Dictionary<int, string>
                        {
                            { 0, "https://d1hjkbq40fs2x4.cloudfront.net/2017-08-21/files/landscape-photography_1645-t.jpg" }
                        },
                IsCheckMuc2 = true,
                IsCheckMuc3 = true
            };

            //convert data to json
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dataLand);

            web_1.CoreWebView2.PostWebMessageAsJson(json);
        }
    }
}
