using OGL.Common;

namespace Character_Builder_Builder
{
    public delegate void SavedEvent(object sender, string id);
    public interface IMainEditor : IHistoryManager
    {
        event SavedEvent Saved;
        bool Save();
        void Exit();
        void ShowPreview();
    }
}
