using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json;
namespace HtmlSerializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }

        private HtmlHelper()
        {
            AllTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("files/AllTags.json"));
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("files/SelfClosingTags.json"));
        }
    }
}