#if AVALONIA
using AvaloniaEdit.Highlighting;
#else
using ICSharpCode.AvalonEdit.Highlighting;
#endif

namespace Cdy.Spider.CalculateExpressEditor
{
    public interface IClassificationHighlightColors
    {
        HighlightingColor DefaultBrush { get; }

        HighlightingColor GetBrush(string classificationTypeName);
    }
}