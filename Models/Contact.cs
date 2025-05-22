using System.ComponentModel.DataAnnotations;

namespace ContactsApi.Models;

public class Contact
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string Phone { get; set; } = string.Empty;
}