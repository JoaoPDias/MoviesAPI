using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTO
{
    public class UpdateMovieDTO
    {
        public string Title { get; set; }
        [Required(ErrorMessage = "The Genre is required")]
        [StringLength(50, ErrorMessage = "The genre length cannot exceed 50 characters")]
        public string Genre { get; set; }
        [Required(ErrorMessage = "The Duration is required")]
        [Range(60, 360, ErrorMessage = "The duration should be between 60 and 360 minutes")]
        public int Duration { get; set; }
    }
}
