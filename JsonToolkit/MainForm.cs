using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonToolkit
{
    public partial class MainForm : Form
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        private PrivateFontCollection _fontCollection = new PrivateFontCollection();
        private Font DefaultFont => new Font(_fontCollection.Families[0], 11.0f);

        public MainForm()
        {
            InitializeComponent();
            LoadFont();
            ApplyFont();
        }

        public void LoadJsonClipboard()
        {
            var json = Clipboard.GetText();
            var prettyJson = JToken.Parse(json).ToString(Formatting.Indented);

            txtJson.Text = prettyJson;

            this.Show();
        }

        // Borrowed from https://stackoverflow.com/a/1956043/1769999
        private void LoadFont()
        {
            Stream fontStream = new MemoryStream(Properties.Resources.Cascadia);
            //create an unsafe memory block for the data
            System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);
            //create a buffer to read in to
            Byte[] fontData = new Byte[fontStream.Length];
            //fetch the font program from the resource
            fontStream.Read(fontData, 0, (int)fontStream.Length);
            //copy the bytes to the unsafe memory block
            Marshal.Copy(fontData, 0, data, (int)fontStream.Length);

            // We HAVE to do this to register the font to the system (Weird .NET bug !)
            uint cFonts = 0;
            AddFontMemResourceEx(data, (uint)fontData.Length, IntPtr.Zero, ref cFonts);

            //pass the font to the font collection
            _fontCollection.AddMemoryFont(data, (int)fontStream.Length);
            //close the resource stream
            fontStream.Close();
            //free the unsafe memory
            Marshal.FreeCoTaskMem(data);
        }

        private void ApplyFont()
        {
            foreach (Control x in this.Controls)
            {
                if (x is TextBox)
                {
                    ((TextBox)x).Font = DefaultFont;
                }
            }
        }
    }
}
