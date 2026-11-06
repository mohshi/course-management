using System.Net;
using System.Net.Http.Json;
using CourseManagemenbt.Models;
using Xunit;

namespace CourseManagemenbt.Tests;

public class CoursesControllerTests(CourseApiFactory factory) : IClassFixture<CourseApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task CourseCrudLifecycleWorks()
    {
        var newCourse = new
        {
            title = "Modern .NET",
            description = "Build web APIs with ASP.NET Core.",
            instructor = "Alex Smith",
            credits = 3,
            startDate = new DateOnly(2026, 9, 1)
        };

        var createResponse = await client.PostAsJsonAsync("/api/courses", newCourse);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdCourse = await createResponse.Content.ReadFromJsonAsync<Course>();
        Assert.NotNull(createdCourse);
        Assert.True(createdCourse.Id > 0);
        Assert.Equal(newCourse.title, createdCourse.Title);

        var getResponse = await client.GetAsync($"/api/courses/{createdCourse.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var updateResponse = await client.PutAsJsonAsync($"/api/courses/{createdCourse.Id}", new
        {
            id = createdCourse.Id,
            title = "Advanced .NET",
            description = newCourse.description,
            instructor = newCourse.instructor,
            credits = 4,
            startDate = newCourse.startDate
        });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var listResponse = await client.GetFromJsonAsync<List<Course>>("/api/courses");
        var updatedCourse = Assert.Single(listResponse!);
        Assert.Equal("Advanced .NET", updatedCourse.Title);
        Assert.Equal(4, updatedCourse.Credits);

        var deleteResponse = await client.DeleteAsync($"/api/courses/{createdCourse.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var missingResponse = await client.GetAsync($"/api/courses/{createdCourse.Id}");
        Assert.Equal(HttpStatusCode.NotFound, missingResponse.StatusCode);
    }

    [Fact]
    public async Task CreateCourseWithMissingTitleReturnsBadRequest()
    {
        var response = await client.PostAsJsonAsync("/api/courses", new
        {
            description = "Missing required title.",
            instructor = "Alex Smith",
            credits = 3,
            startDate = new DateOnly(2026, 9, 1)
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
