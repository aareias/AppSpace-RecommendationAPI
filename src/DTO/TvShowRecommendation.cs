namespace DTO;

public class TvShowRecommendation : Recommendation
{
    public int Seasons { get; set; }
    
    public int Episodes { get; set; }
    
    public bool Finished { get; set; }
}