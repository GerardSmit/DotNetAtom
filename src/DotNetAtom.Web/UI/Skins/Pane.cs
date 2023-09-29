using WebFormsCore.UI.WebControls;

namespace DotNetAtom.UI.Skins;

public class Pane
{
    public Pane(HtmlControl pane)
    {
        PaneControl = pane;
    }

    public HtmlControl PaneControl { get; set; }
}
