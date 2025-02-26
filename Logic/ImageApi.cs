public class ImageApi
{
    public Guid id { get; set; }
    public string imagepath { get; set; }
    public int apiport { get; set; }

    public ImageApi(string imagepath, int apiport)
    {
        this.id = Guid.NewGuid();
        this.imagepath = imagepath;
        this.apiport = apiport;
    }
}