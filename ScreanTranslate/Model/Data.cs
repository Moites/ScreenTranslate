using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreanTranslate.Model
{
    public class Hotkey
    {
        public Keys Key { get; set; }
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }
        public bool Status { get; set; }
    }
    public class DataDB
    {
        public string Text { get; set; }
        public string TranslateText { get; set; }
        public byte[] Screenshot { get; set; }
        public DateTime DataTime { get; set; }

    }
}
