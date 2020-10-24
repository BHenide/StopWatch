using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StopWatch.Support
{
    static class ReformerNameButton
    {
        public static string Reform(KeyEventArgs KeyE)
        {
            string NewName = null;
            int Digits = KeyE.Key.GetHashCode();
            if(34 <= Digits && Digits <= 43)
            {
                NewName = KeyE.Key.ToString().Substring(1);
            }
            else if (KeyE.Key.ToString() == "System")
            {
                NewName = KeyE.SystemKey.ToString();
            }
            else
            {
                NewName = KeyE.Key.ToString();
            }
            return NewName;
        }
    }
}
