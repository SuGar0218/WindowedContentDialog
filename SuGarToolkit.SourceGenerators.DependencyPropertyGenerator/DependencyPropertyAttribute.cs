using System;

namespace SuGarToolkit.SourceGenerators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DependencyPropertyAttribute : Attribute
    {
        /// <summary>
        /// Constant default value.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Path.To.Target.Value
        /// </summary>
        public string DefaultValuePath { get; set; }

        /// <summary>
        /// nameof(DependencyPropertyChangedCallback)
        /// </summary>
        public string PropertyChanged { get; set; }
    }
}