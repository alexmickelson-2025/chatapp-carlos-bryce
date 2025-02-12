namespace Logic;

public record Message
{
    public int id {get; set;}
    public string author {get; set;}
    public string content {get; set;}
    public string creationtime {get; set;}
    public Message(int id, string author, string content, string creationtime)
    {
        this.id = id;
        this.author = author;
        this.content = content;
        this.creationtime = creationtime;
    }
}
