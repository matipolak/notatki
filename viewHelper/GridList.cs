using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Notatki.viewHelper
{
    class GridList
    {
        public void Init(ScrollViewer scrollViewer)
        {
            Grid mainGrid = new Grid();
            mainGrid.Name = "mainGrid";

            scrollViewer.Content = mainGrid;
        }
    }
}
