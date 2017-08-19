namespace CB_5e.Views
{
    public interface IClipboardService
    {
        byte[] GetImageData();
        string GetTextData();
        void PutTextData(string text, string label = "Text");
    }
}