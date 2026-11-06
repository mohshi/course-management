using CourseManagemenbt.Data;
using CourseManagemenbt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagemenbt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(CourseDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses(CancellationToken cancellationToken)
    {
        return Ok(await dbContext.Courses
            .AsNoTracking()
            .OrderBy(course => course.Title)
            .ToListAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Course>> GetCourse(int id, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(course => course.Id == id, cancellationToken);

        return course is null ? NotFound() : Ok(course);
    }

    [HttpPost]
    public async Task<ActionResult<Course>> CreateCourse(Course course, CancellationToken cancellationToken)
    {
        course.Id = 0;
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCourse(int id, Course course, CancellationToken cancellationToken)
    {
        if (id != course.Id)
        {
            return BadRequest("The route id must match the course id.");
        }

        var existingCourse = await dbContext.Courses.FindAsync([id], cancellationToken);
        if (existingCourse is null)
        {
            return NotFound();
        }

        existingCourse.Title = course.Title;
        existingCourse.Description = course.Description;
        existingCourse.Instructor = course.Instructor;
        existingCourse.Credits = course.Credits;
        existingCourse.StartDate = course.StartDate;

        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses.FindAsync([id], cancellationToken);
        if (course is null)
        {
            return NotFound();
        }

        dbContext.Courses.Remove(course);
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
