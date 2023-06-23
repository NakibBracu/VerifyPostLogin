using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsingMiddleWare.CustomeMiddleWares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/" && context.Request.Method == "POST")
            {
                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    string requestBody = await reader.ReadToEndAsync();

                    Dictionary<string, StringValues> queryDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(requestBody);

                    string email = queryDict.ContainsKey("email") ? queryDict["email"].ToString() : null;
                    string password = queryDict.ContainsKey("password") ? queryDict["password"].ToString() : null;

                    // Valid email and password as per the requirement specification
                    string validEmail = "admin@example.com";
                    string validPassword = "admin1234";
                    bool isValidLogin = (email == validEmail && password == validPassword);

                    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || !isValidLogin)
                    {
                        context.Response.StatusCode = 400;
                        if (email != validEmail)
                            await context.Response.WriteAsync("Invalid input for 'email'\n");
                        if (password != validPassword)
                            await context.Response.WriteAsync("Invalid input for 'password'\n");
                        else if (!isValidLogin)
                            await context.Response.WriteAsync("Invalid login\n");
                    }
                    else
                    {
                        await context.Response.WriteAsync("Successful login\n");
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }











    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
