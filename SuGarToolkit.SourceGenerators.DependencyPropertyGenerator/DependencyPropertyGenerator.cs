using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
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
            public bool ManualSetDefaultValue { get; set; }
            public string DefaultValueLiteral { get; set; }
            public AttributeData AssociatedAttribute { get; set; }
        }

        private static IncrementalValuesProvider<T> Merge<T>(
            IncrementalValuesProvider<T> one,
            IncrementalValuesProvider<T> other)
        {
            return one.Collect()
                .Combine(other.Collect())
                .SelectMany((tuple, _) =>
                {
                    List<T> merged = new List<T>(tuple.Left.Length + tuple.Right.Length);
                    merged.AddRange(tuple.Left);
                    merged.AddRange(tuple.Right);
                    return merged;
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

        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            //System.Diagnostics.Debugger.Launch();

            initContext.RegisterPostInitializationOutput(postContext =>
            {
                postContext.AddSource($"{TargetAttributeFullQualifiedName}.g.cs", @"
using System;

namespace SuGarToolkit.SourceGenerators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class DependencyPropertyAttribute : Attribute
    {
        public string DefaultValueName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class DependencyPropertyAttribute<T> : Attribute
    {
        public T DefaultValue { get; set; }
    }
}");
            });

            IncrementalValuesProvider<DependencyPropertyInfo> propertyInfosWithoutDefaultValueProvider = initContext.SyntaxProvider
                .ForAttributeWithMetadataName(
                    TargetAttributeFullQualifiedName,
                    (syntaxNode, _) => syntaxNode is PropertyDeclarationSyntax,
                    (syntaxContext, _) =>
                    {
                        string defaultValueName;
                        AttributeData associatedAttribute = syntaxContext.Attributes[0];
                        defaultValueName = GetPropertyLiteral(associatedAttribute, "DefaultValueName");
                        if (string.IsNullOrEmpty(defaultValueName))
                        {
                            return new DependencyPropertyInfo
                            {
                                PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                                ManualSetDefaultValue = !string.IsNullOrEmpty(defaultValueName),
                                DefaultValueLiteral = defaultValueName,
                                AssociatedAttribute = associatedAttribute
                            };
                        }
                        else
                        {
                            defaultValueName = defaultValueName.Substring(1, defaultValueName.Length - 2);
                            return new DependencyPropertyInfo
                            {
                                PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                                ManualSetDefaultValue = !string.IsNullOrEmpty(defaultValueName),
                                DefaultValueLiteral = defaultValueName,
                                AssociatedAttribute = associatedAttribute
                            };
                        }
                    });

            IncrementalValuesProvider<DependencyPropertyInfo> propertyInfosWithDefaultValueProvider = initContext.SyntaxProvider
                .ForAttributeWithMetadataName(
                    $"{TargetAttributeFullQualifiedName}`1",
                    (syntaxNode, _) => syntaxNode is PropertyDeclarationSyntax,
                    (syntaxContext, _) =>
                    {
                        AttributeData associatedAttribute = syntaxContext.Attributes[0];
                        return new DependencyPropertyInfo
                        {
                            PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                            ManualSetDefaultValue = true,
                            DefaultValueLiteral = GetPropertyLiteral(associatedAttribute, "DefaultValue"),
                            AssociatedAttribute = associatedAttribute
                        };
                    });

            IncrementalValueProvider<ImmutableArray<DependencyPropertyInfo>> allPropertyInfosProvider = Merge(
                propertyInfosWithDefaultValueProvider,
                propertyInfosWithoutDefaultValueProvider
            ).Collect();

            initContext.RegisterSourceOutput(allPropertyInfosProvider, (sourceProductionContext, propertyInfos) =>
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
                        if (dependencyPropertyInfo.ManualSetDefaultValue)
                        {
                            stringBuilder.AppendLine($@"
        {accessModifier} static readonly DependencyProperty {propertyName}Property = DependencyProperty.Register(
            nameof({propertyName}),
            typeof({propertyTypeName}),
            typeof({ownerClassName}),
            new PropertyMetadata({dependencyPropertyInfo.DefaultValueLiteral})
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
            new PropertyMetadata(default({propertyTypeName}))
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
    }
}
