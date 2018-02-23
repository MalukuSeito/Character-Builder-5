using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using OGL.Common;
using System.Globalization;
using System.Linq.Expressions;
using OGL.Keywords;

namespace Data_Browser
{
    public interface ITable : IComparable<ITable>
    {
        string Name { get; }
        int Results { get; set; }
        void GetDistinct();
        IEnumerable<IXML> GetValues(string search = null, bool onlyName = false, bool exactMatch = false);
        IEnumerable<IXML> Refine(IEnumerable<IXML> data);
        List<KeyValuePair<MemberInfo, string>> Columns { get; }
        void ResetRefinements();
        void AddControls(FlowLayoutPanel panel);
        string GetValue(MemberInfo mi, object o);
    }

    public class Table<T> : ITable where T: IXML
    {
        public string Name { get; }
        public Dictionary<MemberInfo, List<string>> DistinctValues;
        public Dictionary<MemberInfo, ComboBox> Selectors;
        public List<KeyValuePair<MemberInfo, string>> Columns { get; }
        public int Results { get; set; }
        public Table(string name)
        {
            Name = name;
            Results = 0;
            Columns = new List<KeyValuePair<MemberInfo, string>>
            {
                new KeyValuePair<MemberInfo, string>(PropertyHelper<IXML>.GetProperty(e => e.Name), "Name")
            };
            //if (select != null) Columns.Add(new KeyValuePair<MemberInfo, string>(select, select.Name));
            //if (select2 != null) Columns.Add(new KeyValuePair<MemberInfo, string>(select2, select2.Name));
            //if (select3 != null) Columns.Add(new KeyValuePair<MemberInfo, string>(select3, select3.Name));
            //if (select4 != null) Columns.Add(new KeyValuePair<MemberInfo, string>(select4, select4.Name));
            //if (select5 != null) Columns.Add(new KeyValuePair<MemberInfo, string>(select5, select5.Name));
            //if (select6 != null) Columns.Add(new KeyValuePair<MemberInfo, string>(select6, select6.Name));
            
        }

        public Table()
        {
            Name = typeof(T).Name;
            Results = 0;
            Columns = new List<KeyValuePair<MemberInfo, string>>
            {
                new KeyValuePair<MemberInfo, string>(PropertyHelper<IXML>.GetProperty(e => e.Name), "Name")
            };
        }

        public Table<T> Add<TValue>(Expression<Func<T, TValue>> property, string title = null)
        {
            Expression body = property;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    MemberInfo pi =  (MemberInfo)((MemberExpression)body).Member;
                    Columns.Add(new KeyValuePair<MemberInfo, string>(pi, title ?? pi.Name));
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return this;
        }

        public Table<T> Add<TOther, TValue>(Expression<Func<TOther, TValue>> property, string title = null) where TOther: T
        {
            Expression body = property;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    MemberInfo pi = (MemberInfo)((MemberExpression)body).Member;
                    Columns.Add(new KeyValuePair<MemberInfo, string>(pi, title ?? pi.Name));
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return this;
        }

        //public Table(Type name, Dictionary<MemberInfo, string> columns)
        //{
        //    Name = name;
        //    Columns = new List<KeyValuePair<MemberInfo, string>>
        //    {
        //        new KeyValuePair<MemberInfo, string>(PropertyHelper<IXML>.GetProperty(e => e.Name), "Name")
        //    };
        //    foreach (MemberInfo s in columns.Keys) Columns.Add(new KeyValuePair<MemberInfo, string>(s, columns[s]));
        //    Columns.Add(new KeyValuePair<MemberInfo, string>(PropertyHelper<IXML>.GetProperty(e => e.Source), "Source"));
        //    Results = 0;
        //}
        public override string ToString()
        {
            return Name + (Results > 0 ? " (" + Results.ToString() + ")" : "");
        }
        public int CompareTo(ITable other)
        {
            return this.Name.CompareTo(other.Name);
        }

        public string GetValue(MemberInfo mi, object o)
        {
            if (mi.ReflectedType.IsAssignableFrom(o.GetType())) 
                try
                {
                    object oo = null;
                    if (mi is PropertyInfo pi) oo = pi.GetValue(o);
                    if (mi is FieldInfo fi) oo = fi.GetValue(o);
                    if (oo is List<Keyword> l) return string.Join(", ", l);
                    if (oo is List<string> ls) return string.Join(", ", ls);
                    return oo?.ToString() ?? "";
                } catch (Exception)
                {

                }
            return "";
        }

        public object GetObject(MemberInfo mi, object o)
        {
            if (mi.ReflectedType.IsAssignableFrom(o.GetType()))
                try
                {
                    object oo = null;
                    if (mi is PropertyInfo pi) return pi.GetValue(o);
                    if (mi is FieldInfo fi) return fi.GetValue(o);

                }
                catch (Exception)
                {

                }
            return "";
        }

