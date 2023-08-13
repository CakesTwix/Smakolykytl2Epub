using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Smakolykytl2Epub.Utils;

public class Content
{
    public string type { get; set; }
    public List<Mark> marks { get; set; }
    public string text { get; set; }
}

public class Mark
{
    public string type { get; set; }
}

public class TextJson
{
    public string type { get; set; }
    public List<Content> content { get; set; }
}

public class HtmlConverter
{
    public static string ConvertJsonToHtml(string json)
    {
        var token = JToken.Parse(json);
        return ConvertTokenToHtml(token);
    }

    private static string ConvertTokenToHtml(JToken token)
    {
        var html = "";

        if (token is JArray)
        {
            foreach (var childToken in token.Children()) html += ConvertTokenToHtml(childToken);
        }
        else if (token is JObject)
        {
            var text = JsonConvert.DeserializeObject<TextJson>(token.ToString());
            foreach (var str in text?.content!)
            {
                if (str.type == "hardBreak") html += "<br>";
                html += str.text;
            }

            html += "<br>";
        }
        else
        {
            html += token.ToString();
        }

        return html;
    }
}