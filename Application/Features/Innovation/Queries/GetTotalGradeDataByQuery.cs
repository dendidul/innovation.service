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
    public class GetTotalGradeDataByQuery : IRequest<AHMHRIRS021_IPTOTALGRADE>
    {
        public string vnrp { get; set; }
        public string vperiodId { get; set; }
        public class GetTotalGradeDataByQueryHandler : IRequestHandler<GetTotalGradeDataByQuery, AHMHRIRS021_IPTOTALGRADE>
        {
            private readonly IHrirsInnovationContext _hrirscontext;

            public GetTotalGradeDataByQueryHandler(IHrirsInnovationContext hrirscontext)
            {
                _hrirscontext = hrirscontext;
            }

            public async Task<AHMHRIRS021_IPTOTALGRADE> Handle(GetTotalGradeDataByQuery request, CancellationToken cancellationToken)
            {
                AHMHRIRS021_IPTOTALGRADE result = new AHMHRIRS021_IPTOTALGRADE();
                var getData = await _hrirscontext.GetTotalGrade(request.vnrp,request.vperiodId,cancellationToken);

                result.VGRADEA = getData.Count(x => x.Vgrade == "A").ToString();
                result.VGRADEB = getData.Count(x => x.Vgrade == "B").ToString();
                result.VGRADEC = getData.Count(x => x.Vgrade == "C").ToString();

                return result;
            }
        }
    }
}
