using System.Web.UI;

namespace Demo.Webparts.VisualWebParts
{
    public interface IListFilterString
    {
        string ListNameString { get; }
        string ListFilterString { get; }
        Control TriggerControl { get; }
    }
}
