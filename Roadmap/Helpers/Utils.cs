using System.Security.Claims;

namespace Roadmap.Helpers;

public static class Utils
{
    public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier);
}