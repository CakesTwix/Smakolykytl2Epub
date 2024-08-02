using EpubSharp;
using EpubSharp.Format;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Smakolykytl2Epub.Utils;

public class Content
{
    public string type { get; set; }
    public List<Mark> marks { get; set; }
    public string text { get; set; }
}
public class Attrs
{
    public string src { get; set; }
    public string alt { get; set; }
    public string title { get; set; }
}
public class Mark
{
    public string type { get; set; }
}

public class TextJson
{
    public string type { get; set; }
    public List<Content?> content { get; set; }
    public Attrs attrs { get; set; }
}

public class HtmlConverter
{
    public static string ConvertJsonToHtml(string json, EpubWriter writer)
    {
        var client = new HttpClient();
        var token = JToken.Parse(json);
        return ConvertTokenToHtml(token, client, writer);
    }

    private static string ConvertTokenToHtml(JToken token, HttpClient client, EpubWriter writer)
    {
        var html = "";

        if (token is JArray)
        {
            foreach (var childToken in token.Children()) html += ConvertTokenToHtml(childToken, client, writer);

        }
        else if (token is JObject)
        {

            var text = JsonConvert.DeserializeObject<TextJson>(token.ToString());
            var count = (text?.content?.Count != 0) ? text?.content?.Count : 1;

            for (int i = 0; i < count; i++)
            {

                Content str = text.content[i];
                if (str != null)
                {
                    html += str.text;
                }
                if (str.type == "hardBreak")
                {
                    html += "<br>";
                }
                if (str.type == "paragraph")
                {
                    html += "<p>";
                }

            }
            if (text.type == "image")
            {
                //var webRequest = new HttpRequestMessage(HttpMethod.Get, text.attrs.src);
                //var response = client.Send(webRequest);
                //var type = text.attrs.src.EndsWith(".jpg") ? EpubContentType.ImageJpeg : 
                //text.attrs.src.EndsWith(".png") ? EpubContentType.ImagePng : throw new Exception($"Unknown image ext {text.attrs.src}");
                //using var reader = new StreamReader(response.Content.ReadAsStream());
                //writer.AddFile(text.attrs.src, reader.ReadToEnd(), type);
                //html += text.attrs.src;
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