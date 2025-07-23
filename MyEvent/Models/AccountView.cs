using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace MyEvent.Models;

public class LoginVM
{
    [Required(ErrorMessage = "ID is required.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "ID must be exactly 8 digits")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "ID must contain only digits.")]
    [Remote("CheckId", "Account", ErrorMessage = "ID already exists.")]
    public string Id { get; set; }
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [StringLength(100, MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}

public class RegisterMemberVM
{
    [Required(ErrorMessage = "ID is required.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "ID must be exactly 8 digits")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "ID must contain only digits.")]
    [Remote("CheckId", "Account", ErrorMessage = "ID already exists.")]
        public string Id { get; set; }
    [StringLength(100)]
    [EmailAddress]
    [Remote("CheckEmail", "Account", ErrorMessage = "Duplicated {0}.")]
    public string Email { get; set; }

    [StringLength(100, MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [StringLength(100, MinimumLength = 5)]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string Confirm { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    public IFormFile Photo { get; set; }
}
public class RegisterAdminVM
{
    [Required(ErrorMessage = "ID is required.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "ID must be exactly 8 digits")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "ID must contain only digits.")]
    [Remote("CheckId", "Account", ErrorMessage = "ID already exists.")]
    public string Id { get; set; }
    [StringLength(100)]
    [EmailAddress]
    [Remote("CheckEmail", "Account", ErrorMessage = "Duplicated {0}.")]
    public string Email { get; set; }

    [StringLength(100, MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [StringLength(100, MinimumLength = 5)]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string Confirm { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    public IFormFile Photo { get; set; }
}
public class UpdatePasswordVM
{
    [StringLength(100, MinimumLength = 5)]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string Current { get; set; }

    [StringLength(100, MinimumLength = 5)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string New { get; set; }

    [StringLength(100, MinimumLength = 5)]
    [Compare("New")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string Confirm { get; set; }
}

public class UpdateProfileVM
{
    public string? Email { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    public string? PhotoURL { get; set; }

    public IFormFile? Photo { get; set; }
}

public class ResetPasswordVM
{
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }
}

