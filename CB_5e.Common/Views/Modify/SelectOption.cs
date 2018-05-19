﻿using Xamarin.Forms;

namespace CB_5e.Views.Modify
{
    public class SelectOption
    {
        public string Text {get; set;}
        public string Detail { get; set; }
        public object Value { get; set; }
        public Color TextColor { get => Color.Default; }
        public SelectOption()
        {

        }
        public SelectOption (string text, string detail, object val = null)
        {
            Text = text;
            Detail = detail;
            Value = val;
        }
    }
}