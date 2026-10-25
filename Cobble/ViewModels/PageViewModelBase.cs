using Cobble.Abstractions;
using Cobble.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cobble.ViewModels
{
    public abstract class PageViewModelBase(ThemeViewModel themeViewModel,IWindowService windowService) : ViewModelBase(themeViewModel)
    {
        protected readonly IWindowService _windowService = windowService;
    }
}
