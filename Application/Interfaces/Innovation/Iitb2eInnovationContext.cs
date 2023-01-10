using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Innovation
{
    public interface Iitb2eInnovationContext
    {
        Task<string> GetNRPDelegasi(string pnrp, CancellationToken cancellationToken);
        Task<string> GetUserIdByNrp(string pnrp, CancellationToken cancellationToken);
        Task<List<string>> GetUserRoles(string user, CancellationToken cancellationToken);
    }
}
