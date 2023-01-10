using Core.Domain.Entities;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Features.Innovation.Queries
{
    public class GetGuidDataByQuery : IRequest<GetGuidResponse>
    {

        public class GetGuidDataByQueryHandler : IRequestHandler<GetGuidDataByQuery, GetGuidResponse>
        {

            public async Task<GetGuidResponse> Handle(GetGuidDataByQuery request, CancellationToken cancellationToken)
            {
                GetGuidResponse result = new GetGuidResponse();
                result.ahmhrirs021_guid = Guid.NewGuid().ToString();

                return result;
            }

        }

    }
}
