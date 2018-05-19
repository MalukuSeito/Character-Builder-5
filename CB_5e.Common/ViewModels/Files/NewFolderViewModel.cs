using CB_5e.Models;
using System.IO;

namespace CB_5e.ViewModels.Files
{
    public class NewFolderViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public NewFolderViewModel(DirectoryInfo folder)
        {
            Title = folder.Name;
        }
    }
}