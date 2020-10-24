using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StopWatch.View
{
    /// <summary>
    /// Interaction logic for CashButton.xaml
    /// </summary>
    public partial class CashButton : Window
    {
        private bool ButtonName;
        public CashButton(bool _ButtonName)
        {
            InitializeComponent();
            ButtonName = _ButtonName;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            string NewName = Support.ReformerNameButton.Reform(e);
            if (ButtonName)
            {
                Properties.Settings.Default.ButtonCTS = NewName;
            }
            else
            {
                Properties.Settings.Default.ButtonDSR = NewName;
            }
            Close();
        }
    }
}
