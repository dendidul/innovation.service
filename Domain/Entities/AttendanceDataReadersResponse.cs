using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class AttendanceDataReadersResponse
    {
        public string Date { get; set; }
        public string Division { get; set; }
        public string Departement { get; set; }
        public string OrganizationUnit { get; set; }
        public List<AttendanceDataTap> In { get; set; }
        public List<AttendanceDataTap> Out { get; set; }

        public AttendanceDataReadersResponse()
        {
            Date = null;
            Division = null;
            Departement = null;
            OrganizationUnit = null;
            In = new List<AttendanceDataTap>();
            Out = new List<AttendanceDataTap>();
        }
    }

   

    

    

   
}
