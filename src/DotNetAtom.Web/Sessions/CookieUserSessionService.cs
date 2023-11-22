using System;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Framework;
using DotNetAtom.Providers;
using DotNetAtom.Security;
using HttpStack;

namespace DotNetAtom.Sessions;

public class CookieUserSessionService(
	IHttpContextProvider httpContextProvider,
	IUserService userService
) : IUserSessionService
{
	private const string CookieName = ".DOTNETATOM";

	public async Task<IUserInfo> GetCurrentUserAsync()
	{
		var httpContext = httpContextProvider.HttpContext;

		if (httpContext is null)
		{
			return AnonymousUserInfo.Instance;
		}

		var feature = httpContext.Features.Get<IAtomFeature>();

		if (feature is { AtomContext.PortalSettings.User: { UserId: not -1 } user })
		{
			return user;
		}

		if (!httpContext.Request.Cookies.TryGetValue(CookieName, out var cookieValue))
		{
			return AnonymousUserInfo.Instance;
		}

		var index = cookieValue.AsSpan().IndexOf(':');
		var portalId = int.Parse(cookieValue.AsSpan(0, index).ToString());
		var userId = int.Parse(cookieValue.AsSpan(index + 1).ToString());

		user = await userService.GetUserAsync(portalId, userId);

		if (user is null)
		{
			return AnonymousUserInfo.Instance;
		}

		if (feature is { AtomContext.PortalSettings: {} portalSettings })
		{
			portalSettings.User = user;
		}

		return user;
	}

	public Task SetCurrentUserAsync(IUserInfo user)
	{
		var httpContext = httpContextProvider.HttpContext ?? throw new Exception("HttpContext is null");
		var feature = httpContext.Features.Get<IAtomFeature>();

		if (feature is { AtomContext.PortalSettings: {} portalSettings })
		{
			portalSettings.User = user;
		}

		if (user.UserId is -1)
		{
			httpContext.Response.Cookies.Delete(CookieName);
		}
		else
		{
			httpContext.Response.Cookies.Append(CookieName, $"{user.PortalId}:{user.UserId}", new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict
			});
		}

		return Task.CompletedTask;
	}
}
