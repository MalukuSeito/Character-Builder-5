using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CB_5e.Helpers;
using CB_5e.Models;
using CB_5e.Views;

using Xamarin.Forms;
using PCLStorage;

namespace CB_5e.ViewModels
{
    public class FilesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<object> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public IFolder Folder { get; private set; }

        public FilesViewModel(IFolder f)
        {
            Folder = f;
            Title = f.Name;
            Items = new ObservableRangeCollection<object>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as Item;
            //    Items.Add(_item);
            //    await DataStore.AddItemAsync(_item);
            //});
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                foreach (IFolder f in await Folder.GetFoldersAsync())
                {
                    if (f.Name.StartsWith(".")) continue;
                    Items.Add(f);
                }
                foreach (IFile f in await Folder.GetFilesAsync())
                {
                    if (f.Name.StartsWith(".")) continue;
                    Items.Add(f);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}