namespace MoviesAPI.DTO;

public class ReadMovieDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public int Duration { get; set; }
    public DateTime RequestAt { get; set; } = DateTime.Now;
}
