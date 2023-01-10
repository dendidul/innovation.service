using Core.Application.Interfaces.Innovation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Features.Innovation.Queries
{
    public class GetPeriodIdDataByQuery : IRequest<string>
    {
        public class GetPeriodIdDataByQueryHandler : IRequestHandler<GetPeriodIdDataByQuery, string>
        {
            private readonly IHrirsInnovationContext _hrirscontext;

            public GetPeriodIdDataByQueryHandler(IHrirsInnovationContext hrirscontext)
            {
                _hrirscontext = hrirscontext;
            }

            public async Task<string> Handle(GetPeriodIdDataByQuery request, CancellationToken cancellationToken)
            {
                var result = await _hrirscontext.GetPeriodID(cancellationToken);
                return result;
            }
        }
    }
}
