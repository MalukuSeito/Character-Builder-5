namespace OGL.Common
{
    public interface IMatchable
    {
        bool Matches(string text, bool nameOnly);
    }
}