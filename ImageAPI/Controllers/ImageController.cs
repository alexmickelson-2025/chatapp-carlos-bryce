using Microsoft.AspNetCore.Mvc;

namespace ImageAPI.Controllers;

[ApiController]
public class ImageController : ControllerBase
{
    string IntervalDelay = Environment.GetEnvironmentVariable("INTERVAL_DELAY") ?? throw new Exception("INTERVAL_DELAY environment variable not set");

    [HttpPost("images/{redirectId}/addImage")]
    public async Task<string> AddImage([FromRoute] string redirectId, [FromForm(Name = "image")] IFormFile image)
    {
        Console.WriteLine("redirectId was " + redirectId);
        await Task.Delay(int.Parse(IntervalDelay));
        Guid newName = Guid.NewGuid();

        Console.WriteLine("Inside of /addImage");
        if (image is null || image.Length == 0)
        {
            throw new Exception("Image is missing or empty");
        }
        var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

        Console.WriteLine($"Image name is {newName}");
        var fileName = $"{newName}.png";
        var filePath = Path.Combine(imageDirectory, fileName);

        // Save the file to the specified path
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return fileName;
    }
}
