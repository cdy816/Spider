#if AVALONIA
using AvaloniaEdit.CodeCompletion;
#else
using ICSharpCode.AvalonEdit.CodeCompletion;
#endif

namespace Cdy.Spider.CalculateExpressEditor
{
    public interface ICompletionDataEx : ICompletionData
    {
        bool IsSelected { get; }

        string SortText { get; }
    }

    public interface IOverloadProviderEx : IOverloadProvider
    {
        void Refresh();
    }
}