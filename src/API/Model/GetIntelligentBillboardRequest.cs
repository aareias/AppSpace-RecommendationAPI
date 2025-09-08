namespace API.Model;

public class GetIntelligentBillboardRequest
{
    public required DateTime Since { get; set; }
    public required DateTime Until { get; set; }
    public required int BigRooms { get; set; }
    public required int SmallRooms { get; set; }
    public bool FilterByMostSuccessful { get; set; }
}