using System.Security.Claims;
using System.Text.Json;
using Abstractions.CommonModels;

namespace SportSchool.Http;

public class CurrentHttpContextAccessor : ICurrentHttpContextAccessor
{
    public string IdentityClientId { get; set; } = null!;
    public string? IdentityUserId { get; set; }
    public IEnumerable<string> UserRoles { get; set; } = null!;
    public string MethodName { get; set; } = null!;
    public string? UserName { get; set; }
    public string? UserSurname { get; set; }

    public void SetContext(HttpContext context)
    {
        if (!string.IsNullOrEmpty(IdentityClientId) || !string.IsNullOrEmpty(IdentityUserId) || !string.IsNullOrEmpty(MethodName))
        {
            return;
        }
        var user = context.User;
        var claims = user.Claims.ToList();
        IdentityClientId = user.Claims.FirstOrDefault(x => x.Type == "azp")?.Value ??
                           throw new ApplicationException("client_id не задан в токене!");

        IdentityUserId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        UserRoles = JsonSerializer.Deserialize<RealmAccessModel>(user.Claims.FirstOrDefault(x => x.Type == "realm_access").Value).Roles;
        

        MethodName = context.Request.Method;

        UserName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;

        UserSurname = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
    }
}