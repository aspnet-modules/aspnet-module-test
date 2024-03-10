using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Module.Test.Int.Auth;

//    https://gunnarpeipman.com/aspnet-core-integration-test-fake-user/
public abstract class BaseTestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    protected BaseTestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected abstract IEnumerable<string> GetUserRoles();

    protected virtual Guid GetUserId()
    {
        return Guid.NewGuid();
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, GetUserId().ToString()),
            new(ClaimTypes.Name, "Test user"),
            new(ClaimTypes.Email, "test@email.com")
        };
        claims.AddRange(GetUserRoles().Select(role => new Claim(ClaimTypes.Role, role)));
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");
        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}