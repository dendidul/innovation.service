using AHM.Authentication.Jwt.Authorization;
using AHM.Domain.Ahmhrirs.Entities;
using AHM.Domain.Ahmhrirs.Entities.Innovation;
using AHM.WebApi.Common.Controllers;
using AHM.WebApi.Common.Responses;
using Core.Application.Features.Innovation.Commands;
using Core.Application.Features.Innovation.Queries;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InnovationController : AhmBaseController<InnovationController>
    {


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("GetStatusIp")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<List<AhmhrirsTxnip>>> GetStatusIp(GetStatusIpByQuery request, CancellationToken cancellationToken)
        {
            // var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            var response = await Mediator!.Send(new GetStatusIpByQuery() { vnrp = UserIdentity.Nrp, vstatus =request.vstatus }, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<List<AhmhrirsTxnip>> ()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("GetDetIp")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<GetDetIPResponseViewModel>> GetDetIp(GetDetIPDataByQuery request, CancellationToken cancellationToken)
        {
            // var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            var response = await Mediator!.Send(new GetDetIPDataByQuery() { vnrp = UserIdentity.Nrp, vnoreg = request.vnoreg, domain = request.domain ,username = UserIdentity.UserName,zone=UserIdentity.Zone}, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<GetDetIPResponseViewModel>()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("UploadAPI")]
        [AhmFuncAuthorize("UPLOAD")]
        public async Task<BaseResponse<UploadAPIResponseViewModel>> UploadAPI(UploadAPIDataCommand request, CancellationToken cancellationToken)
        {
            //var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            var response = await Mediator!.Send(new UploadAPIDataCommand() { vfile = request.vfile, domain = request.domain, username = UserIdentity.UserName, zone = UserIdentity.Zone , vname = request.vname,vflag=request.vflag,vattachNameId=request.vattachNameId,vnote =request.vnote}, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<UploadAPIResponseViewModel>()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("InsertDataMobile")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<string>> InsertDataMobile(InsertDataMobileCommand request, CancellationToken cancellationToken)
        {
            request.nrp = UserIdentity.Nrp;
            request.displayName = UserIdentity.DisplayName;
            request.userName = UserIdentity.UserName;
            request.Email = UserIdentity.Email;
            request.Zone = UserIdentity.Zone;
            
            var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
           
            if (response == "Success")
            {
                return new BaseResponse<string>()
                {
                    Data = response,
                    Message = string.Empty,
                    Status = 1
                };
            }
            else
            {
                return new BaseResponse<string>()
                {
                    Data = response,
                    Message = string.Empty,
                    Status = 0
                };
            }

           
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("GetPeriodId")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<string>> GetPeriodId(GetPeriodIdDataByQuery request, CancellationToken cancellationToken)
        {
            var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<string>()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("GetTotalGrade")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<AHMHRIRS021_IPTOTALGRADE>> GetTotalGrade(GetTotalGradeDataByQuery request, CancellationToken cancellationToken)
        {
            var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<AHMHRIRS021_IPTOTALGRADE>()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("GetHistoryWorkflow")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<List<HistoryTable>>> GetHistoryWorkflow(GetHistoryWorkflowDataByQuery request, CancellationToken cancellationToken)
        {
            var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<List<HistoryTable>>()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("GetGuid")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<GetGuidResponse>> GetGuid(GetGuidDataByQuery request, CancellationToken cancellationToken)
        {
            var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<GetGuidResponse>()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("GetGolonganUserData")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<GetGolonganUserViewModel>> GetGolonganUserData(GetGolonganUserDataByQuery request, CancellationToken cancellationToken)
        {
            request.vnrp = UserIdentity.Nrp;
            var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<GetGolonganUserViewModel>()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }


        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("GetInfoSec")]
        [AhmFuncAuthorize("OPEN")]
        public async Task<BaseResponse<AHMHRIRS021_Info>> GetInfoSec(GetInfoSecDataByQuery request, CancellationToken cancellationToken)
        {
            request.username = UserIdentity.UserName;
            request.nrp = UserIdentity.Nrp;
              
            var response = await Mediator!.Send(request, cancellationToken).ConfigureAwait(false);
            return new BaseResponse<AHMHRIRS021_Info>()
            {
                Data = response,
                Message = string.Empty,
                Status = 1
            };
        }

    }
}




