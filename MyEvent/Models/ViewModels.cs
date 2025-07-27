using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

#nullable disable warnings

public class EventVM
{
    [MaxLength(100)]
    public string Title { get; set; }

    [Remote("CheckDate", "Home", ErrorMessage = "Invalid {0}.")]
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    [Display(Name = "Category")]
    [StringLength(8)]
    [RegularExpression(@"^CAT\d{5}$", ErrorMessage = "Invalid {0}.")]
    [Remote("CheckCategoryId", "Home", ErrorMessage = "Invalid {0}.")]
    public string CategoryId { get; set; }

    [Display(Name = "Address")]
    public string AddressId { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    [Display(Name = "Contact Email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid {0}.")]
    public string ContactEmail { get; set; }
    public string? ImageUrl { get; set; }
}


