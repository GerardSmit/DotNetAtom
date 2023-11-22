using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace DotNetAtom;

public static class FileInfoExtensions
{
	public static async ValueTask<string> ReadAllTextAsync(this IFileInfo file)
	{
		using var stream = file.CreateReadStream();
		using var reader = new StreamReader(stream);

		return await reader.ReadToEndAsync().ConfigureAwait(false);
	}
}
