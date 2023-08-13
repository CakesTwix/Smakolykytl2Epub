using EpubSharp;
using Smakolykytl2Epub.Utils;

var client = new HttpClient();


var projectTitle = await Ranobe.GetById(int.Parse(args[0]));
if (projectTitle != null)
{
    // Print
    var project = projectTitle.project;
    Console.WriteLine(project.title);
    Console.WriteLine(project.alternatives);
    Console.WriteLine(project.description);

    // Basic Info
    var writer = new EpubWriter();
    writer.AddAuthor(project.author);
    writer.SetTitle(project.title);

    // Add Cover Image
    using (var response = await client.GetAsync(project.image.url))
    {
        var imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        writer.SetCover(imageBytes, ImageFormat.Png);
    }

    var books = (await Ranobe.GetChaptersById(int.Parse(args[0])))?.books[int.Parse(args[1]) - 1];
    if (books != null)
        foreach (var item in books.chapters)
        {
            Console.WriteLine(item.title);
            var content = (await Ranobe.GetChapterById(item.id))!.chapter.content;
            writer.AddChapter(item.title, HtmlConverter.ConvertJsonToHtml(content));
            Thread.Sleep(1000);
        }

    // Done
    writer.Write("new.epub");
}