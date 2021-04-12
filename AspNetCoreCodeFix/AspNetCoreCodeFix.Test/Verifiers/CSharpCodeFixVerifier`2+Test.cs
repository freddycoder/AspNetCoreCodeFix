using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.Collections.Immutable;

namespace AspNetCoreCodeFix.Test
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, MSTestVerifier>
        {
            public Test()
            {
                //ReferenceAssemblies = new ReferenceAssemblies("*");

                //ReferenceAssemblies.AddPackages(new ImmutableArray<PackageIdentity>
                //{
                //    new PackageIdentity("Microsoft.AspNetCore", "2.2.0")
                //});

                SolutionTransforms.Add((solution, projectId) =>
                {
                    var compilationOptions = solution.GetProject(projectId).CompilationOptions;
                    
                    compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                        compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));

                    solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);
                    
                    return solution;
                });
            }
        }
    }
}
