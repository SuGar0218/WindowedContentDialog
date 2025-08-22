using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

internal class NavigationItem
{
    protected NavigationItem(string name, Type pageType, object? parameter = null)
    {
        Name = name;
        PageType = pageType;
        Parameter = parameter;
    }

    protected NavigationItem(Type pageType, object? parameter = null) : this(pageType.Name, pageType, parameter) { }

    public string Name { get; set; }
    public Type PageType { get; set; }
    public object? Parameter { get; set; }
}

internal class NavigationItem<TPage> : NavigationItem where TPage : Page
{
    public NavigationItem(string name, object? parameter = null) : base(name, typeof(TPage), parameter) { }

    public NavigationItem(object? parameter = null) : base(typeof(TPage), parameter) { }
}
