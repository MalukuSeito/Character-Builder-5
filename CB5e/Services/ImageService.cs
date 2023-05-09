using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OGL;

namespace CB5e.Services
{
    public class ImageService
    {
        public Dictionary<byte[], string> Cache { get; } = new();
    }
}
