using System.Threading.Tasks;

namespace CB_5e.Services
{
    public interface IPhotoService
    {
        Task<byte[]> GetImageDataAsync();
    }
}