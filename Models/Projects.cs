namespace Smakolykytl2Epub.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Genre
{
    public int id { get; set; }
    public string title { get; set; }
}

public class Image
{
    public int id { get; set; }
    public string url { get; set; }
    public string name { get; set; }
}

public class Project
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string author { get; set; }
    public string translator { get; set; }
    public DateTime modifiedAt { get; set; }
    public string alternatives { get; set; }
    public string release { get; set; }
    public string nation { get; set; }
    public string status { get; set; }
    public string status_translate { get; set; }
    public Image image { get; set; }
    public List<Tag> tags { get; set; }
    public List<Genre> genres { get; set; }
}

public class Projects
{
    public Project project { get; set; }
}

public class Tag
{
    public int id { get; set; }
    public string title { get; set; }
}

