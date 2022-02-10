using System.ComponentModel.DataAnnotations;

namespace BookStoreAppApi.Models.Author;

public class AuthorCreateDto
{

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [StringLength(250)]
    public string Bio { get; set; }
}