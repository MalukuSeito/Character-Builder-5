using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using OGL.Common;

namespace Character_Builder_Builder
{
    public partial class ImageChooser : UserControl
    {
        public IHistoryManager History { get; set; }
        private IImageEditor image;
        public IImageEditor Image {
            get {
                return image;
            }
            set {
                if (image != null) image.ImageChanged -= Image_ImageChanged;
                image = value;
                if (image != null) image.ImageChanged += Image_ImageChanged;
            }
        }

        private void Image_ImageChanged(object sender, Bitmap Image)
        {
            portraitBox.Image = Image;
        }

        public ImageChooser()
        {
            InitializeComponent();
        }

        private void portraitBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                String s = (String)e.Data.GetData(DataFormats.StringFormat);
                if (s.StartsWith("data:"))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (file.Count() == 1) e.Effect = DragDropEffects.Copy;
                else e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.Clear(Color.Transparent);
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private Bitmap cutToSize(Bitmap b)
        {
            if (b.Width > 360 || b.Height > 360)
            {
                double scale = Math.Min(360.0 / b.Width, 360.0 / b.Height);
                var scaleWidth = (int)(b.Width * scale);
                var scaleHeight = (int)(b.Height * scale);
                return ResizeImage(b, scaleWidth, scaleHeight);
            }
            return b;
        }

        private void portraitBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                History?.MakeHistory("");
                image.SetImage(cutToSize((Bitmap)e.Data.GetData(DataFormats.Bitmap)));
            }
            else if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                String s = (String)e.Data.GetData(DataFormats.StringFormat);
                Match m = Regex.Match(s, @"data:\s*;\s*base64\s*,\s*(?<data>.+)");
                if (m.Success)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(m.Groups["data"].Value)))
                        {
                            History?.MakeHistory("");
                            image.SetImage(cutToSize(new Bitmap(ms)));
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading drag/drop object ");
                    }
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    History?.MakeHistory("");
                    string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (file.Count() == 1) image.SetImage(cutToSize(new Bitmap(file[0])));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading drag/drop object ");
                }
            }
        }

        private void choosePortrait_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose an Image";
            List<String> extensions = new List<string>();
            foreach (string s in (from c in ImageCodecInfo.GetImageEncoders() select c.FilenameExtension)) extensions.AddRange(s.Split(';'));
            ofd.Filter = "Image Files | *." + String.Join(";", extensions);
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                try
                {
                    History?.MakeHistory("");
                    image.SetImage(cutToSize(new Bitmap(ofd.FileName)));
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, "Error loading file " + ofd.FileName);
                }
            }
        }

        private void removePortrait_Click(object sender, EventArgs e)
        {
            image.SetImage(null);
        }
    }
}
