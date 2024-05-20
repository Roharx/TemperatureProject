using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Office;

public class CreateOfficeDto
{
    public CreateOfficeDto(string name, string location)
    {
        Name = name;
        Location = location;
    }

    [Required(ErrorMessage = "Office name is required")]
    [StringLength(255, ErrorMessage = "Name must be between 2 and 255 characters", MinimumLength = 2)] 
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Office location is required")]
    [StringLength(255, ErrorMessage = "Location must be between 3 and 255 characters", MinimumLength = 3)]
    public string Location { get; set; }
}