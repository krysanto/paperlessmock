using Microsoft.OpenApi.Models;
using Paperless.FileIO;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });


        builder.Services.Configure<FileStorageServiceOptions>(
                    builder.Configuration.GetSection(FileStorageServiceOptions.FileStorage));

        var app = builder.Build();


        // app.UseDefaultFiles();
        // app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        app.UseSwagger(p => p.RouteTemplate = "swagger/{documentName}/swagger.json");

        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "swagger";
            //TODO: Either use the SwaggerGen generated OpenAPI contract (generated from C# classes)
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mock Server");

            //TODO: Or alternatively use the original OpenAPI contract that's included in the static files
            // c.SwaggerEndpoint("/openapi-original.json", "Mock Server Original");
        });
        app.UseRouting();
        app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        // }

        // app.UseHttpsRedirection();

        // app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}