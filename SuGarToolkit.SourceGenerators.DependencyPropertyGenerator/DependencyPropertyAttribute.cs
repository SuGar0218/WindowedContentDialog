using System;

namespace SuGarToolkit.SourceGenerators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DependencyPropertyAttribute : Attribute
    {
        public object DefaultValue { get; set; }

        public string DefaultValuePath { get; set; }

        //public Microsoft.UI.Xaml.PropertyChangedCallback OnPropertyChanged { get; set; }
    }
}