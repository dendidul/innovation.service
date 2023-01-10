using AHM.Domain.Ahmhrirs.Entities.Innovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class AHMHRIRS021_Info
    {
        public AHMHRIRS021_InfoSect infoSect { get; set; }
        public List<AHMHRIRS021_LISTIP> datadetail { get; set; }
    }
}
