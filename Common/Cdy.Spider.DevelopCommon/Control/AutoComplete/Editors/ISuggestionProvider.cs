using System.Collections;

namespace Cdy.Spider.DevelopCommon
{
    public interface ISuggestionProvider
    {

        #region Public Methods

        IEnumerable GetSuggestions(string filter);

        #endregion Public Methods

    }
}
