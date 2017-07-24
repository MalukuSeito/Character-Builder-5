using Character_Builder;
using System;
using System.IO;
using Xamarin.Forms;

namespace CB_5e.Models
{
    public class Character : BaseDataObject
    {
        public Player Player { get; set; }

        public string Text
        {
            get { return Player?.Name; }
        }

        public string Description
        {
            get {
                if (Player == null) return null;
                try
                {
                    return Player.GetRaceSubName() + " " + String.Join(" | ", Player.GetClassesStrings());
                } catch (Exception e)
                {
                    return e.Message + e.StackTrace;
                }
                }
        }

        public ImageSource Portrait
        {
            get { if (Player != null && Player.Portrait != null) return ImageSource.FromStream(() => new MemoryStream(Player.Portrait)); return null; }
        }
    }
}
