using System.Text.Json;
using Logic;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/messagesapi/api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    string IntervalDelay = Environment.GetEnvironmentVariable("INTERVAL_DELAY") ?? throw new Exception("INTERVAL_DELAY environment variable not set");
    private Dictionary<int, int> portToApiNum = new Dictionary<int, int>{
        {5105, 1},
        {5106, 2},
        {5107, 3}
    };

    public MessageController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("addMessage")]
    public async Task PostMessage([FromForm(Name = "message")] string message, [FromForm(Name = "image")] IFormFile? image)
    {
        await Task.Delay(int.Parse(IntervalDelay));
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
        await Task.Delay(int.Parse(IntervalDelay));
        Console.WriteLine("Getting messages");
        var messages = await _context.message.ToListAsync();
        messages = messages.OrderBy(x => Random.Shared.Next()).ToList();
        Console.WriteLine("There were " + messages.Count + " message found");
        return messages;
    }

    [HttpGet("getPort")]
    public async Task<int> GetPort([FromQuery] string imagePath)
    {
        await Task.Delay(int.Parse(IntervalDelay));
        Console.WriteLine($"received image path : {imagePath}");
        Console.WriteLine("Getting port");
        var imagePorts = await _context.imageapi.ToListAsync();
        var port = imagePorts.Where(i => i.imagepath == imagePath).FirstOrDefault().apiport;
        Console.WriteLine("The port found was " + port);
        return port;
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

        //TODO: generate random number 0,1, or 2. use that as the api
        Random random = new Random();
        int randomPort = random.Next(5105, 5108);

        Console.WriteLine("Before adding image url was " + Imageurl + "/images/" + portToApiNum[randomPort] + "/addImage");

        await Task.Delay(int.Parse(IntervalDelay));
        var response = await httpClient.PostAsync(Imageurl + "/images/" + portToApiNum[randomPort] + "/addImage", content);
        Console.WriteLine("Response from adding image was " + response);

        string filePath = await response.Content.ReadAsStringAsync();
        _context.imageapi.Add(new ImageApi(filePath, randomPort));
        await _context.SaveChangesAsync();

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Image uploaded successfully! Name was " + filePath + " and port was " + randomPort);
            return filePath;
        }
        else
        {
            Console.WriteLine("was not succesful!");
            return null;
        }


    }
}

