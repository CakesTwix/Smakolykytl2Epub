using EpubSharp;
using System.CommandLine;
using Smakolykytl2Epub.Models;
using Smakolykytl2Epub.Utils;

// CLI
var titleIDOption = new Option<int>(
    aliases: ["--title", "-t"],
    description: "Title ID, can be taken from the link"
) {IsRequired = true};
    
var chapterIDOption = new Option<int>(
    aliases: ["--chapter", "-c"],
    description: "Chapter ID, can be taken from the link"
) {IsRequired = true};

var rootCommand = new RootCommand("A simple ranobe loader for Smakolykytl :)");
rootCommand.AddOption(titleIDOption);
rootCommand.AddOption(chapterIDOption);

// Run main program
rootCommand.SetHandler(DownloadTitleAsync, titleIDOption, chapterIDOption);

return rootCommand.InvokeAsync(args).Result;

// Main Program
static async Task DownloadTitleAsync(int titleID, int chapterID){
    var client = new HttpClient();
    var projectTitle = await Ranobe.GetById(titleID);
    if (projectTitle.project != null)
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

        Books? books = await Ranobe.GetChaptersById(titleID);
        Book? book =  books.books[chapterID];

        if (book != null)
        {
            foreach (var item in book.chapters)
            {
                Console.WriteLine("Завантаження розділу: {0}", item.title);
                var content = (await Ranobe.GetChapterById(item.id))!.chapter.content;
                
                writer.AddChapter(item.title, HtmlConverter.ConvertJsonToHtml(content, writer));
                Thread.Sleep(1000);
            }

            // Done
            var fileName = string.Format("{0} - {1}.epub", project.title, book.title);
            Console.WriteLine("Файл збережено як: {0}", fileName);
        }
    } else {
        Console.WriteLine("Нічого не знайшли :(");
    }
}
