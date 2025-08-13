using System.ComponentModel.DataAnnotations;

namespace Roadmap.Models;

public class User
{
    [Key] [MaxLength(255)] public string Id { get; init; } // ClaimTypes.NameIdentifier of the OIDC user

    [Required] [MaxLength(80)] public string FirstName { get; init; } // ClaimTypes.GivenName

    [Required] [MaxLength(80)] public string LastName { get; init; } // ClaimTypes.Surname

    [Required] [MaxLength(255)] public string Email { get; init; } // ClaimTypes.Email

    [MaxLength(15)] public string CampusId { get; init; }

    // Whether the account information has been verified by an advisor. This is to guard against
    // people creating accounts using other people's information.
    public bool IsVerified { get; init; }
}