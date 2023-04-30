using BLL.Enums;
using BLL.IServices;

namespace API.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async System.Threading.Tasks.Task Invoke(HttpContext context, ISessionService session, IAuthService authService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var claims = authService.ValidateToken(token!);

        if (claims.Count > 0)
        {
            Guid id = Guid.Parse(claims["nameid"]);
            RoleEnum role = Enumeration.GetByCode<RoleEnum>(claims["role"])!;
            session.SetSession(role, id);

        }

        await _next(context);
    }
}

