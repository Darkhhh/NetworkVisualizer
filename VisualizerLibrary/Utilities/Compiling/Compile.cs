using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using VisualizerLibrary.Core.Analysers;

namespace VisualizerLibrary.Utilities.Compiling
{
    public static class Compile
    {
        public static bool TryGetAnalyser(string analyserName, string editorText, out string errors, [MaybeNullWhen(false)] out INetworkAnalyser analyser)
        {
            analyser = null;
            errors = string.Empty;

            var sourceCode = CodeWrap.Analyser(analyserName, editorText);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            if (syntaxTree is null)
            {
                errors += "\nCould'not create syntax tree";
                return false;
            }

            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            if (assemblyPath is null)
            {
                errors += "\nCould'not get object assembly path";
                return false;
            }
            if (!TryGetExeDirectory(out var exePath))
            {
                errors += "\nCould'not get exe assembly path";
                return false;
            }

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(
                    Path.Combine(assemblyPath, "System.Private.CoreLib.dll")),
                MetadataReference.CreateFromFile(
                    Path.Combine(assemblyPath, "System.Console.dll")),
                MetadataReference.CreateFromFile(
                    Path.Combine(assemblyPath, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(
                    Path.Combine(assemblyPath, "System.Collections.dll")),
                MetadataReference.CreateFromFile(
                    Path.Combine(assemblyPath, "System.Linq.dll")),
                MetadataReference.CreateFromFile(
                    Path.Combine(assemblyPath, "System.ObjectModel.dll")),
                MetadataReference.CreateFromFile(
                    Path.Combine(exePath, "WpfApp1.dll")),
                MetadataReference.CreateFromFile(
                    Path.Combine(exePath, "NetworkVisualizer.dll")),
            };

            var compilation = CSharpCompilation.Create(CodeWrap.Namespace)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references)
                .AddSyntaxTrees(syntaxTree);

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            if (!result.Success)
            {
                errors += "Build failed. " + result.Diagnostics.Count() + " errors: " + string.Join("", result.Diagnostics.Select(o => "\n  " + o.ToString()));
                return false;
            }
            ms.Seek(0, SeekOrigin.Begin);

            var assembly = Assembly.Load(ms.ToArray());
            if (assembly is null)
            {
                errors += $"\nCould'not load dynamic assembly";
                return false;
            }

            var instance = assembly.CreateInstance($"{CodeWrap.Namespace}.{analyserName}");
            if (instance is null)
            {
                errors += $"\nCould'not create instance of {CodeWrap.Namespace}.{analyserName}";
                return false;
            }
            if (!instance.GetType().IsAssignableTo(typeof(INetworkAnalyser)))
            {
                errors += $"\nCould'not cast instance of {CodeWrap.Namespace}.{analyserName} to {nameof(INetworkAnalyser)}";
                return false;
            }
            var analyserCast = (INetworkAnalyser)instance;
            if (analyserCast is null)
            {
                errors += $"\nCould'not cast instance of {CodeWrap.Namespace}.{analyserName} to {nameof(INetworkAnalyser)}";
                return false;
            }
            analyser = analyserCast;
            return true;
        }

        private static string GetExeDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(codeBase);
            if (path is null) throw new Exception("Can't get exe path");
            return path;
        }
        private static bool TryGetExeDirectory([MaybeNullWhen(false)] out string path)
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(codeBase);
            if (path is null) return false;
            return true;
        }
    }
}
