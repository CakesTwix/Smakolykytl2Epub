using Newtonsoft.Json;
using Smakolykytl2Epub.Models;

namespace Smakolykytl2Epub.Utils;

public class Ranobe
{
    private const string ApiUrl = "https://api.smakolykytl.site/api/user/";
    private const string SiteUrl = "https://smakolykytl.site/";
    private static readonly HttpClient Client = new();

    public static async Task<Projects?> GetById(int id)
    {
        var response = await Client.GetAsync(ApiUrl + "projects/" + id);
        var content = await response.Content.ReadAsStringAsync();
        // Console.WriteLine(content);
        return JsonConvert.DeserializeObject<Projects>(content);
    }

    public static async Task<Books?> GetChaptersById(int id)
    {
        var response = await Client.GetAsync(ApiUrl + "projects/" + id + "/books");
        var content = await response.Content.ReadAsStringAsync();
        // Console.WriteLine(content);
        return JsonConvert.DeserializeObject<Books>(content);
    }

    public static async Task<Chapters?> GetChapterById(int id)
    {
        var response = await Client.GetAsync(ApiUrl + "chapters/" + id);
        var content = await response.Content.ReadAsStringAsync();
        // Console.WriteLine(content);
        return JsonConvert.DeserializeObject<Chapters>(content);
    }
}