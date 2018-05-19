using Character_Builder;
using System;
using System.IO;
using Xamarin.Forms;

namespace CB_5e.Models
{
    public class Character : BaseDataObject
    {

        public Character(Player p)
        {
            Player = p;
            Description = p.GetRaceSubName() + " " + String.Join(" | ", p.GetClassesStrings());
            Portrait = Player.Portrait != null ? ImageSource.FromStream(() => new MemoryStream(Player.Portrait)) : null;
            Text = Player.Name;
        }
        public Player Player { get; private set; }

        public string Text { get; private set; }
        //{
        //    get { return Player?.Name; }
        //}

        public string Description { get; private set; }
        //{
        //    get {
        //        if (Player == null) return null;
        //        try
        //        {
        //            return Player.GetRaceSubName() + " " + String.Join(" | ", Player.GetClassesStrings());
        //        } catch (Exception e)
        //        {
        //            return e.Message + e.StackTrace;
        //        }
        //        }
        //}

        public ImageSource Portrait { get; private set; }
        //{
        //    get { if (Player != null && Player.Portrait != null) return ImageSource.FromStream(() => new MemoryStream(Player.Portrait)); return null; }
        //}
    }
}
