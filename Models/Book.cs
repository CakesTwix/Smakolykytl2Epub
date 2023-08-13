namespace Smakolykytl2Epub.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Book
{
    public int id { get; set; }
    public int rank { get; set; }
    public string title { get; set; }
    public List<Chapter> chapters { get; set; }
}

public class Chapter
{
    public int id { get; set; }
    public string title { get; set; }
    public string rank { get; set; }
    public string content { get; set; }
    public DateTime modifiedAt { get; set; }
    public List<object> images { get; set; }
    public Book book { get; set; }
}

public class Books
{
    public List<Book> books { get; set; }
}

