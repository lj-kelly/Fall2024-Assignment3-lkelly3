using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_lkelly3.Models
{
    public class Movies
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link {  get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public byte[]? Poster { get; set; }
        
        
    }
}
