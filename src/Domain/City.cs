namespace Domain;

public class City
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int Population { get; set; }
    
    public Cinema[] Cinemas { get; set; }
}