using NetworkVisualizer.Code.Core.Data;
using System.Collections.ObjectModel;
using VisualizerLibrary.Core.Analysers;
using VisualizerLibrary.Core.Networks;
using VisualizerLibrary.Utilities;

namespace NetworkVisualizer.Code.MVVM.Models.CodeEditorPage;

public class CodeEditorDocumentsHandler
{
    public void AddNewEmptyDocument(ObservableCollection<CodeDocument> docs)
    {
        docs.Add(new CodeDocument { Title = "New Document", Code = "//Your code here" });
    }

    public void AddNewAnalyserDocument(ObservableCollection<CodeDocument> docs)
    {
        docs.Add(new CodeDocument
        {
            Title = "New Analyser",
            Code = 
            $$"""
            public class AnalyserSample : INetworkAnalyser
            {
            {{CodeToText.FromInterface<INetworkAnalyser>()}}
            }
            """
        });
    }


    public void AddNewNetwork(ObservableCollection<CodeDocument> docs)
    {
        docs.Add(new CodeDocument
        {
            Title = "New Network",
            Code = $$"""
                   public class NetworkSample : NetworkBase
                   {
                   {{CodeToText.FromAbstractClass<NetworkBase>()}}
                   }
                   """
        });
    }
}
