using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class AttendanceDataTapMobile
    {
        public DateTime? TransactionTime { get; set; }
        public string TapType { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string Coordinate { get; set; }
    }
}
