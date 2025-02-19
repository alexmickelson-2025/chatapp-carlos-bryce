using Microsoft.AspNetCore.Mvc;

namespace ImageAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    [HttpPost("addImage")]
    public async Task<string> AddImage([FromForm(Name ="image")] IFormFile image)
    {

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
