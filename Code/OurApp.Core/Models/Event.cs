using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Models
{
    public class Event
    {
        private string photo { get; set; }
        private string title { get; set; }
        private string description { get; set; }
        private string startDate { get; set; }
        private string endDate { get; set; }
        private string location { get; set; }
        private int hostID { get; set; }
        private int collaboratorID { get; set; }
    }
}