        public void GetDistinct()
        {
            Columns.Add(new KeyValuePair<MemberInfo, string>(PropertyHelper<IXML>.GetProperty(e => e.Source), "Source"));
            DistinctValues = new Dictionary<MemberInfo, List<string>>();
            Selectors = new Dictionary<MemberInfo, ComboBox>();
            IEnumerable<object> all = GetValues().ToList();
            for (int i = 1; i < Columns.Count; i++)
            {
                if (Columns[i].Value != "")
                {
                    MemberInfo pi = Columns[i].Key;
                    List<string> values = new List<string>();
                    foreach (object o in all) {
                        object oo = GetObject(pi, o);
                        if (oo is List<Keyword> l) values.AddRange(l.Select(s => s.ToString()));
                        else if (oo is List<string> ls) values.AddRange(ls);
                        else if (oo != null) values.Add(oo.ToString());
                    }
                    values = values.OrderBy(s => s).Distinct().ToList();
                    DistinctValues.Add(Columns[i].Key, values);
                    ComboBox box = new ComboBox
                    {
                        DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
                    };
                    box.Items.Add(Columns[i].Value + ":");
                    box.Items.Add("");
                    box.Items.AddRange(values.ToArray());
                    box.Size = new System.Drawing.Size(213, 21);
                    box.Name = pi.Name;
                    Selectors.Add(pi, box);
                }
            }
        }

        private bool Match(IXML r, List<string> match, bool onlyName)
        {
            foreach (string s in match)
            {
                if (!r.Matches(s, onlyName)) return false;
            }
            return true;
        }

        public IEnumerable<IXML> GetValues(string search = null, bool onlyName = false, bool exactMatch = false)
        {
            List<string> match;
            if (!exactMatch && search != null)
            {
                match = search.Split('"')
                     .Select((element, index) => index % 2 == 0  // If even index
                                           ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                           : new string[] { element })  // Keep the entire item
                     .SelectMany(element => element).ToList();
            } else if (search != null && search != "")
            {
                match = new List<string>() { search };
            } else
            {
                match = new List<string>();
            }

            switch (typeof(T).Name)
            {
                case "Skill":
                    return from r in Program.Context.Skills.Values
                           where search == null || search == "" || Match(r, match, onlyName) orderby r.Name, r.Source
                           select r;
                case "Background":
                    return from r in Program.Context.Backgrounds.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "Condition":
                    return from r in Program.Context.Conditions.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "ClassDefinition":
                    return from r in Program.Context.Classes.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "SubClass":
                    return from r in Program.Context.SubClasses.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "Feature":
                    return from r in Program.Context.Features
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source, r.Category
                           select r;
                case "Race":
                    return from r in Program.Context.Races.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "SubRace":
                    return from r in Program.Context.SubRaces.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "Item":
                    return from r in Program.Context.Items.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "Spell":
                    return from r in Program.Context.Spells.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "Language":
                    return from r in Program.Context.Languages.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source
                           select r;
                case "MagicProperty":
                    return from r in Program.Context.Magic.Values
                           where search == null || search == "" || Match(r, match, onlyName)
                           orderby r.Name, r.Source, r.Category
                           select r;
                default:
                    throw new NotImplementedException();
            }
        }

        
        public IEnumerable<IXML> Refine(IEnumerable<IXML> data)
        {
            for (int i = 1; i < Columns.Count; i++)
            {
                ComboBox selector = Selectors[Columns[i].Key];
                if (selector.SelectedIndex > 1)
                {
                    MemberInfo pi = Columns[i].Key;
                    string s = selector.SelectedItem.ToString();
                    data = data.Where(o =>
                    {
                    object oo = GetObject(pi, o);
                    if (oo is List<Keyword> l) return l.Exists(kw => kw.ToString().Equals(s, StringComparison.InvariantCultureIgnoreCase));
                        if (oo is List<string> ls) return ls.Exists(ss => StringComparer.InvariantCultureIgnoreCase.Equals(ss, s));
                        return StringComparer.InvariantCultureIgnoreCase.Equals(oo?.ToString(), s);
                    });
                }
            }
            return data;
        }
        public void ResetRefinements()
        {
            foreach (ComboBox box in Selectors.Values) box.SelectedIndex = 0;
        }

        public void AddControls(FlowLayoutPanel flowLayoutPanel1)
        {
            flowLayoutPanel1.SuspendLayout();
            if (Selectors != null) for (int i = 1; i < Columns.Count; i++) flowLayoutPanel1.Controls.Add(Selectors[Columns[i].Key]);
            flowLayoutPanel1.ResumeLayout();
        }
    }
}
