using VisualizerLibrary.Core.Analysers;

namespace VisualizerLibrary.Utilities.Compiling;

public static class CodeWrap
{
    public const string Namespace = "DynamicCode";

    public static string Analyser(string className, string text)
    {
        var wrap =
            $$"""
                {{GetCommonUsings()}}

                namespace {{Namespace}};

                public class {{className}} : {{nameof(INetworkAnalyser)}}
                {
                    {{text}}
                }
                """;
        return wrap;
    }


    private static string GetCommonUsings()
    {
        return $$"""
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;
                using System.Collections.ObjectModel;
                using NetworkVisializer.Code.Core;
                """;
    }
}
