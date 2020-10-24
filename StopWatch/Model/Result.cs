using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopWatch.Model
{
    public class Result : Support.Notification
    {
        public int Id { get; set; }
        public string Name_One { get; set; }
        public string Time_One { get; set; }
        public string Name_Two { get; set; }
        public string Time_Two { get; set; }
    }
}
