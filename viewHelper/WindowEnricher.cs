using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notatki.viewHelper
{
    class WindowEnricher
    {
        public static void enrichWindow(Window window)
        {
            window.MouseLeftButtonDown += delegate { window.DragMove(); };
        }
    }
}
