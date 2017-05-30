using Character_Builder_5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Character_Builder_Builder.FeatureForms
{
    public partial class ResourceFeatureForm : Form, IEditor<ResourceFeature>
    {
        private ResourceFeature bf;
        public static List<string> RESOURCE_IDS = new List<string>();
        public static List<string> EXCLUSION_IDS = new List<string>();
        public static string convert(Ability a, string s = null)
        {
            StringBuilder sb = new StringBuilder();
            if (s != null && s.Trim() != "" && s.Trim() != "0") sb.Append(s);
            if (a.HasFlag(Ability.Strength)) sb.Append((sb.Length > 0 ? " + " : "") + "StrMod");
            if (a.HasFlag(Ability.Dexterity)) sb.Append((sb.Length > 0 ? " + " : "") + "DexMod");
            if (a.HasFlag(Ability.Constitution)) sb.Append((sb.Length > 0 ? " + " : "") + "ConMod");
            if (a.HasFlag(Ability.Intelligence)) sb.Append((sb.Length > 0 ? " + " : "") + "IntMod");
            if (a.HasFlag(Ability.Wisdom)) sb.Append((sb.Length > 0 ? " + " : "") + "WisMod");
            if (a.HasFlag(Ability.Charisma)) sb.Append((sb.Length > 0 ? " + " : "") + "ChaMod");
            return sb.ToString();
        }
        public ResourceFeatureForm(ResourceFeature f)
        {
            bf = f;
            InitializeComponent();
            basicFeature1.Feature = f;
            foreach (RechargeModifier s in Enum.GetValues(typeof(RechargeModifier))) Recharge.Items.Add(s);
            Recharge.DataBindings.Add("SelectedItem", f, "Recharge", true, DataSourceUpdateMode.OnPropertyChanged);
            foreach (string s in RESOURCE_IDS)
            {
                ResourceID.AutoCompleteCustomSource.Add(s);
                ResourceID.Items.Add(s);
            }
            ResourceID.DataBindings.Add("Text", f, "ResourceID", true, DataSourceUpdateMode.OnPropertyChanged);
            foreach (string s in EXCLUSION_IDS)
            {
                ExclusionID.AutoCompleteCustomSource.Add(s);
                ExclusionID.Items.Add(s);
            }
            ExclusionID.DataBindings.Add("Text", f, "ExclusionID", true, DataSourceUpdateMode.OnPropertyChanged);
            if (f.ValueBonus != Ability.None)
            {
                f.Value = convert(f.ValueBonus, f.Value);
                f.ValueBonus = Ability.None;
            }
            Amount.DataBindings.Add("Text", f, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void ResourceFeatureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ResourceID.Text.Length > 0 && !RESOURCE_IDS.Contains(ResourceID.Text)) RESOURCE_IDS.Add(ResourceID.Text);
            if (ExclusionID.Text.Length > 0 && !EXCLUSION_IDS.Contains(ExclusionID.Text)) EXCLUSION_IDS.Add(ExclusionID.Text);
        }

        public ResourceFeature edit(IHistoryManager history)
        {
            history?.MakeHistory(null);
            ShowDialog();
            return bf;
        }
    }
}
