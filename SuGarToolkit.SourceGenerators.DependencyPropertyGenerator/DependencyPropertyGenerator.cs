using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SuGarToolkit.SourceGenerators
{
    [Generator]
    class DependencyPropertyGenerator : IIncrementalGenerator
    {
        private const string TargetAttributeFullQualifiedName = "SuGarToolkit.SourceGenerators.DependencyPropertyAttribute";

        private struct DependencyPropertyInfo
        {
            public IPropertySymbol PropertySymbol { get; set; }
            public string DefaultValueLiteral { get; set; }
            public string PropertyChangedCallbackLiteral { get; set; }
        }

        private static IncrementalValuesProvider<T> Merge<T>(
            IncrementalValuesProvider<T> one,
            IncrementalValuesProvider<T> other)
        {
            return one.Collect().Combine(other.Collect()).SelectMany((tuple, _) =>
            {
                List<T> merged = new List<T>(tuple.Left.Length + tuple.Right.Length);
                merged.AddRange(tuple.Left);
                merged.AddRange(tuple.Right);
                return merged;
            });
        }

        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            //System.Diagnostics.Debugger.Launch();

            initContext.RegisterPostInitializationOutput(postContext =>
            {
                postContext.AddSource($"{TargetAttributeFullQualifiedName}.g.cs", @"
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
}");
            });

            IncrementalValueProvider<ImmutableArray<DependencyPropertyInfo>> propertyInfosProvider = initContext.SyntaxProvider.ForAttributeWithMetadataName(
                TargetAttributeFullQualifiedName,
                (syntaxNode, _) => syntaxNode is PropertyDeclarationSyntax,
                (syntaxContext, _) =>
                {
                    AttributeData associatedAttribute = syntaxContext.Attributes[0];
                    string defaultValueLiteral = GetPropertyLiteral(associatedAttribute, "DefaultValue");
                    string propertyChangedCallbackLiteral = GetPropertyLiteral(associatedAttribute, "PropertyChanged");
                    if (!string.IsNullOrEmpty(propertyChangedCallbackLiteral))
                    {
                        propertyChangedCallbackLiteral = propertyChangedCallbackLiteral.Substring(1, propertyChangedCallbackLiteral.Length - 2);
                    }
                    if (!string.IsNullOrEmpty(defaultValueLiteral))
                    {
                        return new DependencyPropertyInfo
                        {
                            PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                            DefaultValueLiteral = defaultValueLiteral,
                            PropertyChangedCallbackLiteral = propertyChangedCallbackLiteral
                        };
                    }

                    string defaultValuePath = GetPropertyLiteral(associatedAttribute, "DefaultValuePath");
                    if (string.IsNullOrEmpty(defaultValuePath))
                    {
                        return new DependencyPropertyInfo
                        {
                            PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                            PropertyChangedCallbackLiteral = propertyChangedCallbackLiteral
                        };
                    }
                    else
                    {
                        defaultValuePath = defaultValuePath.Substring(1, defaultValuePath.Length - 2);
                        return new DependencyPropertyInfo
                        {
                            PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                            DefaultValueLiteral = defaultValuePath,
                            PropertyChangedCallbackLiteral = propertyChangedCallbackLiteral
                        };
                    }
                }
            ).Collect();

            initContext.RegisterSourceOutput(propertyInfosProvider, (sourceProductionContext, propertyInfos) =>
            {
                IEnumerable<IGrouping<ISymbol, DependencyPropertyInfo>> groupedByClass = propertyInfos.GroupBy(
                    dependencyPropertyInfo => dependencyPropertyInfo.PropertySymbol.ContainingType,
                    SymbolEqualityComparer.Default);

                foreach (IGrouping<ISymbol, DependencyPropertyInfo> group in groupedByClass)
                {
                    INamedTypeSymbol classSymbol = (INamedTypeSymbol) group.Key;
                    StringBuilder stringBuilder = new StringBuilder().AppendLine($@"
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace {classSymbol.ContainingNamespace}
{{
    {GetAccessibilityLiteral(classSymbol.DeclaredAccessibility)} {(classSymbol.IsAbstract ? "abstract" : "")} partial class {classSymbol.Name}
    {{");
                    foreach (DependencyPropertyInfo dependencyPropertyInfo in group)
                    {
                        string accessModifier = GetAccessibilityLiteral(dependencyPropertyInfo.PropertySymbol.DeclaredAccessibility);
                        string propertyTypeName = $"{dependencyPropertyInfo.PropertySymbol.Type.ContainingNamespace}.{dependencyPropertyInfo.PropertySymbol.Type.Name}";
                        string propertyName = dependencyPropertyInfo.PropertySymbol.Name;
                        string ownerClassName = dependencyPropertyInfo.PropertySymbol.ContainingType.Name;
                        stringBuilder.AppendLine($@"
        {accessModifier} partial {dependencyPropertyInfo.PropertySymbol.Type} {propertyName}
        {{
            get => ({propertyTypeName}) GetValue({propertyName}Property);
            set => SetValue({propertyName}Property, value);
        }}");
                        if (!string.IsNullOrEmpty(dependencyPropertyInfo.DefaultValueLiteral))
                        {
                            stringBuilder.AppendLine($@"
        {accessModifier} static readonly DependencyProperty {propertyName}Property = DependencyProperty.Register(
            nameof({propertyName}),
            typeof({propertyTypeName}),
            typeof({ownerClassName}),
            new PropertyMetadata({dependencyPropertyInfo.DefaultValueLiteral}{(string.IsNullOrEmpty(dependencyPropertyInfo.PropertyChangedCallbackLiteral) ? string.Empty : $", {dependencyPropertyInfo.PropertyChangedCallbackLiteral}")})
        );
                            ");
                        }
                        else
                        {
                            stringBuilder.AppendLine($@"
        {accessModifier} static readonly DependencyProperty {propertyName}Property = DependencyProperty.Register(
            nameof({propertyName}),
            typeof({propertyTypeName}),
            typeof({ownerClassName}),
            new PropertyMetadata(default({propertyTypeName}){(string.IsNullOrEmpty(dependencyPropertyInfo.PropertyChangedCallbackLiteral) ? string.Empty : $", {dependencyPropertyInfo.PropertyChangedCallbackLiteral}")})
        );");
                        }
                    }
                    stringBuilder.AppendLine($@"
    }}
}}");
                    sourceProductionContext.AddSource($"{classSymbol}.DependencyProperty.g.cs", stringBuilder.ToString());
                }
            });
        }

        private static string GetPropertyLiteral(AttributeData attribute, string name)
        {
            foreach (KeyValuePair<string, TypedConstant> pair in attribute.NamedArguments)
            {
                if (pair.Key == name)
                {
                    return pair.Value.ToCSharpString();
                }
            }
            return null;
        }

        private static string GetAccessibilityLiteral(Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Private:
                    return "private";
                case Accessibility.ProtectedAndInternal:
                    return "protected internal";
                case Accessibility.Protected:
                    return "protected";
                case Accessibility.Internal:
                    return "internal";
                case Accessibility.Public:
                    return "public";
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
