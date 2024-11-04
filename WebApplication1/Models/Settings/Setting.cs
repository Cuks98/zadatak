namespace WebApplication1.Models.Settings;

internal sealed class Setting
{
    public int Id { get; set; }
    
    public string Value { get; set; }
    
    public DateTime ValidFrom { get; set; }
}