using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Frame_for_WP.Model
{
    public class SimpleFontItem
    {
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private FontFamily font;
        public FontFamily Font
        {
            get { return font; }
            set { font = value; }
        }

        public SimpleFontItem(string text)
        {
            this.text = text;
            this.font = new FontFamily(text);
            this.text = text;
        }
    }
}
