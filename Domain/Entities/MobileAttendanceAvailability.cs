using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class MobileAttendanceAvailability
    {
        public bool IsAllowed { get; set; }
        public string Message { get; set; }
    }
}
