﻿using OGL.Features;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Features
{

    public interface IFeatureEditModel: IEditModel
    {
        List<Keyword> Keywords { get; }
        string Name { get; set; }
        string Text { get; set; }
        string Prerequisite { get; set; }
        int Level { get; set; }
        bool Hidden { get; set; }
    }

    public class FeatureEditModel<T>: ProxyModel<Keyword>, IFeatureEditModel where T: Feature
    {
        public T Feature { get; set; }
        public FeatureViewModel Model { get; set; }

        public FeatureEditModel(T feature, IEditModel parent, FeatureViewModel fvm) : base (feature.Keywords, parent)
        {
            Feature = feature;
            Model = fvm;
        }

        public string Name {
            get => Feature.Name;
            set
            {
                if (value == Name) return;
                MakeHistory("Name");
                Feature.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Text
        {
            get => Feature.Text;
            set
            {
                if (value == Text) return;
                MakeHistory("Text");
                Feature.Text = value;
                OnPropertyChanged("Text");
            }
        }

        public string Prerequisite
        {
            get => Feature.Prerequisite;
            set
            {
                if (value == Prerequisite) return;
                MakeHistory("Prerequisite");
                Feature.Prerequisite = value;
                OnPropertyChanged("Prerequisite");
            }
        }

        public int Level
        {
            get => Feature.Level;
            set
            {
                if (value == Level) return;
                MakeHistory("Level");
                Feature.Level = value;
                OnPropertyChanged("Level");
            }
        }
        public bool Hidden
        {
            get => Feature.Hidden;
            set
            {
                if (value == Hidden) return;
                MakeHistory("Hidden");
                Feature.Hidden = value;
                OnPropertyChanged("Hidden");
            }
        }

        public List<Keyword> Keywords => Feature.Keywords;

        public override Task<bool> SaveAsync(bool overwrite)
        {
            Model.Refresh();
            return base.SaveAsync(overwrite);
        }
    }
}
