namespace CB_5e.Services
{
    public interface IClipboardService
    {
        byte[] GetImageData();
        string GetTextData();
        void PutTextData(string text, string label = "Text");
    }
}