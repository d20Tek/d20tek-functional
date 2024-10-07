using D20Tek.Functional;
using TodoService.Endpoints.Todos;

namespace TodoService;

internal static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) => 
        builder.Services.AddEndpointsApiExplorer()
                        .AddSwaggerGen()
                        .AddCors()
                        .Pipe(_ => builder);

    public static WebApplication ConfigurePipeline(this WebApplication app) =>
        app.ToIdentity()
           .Iter(app => app.Environment.IsDevelopment().IfTrueOrElse(
                 () =>
                 {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                 }))
           .Iter(app => app.UseHttpsRedirection())
           .Iter(app => app.UseCors())
           .Iter(app => app.MapTodoEndpoints());
}
