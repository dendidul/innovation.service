using Core.Application.Interfaces.Innovation;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Features.Innovation.Queries
{
    public class GetHistoryWorkflowDataByQuery : IRequest<List<HistoryTable>>
    {

        public string noreg { get; set; }

        public class GetHistoryWorkflowDataByQueryHandler : IRequestHandler<GetHistoryWorkflowDataByQuery, List<HistoryTable>>
        {
            private readonly IItwfsInnovationContext _context;
            private readonly IHrirsInnovationContext _Hrirscontext;

            public GetHistoryWorkflowDataByQueryHandler(IItwfsInnovationContext context, IHrirsInnovationContext Hrirscontext)
            {
                _context = context;
                _Hrirscontext = Hrirscontext;
            }

            public async Task<List<HistoryTable>> Handle(GetHistoryWorkflowDataByQuery request, CancellationToken cancellationToken)
            {
                var vwfguid = await _Hrirscontext.GetWFIDByNodoc(request.noreg, cancellationToken);
                var getData = await _context.GetHistoryWorkflow(vwfguid, request.noreg, cancellationToken);
                return getData;
            }

        }

    }
}
