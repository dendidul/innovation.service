using AHM.Domain.Ahmhrirs.Entities;
using Core.Application.Interfaces.Innovation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Features.Innovation.Queries
{
    public class GetStatusIpByQuery : IRequest<List<AhmhrirsTxnip>>
    {

      
        public string vnrp { get; set; }
        public string vstatus { get; set; }

        public class GetStatusIpByQueryHandler : IRequestHandler<GetStatusIpByQuery, List<AhmhrirsTxnip>>        
        {

            private readonly IHRTxnInnovationContext _context;

            private readonly IHrirsInnovationContext _hrirscontext;

            public GetStatusIpByQueryHandler(IHRTxnInnovationContext context, IHrirsInnovationContext hrirscontext)
            {
                _context = context;
                _hrirscontext = hrirscontext; ;
            }

          

            public async Task<List<AhmhrirsTxnip>> Handle(GetStatusIpByQuery request, CancellationToken cancellationToken)
            {
                return await _hrirscontext.GetStatusIp(request.vnrp,request.vstatus, cancellationToken);
            }

            
        }

    }
}
