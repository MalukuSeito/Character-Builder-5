using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CB_5e.Helpers;
using CB_5e.Models;
using CB_5e.Views;
using System.IO;
using Xamarin.Forms;

namespace CB_5e.ViewModels.Files
{
    public class FilesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<object> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public DirectoryInfo Folder { get; private set; }

        public FilesViewModel(DirectoryInfo f)
        {
            Folder = f;
            Title = f.Name;
            Items = new ObservableRangeCollection<object>();
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as Item;
            //    Items.Add(_item);
            //    await DataStore.AddItemAsync(_item);
            //});
        }

        public void ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();

                foreach (DirectoryInfo f in Folder.GetDirectories())
                {
                    if (f.Name.StartsWith(".")) continue;
                    Items.Add(f);
                }
                foreach (FileInfo f in Folder.GetFiles())
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