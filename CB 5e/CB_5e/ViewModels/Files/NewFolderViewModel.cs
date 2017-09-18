using CB_5e.Models;
using PCLStorage;

namespace CB_5e.ViewModels.Files
{
    public class NewFolderViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public NewFolderViewModel(IFolder folder)
        {
            Title = folder.Name;
        }
    }
}