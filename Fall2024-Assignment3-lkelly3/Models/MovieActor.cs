using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fall2024_Assignment3_lkelly3.Models
{
    public class MovieActor
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Actors")]
        public int ActorId { get; set; }

        public Actors? Actors { get; set; }
        
        [ForeignKey("Movies")]
        public int MovieId { get; set; }
        public Movies? Movies {  get; set; } 
    }
}
