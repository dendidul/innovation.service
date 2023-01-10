using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class HistoryTable
    {
        public string VTASKID { get; set; }
        public string VTASKNAME { get; set; }
        public string VCREA { get; set; }
        public DateTime DCREA { get; set; }
        public string VRESULT { get; set; }
        public string VCOMMENT { get; set; }
        public string DCREAS { get; set; }
    }
}
