using DncyTemplate.Application.Models.Application.Navigation;

namespace DncyTemplate.Mvc.Views.Shared.Components.SideBarMenu;

public class SideBarMenuViewModel
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public Dictionary<string, string> MetaData { get; set; }

    public IList<MenuItemModel> Items { get; set; }
}