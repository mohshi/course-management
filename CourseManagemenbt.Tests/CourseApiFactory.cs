using CourseManagemenbt.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CourseManagemenbt.Tests;

public class CourseApiFactory : WebApplicationFactory<Program>
{
    private readonly string databaseName = $"CoursesTests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions>();
            services.RemoveAll<DbContextOptions<CourseDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<CourseDbContext>>();
            services.AddDbContext<CourseDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));
        });
    }
}
