namespace Server.Data;

public class Group
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public long ChannelId { get; set; }
    public Channel Channel { get; set; }
}