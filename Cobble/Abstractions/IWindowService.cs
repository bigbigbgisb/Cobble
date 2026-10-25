using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Cobble.Abstractions
{
    public interface IWindowService
    {
        public abstract Dictionary<ViewModelBase, Window> Windows { get; set; }
        public abstract void ShowWindow<T>() where T : ViewModelBase;
        public abstract void CloseWindow<T>() where T : ViewModelBase;
    }
}
