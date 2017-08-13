using System.Threading.Tasks;

namespace CB_5e.Views
{
    public interface IPhotoService
    {
        Task<byte[]> GetImageDataAsync();
    }
}