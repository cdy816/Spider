using System.Threading.Tasks;

namespace Cdy.Spider.CalculateExpressEditor
{
    public interface ICodeEditorCompletionProvider
    {
        Task<CompletionResult> GetCompletionData(int position, char? triggerChar, bool useSignatureHelp);
    }
}