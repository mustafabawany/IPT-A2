using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace K191273_Q4
{
    public class Program
    {
        public Script scriptData;
        public Program(String Price)
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
        public string name;
        public UI (string name)
        {
            this.name = name;
        }

        public string getScript()
        {
            string json = "";

            if (name != null)
            {
                var path = Environment.GetEnvironmentVariable("folderPath", EnvironmentVariableTarget.Process);

                foreach (string f in Directory.GetDirectories(@path))
                {
                    var filename = Directory.GetFiles(f);

                    for (int i = 0; i < filename.Length; i++)
                    {
                        var fi = new FileInfo(filename[i]);

                        if (fi.Extension == ".json")
                        {
                            String sc = fi.Name.Replace(fi.Extension, "");

                            if (sc.ToLower().Replace(".", "").Replace(" ", "") == name.ToLower().Replace(".", "").Replace(" ", ""))
                            {
                                Program root = JsonConvert.DeserializeObject<Program>(File.ReadAllText(filename[i]));
                                json = JsonConvert.SerializeObject(root, Formatting.Indented);
                                break;
                            }

                        }
                    }
                }
            }
            return json;
        }
    }
    public static class Function1
    {
        [FunctionName("market-summary")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            UI obj = new UI(name);
            
            return new OkObjectResult(obj.getScript());
        }
    }
}
