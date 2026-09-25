namespace Mercado.Models;

public class RecentlyViewedItem
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public DateTime ViewedAt {get; set;}
}
