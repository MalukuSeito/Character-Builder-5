using OGL.Features;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class BasicFeature : UserControl
    {
        private Feature feat;
        public Feature Feature
        {
            get { return feat; }
            set
            {
                NameTF.DataBindings.Clear();
                Hidden.DataBindings.Clear();
                Level.DataBindings.Clear();
                Prereq.DataBindings.Clear();
                Description.DataBindings.Clear();
                feat = value;
                if (value != null)
                {
                    NameTF.DataBindings.Add("Text", value, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
                    Hidden.DataBindings.Add("Checked", value, "Hidden", true, DataSourceUpdateMode.OnPropertyChanged);
                    Level.DataBindings.Add("Value", value, "Level", true, DataSourceUpdateMode.OnPropertyChanged);
                    Prereq.DataBindings.Add("Text", value, "Prerequisite", true, DataSourceUpdateMode.OnPropertyChanged);
                    Binding binding = new Binding("Text", value, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
                    binding.Format += NewlineFormatter.Binding_Format;
                    Description.DataBindings.Add(binding);
                    keywordControl1.Keywords = value.Keywords;
                }
                else keywordControl1.Keywords = null;
            }
        }

        public BasicFeature()
        {
            InitializeComponent();
        }
    }
}
