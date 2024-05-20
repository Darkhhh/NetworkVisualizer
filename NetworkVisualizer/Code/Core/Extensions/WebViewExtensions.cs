using Microsoft.Web.WebView2.Wpf;
using System.Text.Json;

namespace NetworkVisualizer.Code.Core.Extensions;

public static class WebViewExtensions
{
    public static async void SetTextAsync(this WebView2 view, string text)
    {
        await view.ExecuteScriptAsync($"editor.setValue('{JsonEncodedText.Encode(text)}');");
    }

    public static async Task<string> GetTextAsync(this WebView2 view)
    {
        var value = await view.ExecuteScriptAsync("editor.getValue();");
        return value;
    }
}
