using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_lkelly3.Models;

public class Actors
{
    [Required]
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public string Link { get; set; }

    public string Gender { get; set; } 
    public int Age { get; set; }

    public byte[]? Photo { get; set; }
}

