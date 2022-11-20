using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace K191273_Q3
{
    public class Program
    {
        public Script scriptData;
        public Program (String Price)
        {
            scriptData = new Script();
            scriptData.Update(Price);
        }
        public void Update(String Price)
        {
            scriptData.Update(Price);
        }
    }

    public class Script
    {
        public String lastUpdatedOn { set; get; }
        public List<lastUpdates> Updates;

        public Script()
        {
            Updates = new List<lastUpdates>();
        }
        public void Update(String price)
        {
            lastUpdatedOn = DateTime.Now.ToString("ddMMyyHHmmss");
            lastUpdates obj = new lastUpdates();
            obj.Date = DateTime.Now.ToString("ddMMyyHHmmss");
            obj.Price = price;
            Updates.Add(obj);
        }
    }
    public class lastUpdates
    {
        public String Date { set; get; }
        public String Price { set; get; }
    }

    public class UI
    {
        public Dictionary<String, String> DictionaryOfScripts;
        public List<String> Categories;
        public UI()
        {
            DictionaryOfScripts = new Dictionary<string, string>();
            Categories = new List<String>();
        }
        public void deserializeXML(string path)
        {
            foreach (string f in Directory.GetDirectories(@path))
            {
                var filename = Directory.GetFiles(f);

                var fi = new FileInfo(filename[filename.Length - 1]);
                int c = 0;
                while (fi.Extension != ".xml")
                {
                    fi = new FileInfo(filename[filename.Length - 1 - c]);
                    c = c + 1;
                }

                using (XmlReader reader = XmlReader.Create(@filename[filename.Length - 1 - c]))
                {
                    var currentScript = "";
                    var currentPrice = "";
                    var temp = "";

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "Script":
                                    currentScript = reader.ReadString().ToString();
                                    break;
                                case "Price":
                                    currentPrice = reader.ReadString().ToString();
                                    break;
                            }

                            if (!DictionaryOfScripts.ContainsKey(currentScript) && currentScript != "" && currentPrice != "" && currentPrice != temp)
                            {
                                temp = currentPrice;
                                DictionaryOfScripts.Add(currentScript, currentPrice);
                                Categories.Add(f.Substring(f.LastIndexOf("\\") + 1));
                            }
                        }
                    }
                }

                File.Delete(filename[filename.Length - 1 - c]);
            }
        }
    }
    public class Function1
    {
        [FunctionName("ConvertJSON")]
        public void Run([TimerTrigger("0 */20 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            UI obj = new UI();

            var path = Environment.GetEnvironmentVariable("folderPath", EnvironmentVariableTarget.Process); 

            obj.deserializeXML(path);

            int i = 0;
            foreach (KeyValuePair<String, String> item in obj.DictionaryOfScripts)
            {
                //Check if the file already exists

                var filePath = path + "\\" + obj.Categories[i] + "\\" + item.Key.Replace(".", "") + ".json";

                if (File.Exists(filePath))
                {
                    Program root = JsonConvert.DeserializeObject<Program>(File.ReadAllText(filePath));
                    root.Update(item.Value);
                    string json = JsonConvert.SerializeObject(root);
                    File.WriteAllText(filePath, json);
                }
                else
                {
                    Program root = new Program(item.Value);
                    string json = JsonConvert.SerializeObject(root);
                    File.WriteAllText(filePath, json);  
                }
                i = i + 1;
            }  
        }
    }
}
       