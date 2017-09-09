namespace CB_5e.Views
{
    public class SelectOption
    {
        public string Text {get; set;}
        public string Detail { get; set; }
        public object Value { get; set; }
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