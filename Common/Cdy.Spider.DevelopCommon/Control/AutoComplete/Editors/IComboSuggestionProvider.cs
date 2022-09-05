using System.Collections;


namespace Cdy.Spider.DevelopCommon
{
    public interface IComboSuggestionProvider
    {

        #region Public Methods

        IEnumerable GetSuggestions(string filter);
        IEnumerable GetFullCollection();

        #endregion Public Methods
    }
}
