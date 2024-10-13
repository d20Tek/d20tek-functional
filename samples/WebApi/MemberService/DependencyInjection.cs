using D20Tek.Functional;

namespace MemberService;

internal static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) =>
        builder.Services.AddEndpointsApiExplorer()
                        .AddSwaggerGen()
                        .AddCors()
                        .AddControllers()
                        //.AddRepository()
                        .Pipe(_ => builder);

    public static WebApplication ConfigurePipeline(this WebApplication app) =>
        app.ToIdentity()
           .Iter(app => app.UseSwaggerInDev()
                           .UseHttpsRedirection()
                           .UseCors())
           .Iter(app => app.MapControllers());

    private static WebApplication UseSwaggerInDev(this WebApplication app) =>
        app.Pipe(a => a.Environment.IsDevelopment()
                       .IfTrueOrElse(
                            () => a.UseSwagger()
                                   .UseSwaggerUI()))
           .Pipe(a => a);
}
