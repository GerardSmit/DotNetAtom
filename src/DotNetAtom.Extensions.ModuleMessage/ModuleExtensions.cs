using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using DotNetAtom.Modules;
using WebFormsCore.UI;
using WebFormsCore.UI.HtmlControls;
using WebFormsCore.UI.WebControls;

namespace DotNetAtom;

public static class ModuleExtensions
{
	public static ValueTask AddModuleMessageAsync(this UserControlBase controlBase, string message, ModuleMessageType type, bool display)
	{
		return AddModuleMessageInner(controlBase.Parent, message, type, display);
	}

	public static ValueTask AddModuleMessageAsync(this ModuleInstanceContext context, string message, ModuleMessageType type, bool display)
	{
		if (context.ModuleControl is Control control)
		{
			return AddModuleMessageInner(control.Parent, message, type, display);
		}

		return default;
	}

	private static async ValueTask AddModuleMessageInner(Control control, string message, ModuleMessageType type, bool display)
	{
		var webActivator = control.WebActivator;
		var messagePlaceHolder = control.Controls.FirstOrDefault(i => i.ID == "MessagePlaceHolder");

		if (messagePlaceHolder is null)
		{
			messagePlaceHolder = webActivator.CreateControl<PlaceHolder>();
			messagePlaceHolder.ID = "MessagePlaceHolder";
			messagePlaceHolder.EnableViewState = false;

			await control.Controls.AddAsync(messagePlaceHolder);
			control.Controls.MoveToFront(messagePlaceHolder);
		}

		var cssClass = type switch
		{
			ModuleMessageType.GreenSuccess => "dnnFormMessage dnnFormSuccess",
			ModuleMessageType.YellowWarning => "dnnFormMessage dnnFormWarning",
			ModuleMessageType.RedError => "dnnFormMessage dnnFormValidationSummary",
			ModuleMessageType.BlueInfo => "dnnFormMessage dnnFormInfo",
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};

		var messageDiv = webActivator.CreateElement("div");
		await messagePlaceHolder.Controls.AddAsync(messageDiv);

		messageDiv.Visible = display;
		messageDiv.Attributes["class"] = cssClass;

		var messageContainer = webActivator.CreateElement("span");
		await messageDiv.Controls.AddAsync(messageContainer);

		messageContainer.InnerText = message;
	}
}

public enum ModuleMessageType
{
	GreenSuccess = 0,
	YellowWarning = 1,
	RedError = 2,
	BlueInfo = 3,
}