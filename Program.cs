using EpubSharp;
using Smakolykytl2Epub.Models;
using Smakolykytl2Epub.Utils;

var client = new HttpClient();

int titleId = int.Parse(args[0]);
int chapsterId = int.Parse(args[1]) - 1;

var projectTitle = await Ranobe.GetById(titleId);
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

    Books? books = await Ranobe.GetChaptersById(titleId);
    Book? book =  books.books[chapsterId];

    if (book != null)
    {
        foreach (var item in book.chapters)
        {
            Console.WriteLine("Завантаження розділу: {0}", item.title);
            var content = (await Ranobe.GetChapterById(item.id))!.chapter.content;
            writer.AddChapter(item.title, HtmlConverter.ConvertJsonToHtml(content));
            Thread.Sleep(1000);
        }

        // Done
        var fileName = string.Format("{0} - {1}.epub", project.title, book.title);
        writer.Write(fileName);
        Console.WriteLine("Файл збережено як: {0}", fileName);
    }
}