using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.UI.Skins;
using WebFormsCore.UI;

namespace DotNetAtom.UI.Containers.Controls;

public partial class Title : SkinObjectBase
{
	public override async ValueTask RenderAsync(HtmlTextWriter writer, CancellationToken token)
	{
		await writer.WriteAsync(ModuleControl.ModuleContext.Configuration.ModuleTitle);
	}
}
