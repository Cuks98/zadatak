namespace WebApplication1.Models.Settings;

internal sealed class Setting
{
    public int Id { get; set; }
    
    public decimal Value { get; set; }
    
    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }
}