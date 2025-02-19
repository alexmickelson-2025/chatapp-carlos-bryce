using System.Text.Json;
using Logic;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public MessageController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("addMessage")]
    public async Task PostMessage([FromForm(Name ="message")] string message, [FromForm(Name ="image")]IFormFile? image)
    {
        Console.WriteLine("Receieved message to add" + message);
        Message receivedMessage = JsonSerializer.Deserialize<Message>(message);
        
        string imagePath = null;

        if (image != null)
        {
            imagePath = await UpdateFile(image);
        }

        if (imagePath is not null)
        {
            receivedMessage.imagepath = imagePath;
        }

        _context.message.Add(receivedMessage);
        await _context.SaveChangesAsync();
    }

    [HttpGet("getMessages")]
    public async Task<List<Message>> GetMessages()
    {
        Console.WriteLine("Getting messages");
        var messages = await _context.message.ToListAsync();
        messages = messages.OrderBy(x => Random.Shared.Next()).ToList();
        Console.WriteLine("There were " + messages.Count + " message found");
        return messages;
    }

    [HttpDelete("deleteAllMessages")]
    public async Task DeleteAllMessages()
    {
        var allRows = await _context.message.ToListAsync();
        _context.message.RemoveRange(allRows);
        await _context.SaveChangesAsync();
    }

    private async Task<string> UpdateFile(IFormFile file)
    {
        Console.WriteLine("got to update file");
        HttpClient httpClient = new HttpClient();

        string Imageurl = Environment.GetEnvironmentVariable("IMAGE_API_URL") ?? throw new Exception("IMAGE_API_URL environment variable not set");

        //Read the file into a stream
        var stream = file.OpenReadStream(); // 10 MB max
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

        MultipartFormDataContent content = new MultipartFormDataContent();
        content.Add(fileContent, "image", file.Name);  // "image" should match your server-side parameter name

        var response = await httpClient.PostAsync(Imageurl + "/Image/addImage", content);
        Console.WriteLine("Response from adding image was " + response);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Image uploaded successfully! Name was " + await response.Content.ReadAsStringAsync());
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            Console.WriteLine("was not succesful!");
            return null;
        }


    }
}

