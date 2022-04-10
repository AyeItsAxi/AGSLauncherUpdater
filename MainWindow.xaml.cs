using System;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;
using System.IO.Compression;
using Microsoft.Toolkit.Uwp.Notifications;

namespace AGSLauncherUpdater
{
    public partial class MainWindow : Window
    {
        public string LCAppDat = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AveryGame Launcher\\EnvPath.txt";
        public string LDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AveryGame Launcher\\Dir.txt";
        public string FP = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AveryGame Launcher\\EnvPath.txt");
        public string DP = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AveryGame Launcher\\Dir.txt");
        public void DFC(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Directory.Delete(DP, true);
            ZipFile.ExtractToDirectory("Launcher.zip", DP);
            File.Delete("Launcher.zip");
            PBT.Text = "";
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText("The AveryGame launcher has finished updating")
                .Show();
            System.Diagnostics.Process.Start(Path.Combine(DP + "\\AGSLauncherV2.exe"));
            this.Close();
        }
        public MainWindow()
        {
            //use local file reading instead of json info
            //paths save in a weird way
            InitializeComponent();
            if (Directory.Exists("ExCont"))
            {
                Directory.Delete("ExCont", true);
            }
            DST();
        }

        private void DST()
        {

            WebClient wc = new WebClient();
            string DATA = wc.DownloadString("https://raw.githubusercontent.com/imstillamazedbyit/2asd7r8342jrdsah109/main/strings.json");
            LauncherCloud json = JsonConvert.DeserializeObject<LauncherCloud>(DATA);
            wc.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DFC);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DPC);
            wc.DownloadFileAsync(new Uri("https://www.googleapis.com/drive/v3/files/" + json.driveid + "?alt=media&key=AIzaSyD3hsuSxEFnxZkgadbUSPt_iyx8qJ4lwWQ"), "Launcher.zip");
        }

        public void DPC(object sender, DownloadProgressChangedEventArgs e)
        {
            PB.Value = e.ProgressPercentage;
            PBT.Text = e.ProgressPercentage.ToString() + "%";
        }

        public class LauncherCloud
        {
            public string driveid { get; set; }
        }
    }
}