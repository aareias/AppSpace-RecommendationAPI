namespace Application.Requests;

public class GetBillboardRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int BigRooms { get; set; }
    public int SmallRooms { get; set; }
    public bool FilterByMostSuccessful { get; set; }
}