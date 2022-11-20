using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Net;

namespace K191273_Q1
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        WebClient client = new WebClient();
        
        String websiteLink = System.Configuration.ConfigurationManager.AppSettings["WebLink"];
        String dirPath = System.Configuration.ConfigurationManager.AppSettings["dirPath"];


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            DownloadPage();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 300000;  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            client.Dispose();
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
            DownloadPage();
        }

        private void DownloadPage()
        {
            try
            {
                client.DownloadFile(websiteLink, @dirPath + "\\index.html");
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine("Directory Not Found");
            }
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

    }
}
