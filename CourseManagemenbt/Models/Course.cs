using System.ComponentModel.DataAnnotations;

namespace CourseManagemenbt.Models;

public class Course
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required, StringLength(200)]
    public string Instructor { get; set; } = string.Empty;

    [Range(1, 20)]
    public int Credits { get; set; }

    public DateOnly StartDate { get; set; }
}
