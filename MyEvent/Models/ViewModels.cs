using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

#nullable disable warnings

public class EventVM
{
    [MaxLength(100)]
    //[RegularExpression(@"P\d{3}", ErrorMessage = "Invalid {0} format.")]
    public string Title { get; set; }

    [Remote("CheckDate", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    [Display(Name = "Category")]
    [StringLength(8)]
    [RegularExpression(@"^CAT\d{5}$", ErrorMessage = "Invalid {0}.")]
    [Remote("CheckCategoryId", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public string CategoryId { get; set; }

    [Display(Name = "Address")]
    public string AddressId { get; set; }

    [Range(0, 200, ErrorMessage = "Price must be between 0 and 200")]
    [Remote("CheckPrice", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public decimal Price { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    [Display(Name = "Contact Email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid {0}.")]
    public string ContactEmail { get; set; }

    public IFormFile ImageUrl { get; set; }
}


