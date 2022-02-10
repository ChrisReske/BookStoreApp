using System.ComponentModel.DataAnnotations;

namespace BookStoreAppApi.Models.Author;

public class AuthorUpdateDto : BaseDto
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength (250)]
    public string Bio { get; set; }
}