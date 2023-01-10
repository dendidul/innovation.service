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
    public class GetInfoSecDataByQuery : IRequest<AHMHRIRS021_Info>
    {
        public string username { get; set; }
        public string nrp { get; set; }

        public class GetInfoSecDataByQueryHandler : IRequestHandler<GetInfoSecDataByQuery, AHMHRIRS021_Info>
        {
            private readonly IHRTxnInnovationContext _hrtxncontext;
            private readonly Iitb2eInnovationContext _itb2econtext;
            private readonly IHrirsInnovationContext _hrirsContext;

            public GetInfoSecDataByQueryHandler(IHRTxnInnovationContext hrtxncontext, Iitb2eInnovationContext itb2econtext, IHrirsInnovationContext hrirsContext)
            {
                _hrtxncontext = hrtxncontext;
                _itb2econtext = itb2econtext;
                _hrirsContext = hrirsContext;
            }



            public async Task<AHMHRIRS021_Info> Handle(GetInfoSecDataByQuery request, CancellationToken cancellationToken)
            {
                AHMHRIRS021_Info result = new AHMHRIRS021_Info();

                int usr = 0;
                var roles = await _itb2econtext.GetUserRoles(request.username, cancellationToken);
                int golongan = 0;
                var getGolongankaryawan = await _hrtxncontext.getGolonganKaryawan(request.nrp, cancellationToken);

                var getPeriodeAktif = _hrirsContext.GetPeriodAktif(cancellationToken);

                List<string> setting = await _hrirsContext.getSettingValue("SETTING_GOL_IP", cancellationToken);
                if (setting.Count > 0)
                {
                    golongan = int.Parse(setting[0]);
                }

                if ((Convert.ToDouble(getGolongankaryawan) < golongan && !roles.Contains("OU-HEAD") && !roles.Contains("AHMIC_AREAPIC") && !roles.Contains("AHMIC") &&
                  getPeriodeAktif.Equals("")))
                {
                    usr = 1;
                }

                result.infoSect = await _hrtxncontext.GetInfoSectionByNrp(request.nrp, usr,long.Parse(request.nrp), cancellationToken); ;

                return result;
            }


        }

    }
}
