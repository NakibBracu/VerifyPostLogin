using LoginUsingMiddleWare.CustomeMiddleWares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.UseLoginMiddleware();

app.Run(async context => {
    await context.Response.WriteAsync("No response");
});
app.Run();
