using HttpStack;

namespace DotNetAtom.Providers;

internal class DefaultHttpContextProvider : IHttpContextProvider
{
	public IHttpContext? HttpContext { get; set; }
}
