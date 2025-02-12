using System.Text.Json;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public MessageController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("addMessage")]
    public async Task PostMessage(Message message)
    {
        Console.WriteLine("Receieved message to add" + JsonSerializer.Serialize(message));
        _context.message.Add(message);
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
}