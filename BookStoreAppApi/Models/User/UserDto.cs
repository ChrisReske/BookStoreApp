using System.ComponentModel.DataAnnotations;

namespace BookStoreAppApi.Models.User
{
    public class UserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
    }
}