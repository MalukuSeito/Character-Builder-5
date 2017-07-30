using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using CB_5e.Droid;
using CB_5e.Views;
using OGL;
using System.IO;
using OGL.Common;
using CB_5e.Services;

[assembly: Dependency(typeof(HTMLService_Droid))]
namespace CB_5e.Droid
{
    public class HTMLService_Droid : IHTMLService
    {
        public string Convert(IXML obj)
        {
            return obj.ToHTML();
        }

        public void Reset(ConfigManager config)
        {
            HTMLExtensions.Config = config;
            HTMLExtensions.Transforms.Clear();
            HTMLExtensions.Transform = new System.Xml.Xsl.XslCompiledTransform();
        }
    }

}