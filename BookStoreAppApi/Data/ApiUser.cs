using Microsoft.AspNetCore.Identity;

namespace BookStoreAppApi.Data;

public class ApiUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; } 
}