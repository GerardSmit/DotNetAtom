using HttpStack;

namespace DotNetAtom.Providers;

public interface IHttpContextProvider
{
	IHttpContext? HttpContext { get; }
}
