namespace Web_153504_Padvalnikau.Domain.Entities;

public class Sneaker
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public int CategoryId{ get; set; } 
    public double Price { get; set; } 
    public string? Image { get; set; } 
    
}