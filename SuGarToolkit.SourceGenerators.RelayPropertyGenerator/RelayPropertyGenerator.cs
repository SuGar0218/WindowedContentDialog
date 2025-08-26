using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SuGarToolkit.SourceGenerators
{
    [Generator]
    class RelayPropertyGenerator : IIncrementalGenerator
    {
        private const string TargetAttributeFullQualifiedName = "SuGarToolkit.SourceGenerators.RelayPropertyAttribute";

        private struct RelayPropertyInfo
        {
            public IPropertySymbol PropertySymbol { get; set; }
            public string PropertyPath { get; set; }
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
            IncrementalValueProvider<ImmutableArray<RelayPropertyInfo>> relayPropertyInfosProvider = initContext.SyntaxProvider.ForAttributeWithMetadataName(
                TargetAttributeFullQualifiedName,
                (syntaxNode, _) => syntaxNode is PropertyDeclarationSyntax,
                (syntaxContext, _) => new RelayPropertyInfo
                {
                    PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                    PropertyPath = (string) syntaxContext.Attributes[0].ConstructorArguments[0].Value
                }
            ).Collect();

            initContext.RegisterSourceOutput(relayPropertyInfosProvider, (sourceProductionContext, propertyInfosProvider) =>
            {
                IEnumerable<IGrouping<INamedTypeSymbol, RelayPropertyInfo>> relayPropertyInfoGroupedByClass = propertyInfosProvider.GroupBy<RelayPropertyInfo, INamedTypeSymbol>(relayPropertyInfo => relayPropertyInfo.PropertySymbol.ContainingType, SymbolEqualityComparer.Default);
                foreach (IGrouping<INamedTypeSymbol, RelayPropertyInfo> group in relayPropertyInfoGroupedByClass)
                {
                    //CompilationUnitSyntax complicationUnit = SyntaxFactory
                    //    .CompilationUnit()
                    //    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(nameof(System))));
                    //ClassDeclarationSyntax classDeclaration = SyntaxFactory
                    //    .ClassDeclaration(group.Key.Name)
                    //    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
                    //    .WithModifiers(TokenListOf(group.Key.DeclaredAccessibility));
                    //foreach (RelayPropertyInfo relayPropertyInfo in group)
                    //{
                    //    classDeclaration = classDeclaration.AddMembers(
                    //        SyntaxFactory.PropertyDeclaration(
                    //            SyntaxFactory.ParseTypeName($"{relayPropertyInfo.PropertySymbol.Type}"),
                    //            relayPropertyInfo.PropertySymbol.Name)
                    //        .WithModifiers(TokenListOf(relayPropertyInfo.PropertySymbol.DeclaredAccessibility))
                    //        .WithAccessorList(SyntaxFactory.AccessorList(new SyntaxList<AccessorDeclarationSyntax>(
                    //            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    //                .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ""))));
                    //}
                    StringBuilder stringBuilder = new StringBuilder().AppendLine("using System;");
                    stringBuilder.AppendLine($@"
namespace {group.Key.ContainingNamespace}
{{
    {GetAccessibilityLiteral(group.Key.DeclaredAccessibility)} partial class {group.Key.Name}
    {{");
                    foreach (RelayPropertyInfo relayPropertyInfo in group)
                    {
                        stringBuilder.AppendLine($@"
        {GetAccessibilityLiteral(relayPropertyInfo.PropertySymbol.DeclaredAccessibility)} partial {relayPropertyInfo.PropertySymbol.Type} {relayPropertyInfo.PropertySymbol.Name}
        {{
            get => {relayPropertyInfo.PropertyPath};
            set => {relayPropertyInfo.PropertyPath} = value;
        }}");
                    }
                    stringBuilder.AppendLine($@"
    }}
}}");
                    sourceProductionContext.AddSource($"{group.Key}.RelayProperty.g.cs", stringBuilder.ToString());
                }
            });
        }

        //private static SyntaxTokenList TokenListOf(Accessibility accessibility)
        //{
        //    switch (accessibility)
        //    {
        //        case Accessibility.Private:
        //            return SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
        //        case Accessibility.ProtectedAndInternal:
        //            return SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword), SyntaxFactory.Token(SyntaxKind.InternalKeyword));
        //        case Accessibility.Protected:
        //            return SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
        //        case Accessibility.Internal:
        //            return SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
        //        case Accessibility.Public:
        //            return SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
        //        default:
        //            return SyntaxFactory.TokenList();
        //    }
        //}
    }
}
