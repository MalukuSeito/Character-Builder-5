using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using OGL;
using System.IO;
using OGL.Common;
using CB_5e.Services;
using CB_5e.iOS;

[assembly: Dependency(typeof(HTMLService_IOS))]
namespace CB_5e.iOS
{
    public class HTMLService_IOS : IHTMLService
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