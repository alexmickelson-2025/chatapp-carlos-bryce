namespace Logic;

public record Message
{
    public Guid messageid {get; set;}
    public Guid clientid {get;set;}
    public string author {get; set;}
    public string content {get; set;}
    public string creationtime {get; set;}
    public int clockcounter {get; set;}
    public Message(Guid clientid, string author, string content, string creationtime, int clockcounter)
    {
        messageid = Guid.NewGuid();
        this.clientid = clientid;
        this.author = author;
        this.content = content;
        this.creationtime = creationtime;
        this.clockcounter = clockcounter;
    }
}
