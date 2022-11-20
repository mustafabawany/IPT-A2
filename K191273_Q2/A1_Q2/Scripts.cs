using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AngleSharp;

namespace A1_Q2
{
    public class Scripts
    {
        public String Script { get; set; }
        public String Price { get; set; }
    }

    public class MainProgram
    {
        public static async void Serialize(String DirPath , String appConfig_Dir)
        {
            var config = Configuration.Default;
            
            String html = File.ReadAllText(DirPath + "\\index.html");
            var context = BrowsingContext.New(config);
            var doc = await context.OpenAsync(req => req.Content(html));

            var tables = doc.QuerySelectorAll("div");

            foreach (var values in tables)
            {
                List<Scripts> listOfScripts = new List<Scripts>();
                var table_class = values.GetAttribute("class");
                var category = "";
                if (table_class == "table-responsive")
                {
                    // PARSING CATEGORY 

                    var categories = values.QuerySelectorAll("h4");

                    foreach (var indv_category in categories)
                    {
                        category = indv_category.InnerHtml;

                        if (category.Contains("&amp;") || category.Contains("\\") || category.Contains("/") || category.Contains("."))
                        {
                            category = category.Replace("&amp;", "&");
                            category = category.Replace("/", "&");
                            category = category.Replace("\\", "&");
                            category = category.Replace(".", " ");
                        }

                        category = category.Trim();

                        // PARSING SCRIPT AND PRICE 

                        var rows = values.QuerySelectorAll("tr");

                        Dictionary<String, String> DictionaryOfScripts = new Dictionary<String, String>();

                        var currentScript = "";
                        var currentPrice = "";
                        foreach (var row in rows)
                        {
                            var row_class = row.GetAttribute("class");

                            if (row_class == "red-text-td" || row_class == "green-text-td")
                            {
                                var columns = row.QuerySelectorAll("td");
                                int i = 0;

                                foreach (var col in columns)
                                {
                                    if (i == 0)
                                    {
                                        currentScript = col.InnerHtml;
                                    }
                                    else if (i == 5)
                                    {
                                        currentPrice = col.InnerHtml;
                                    }
                                    i = i + 1;
                                }
                            }
                            if (!DictionaryOfScripts.ContainsKey(currentScript) && currentScript != "")
                            {
                                currentScript = currentScript.Trim();
                                currentPrice = currentPrice.Trim();
                                if (currentScript.Contains("&amp;"))
                                {
                                    currentScript = currentScript.Replace("&amp;", "&");
                                }
                                DictionaryOfScripts.Add(currentScript, currentPrice);
                            }
                        }
                        foreach (KeyValuePair<String, String> individualScript in DictionaryOfScripts)
                        {
                            listOfScripts.Add(new Scripts());
                            int length = listOfScripts.Count;
                            listOfScripts[length - 1].Script = individualScript.Key;
                            listOfScripts[length - 1].Price = individualScript.Value;
                        }
                    }

                    Directory.CreateDirectory(appConfig_Dir + category);

                    String current = DateTime.Now.ToString("dd MMM yy");
                    var currentTime = DateTime.Now;
                    String xmlName = "Summary" + current.Replace(" ", "") + currentTime.Hour + currentTime.Minute + currentTime.Second;
                    XmlSerializer serializer = new XmlSerializer(listOfScripts.GetType());
                    using (StreamWriter writer = new StreamWriter(@appConfig_Dir + category + "\\" + xmlName + ".xml"))
                    {
                        serializer.Serialize(writer, listOfScripts);
                    }
                }
            }
        }
    }
}
