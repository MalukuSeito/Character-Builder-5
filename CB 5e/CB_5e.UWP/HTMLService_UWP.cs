using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using CB_5e.Views;
using OGL;
using System.IO;
using OGL.Common;
using CB_5e.Services;
using CB_5e.UWP;

[assembly: Dependency(typeof(HTMLService_UWP))]
namespace CB_5e.UWP
{
    public class HTMLService_UWP : IHTMLService
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