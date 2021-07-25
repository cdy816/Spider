using ICSharpCode.AvalonEdit.CodeCompletion;

namespace Cdy.Spider.CalculateExpressEditor
{
    public static class AvalonEditExtensions
    {
        public static bool IsOpen(this CompletionWindowBase window) => window?.IsVisible == true;
    }
}