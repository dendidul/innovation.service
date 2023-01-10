using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class InfoAbsensi
    {
        public string ShiftName { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public DateTime? AbsenMasuk { get; set; }
        public DateTime? AbsenKeluar { get; set; }
    }
}
