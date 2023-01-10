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
    public class GetGolonganUserDataByQuery : IRequest<GetGolonganUserViewModel>
    {
        public string vnrp { get; set; }
        public class GetGolonganUserDataByQueryHandler : IRequestHandler<GetGolonganUserDataByQuery, GetGolonganUserViewModel>
        {
            private readonly IHRTxnInnovationContext _hrtxnContext;

            private readonly IHrirsInnovationContext _hrirsContext;

            public GetGolonganUserDataByQueryHandler(IHRTxnInnovationContext hrtxnContext, IHrirsInnovationContext hrirsContext)
            {
                _hrtxnContext = hrtxnContext;
                _hrirsContext = hrirsContext;
            }

            public async Task<GetGolonganUserViewModel> Handle(GetGolonganUserDataByQuery request, CancellationToken cancellationToken)
            {
                int settingGolongan = 0;

                GetGolonganUserViewModel result = new GetGolonganUserViewModel();

                string vgolongan = await _hrtxnContext.getGolonganKaryawan(request.vnrp, cancellationToken);
                string vperiodeAktif = _hrirsContext.GetPeriodAktif(cancellationToken);
                string vinputPermission = "0";

                List<string> setting = await _hrirsContext.getSettingValue("SETTING_GOL_IP", cancellationToken);

                if (setting.Count > 0 && setting[0] != "")
                {
                    settingGolongan = int.Parse(setting[0]);
                }

                var getGolset = await _hrirsContext.GetGolonganSets(cancellationToken);
                var valueGolsetnv1 = getGolset != null ? getGolset.Nvalue1 : 0;
                var valueGolsetnv2 = getGolset != null ? getGolset.Nvalue2 : 0;

                if (Convert.ToDouble(vgolongan) >= Convert.ToInt32(valueGolsetnv1) && Convert.ToDouble(vgolongan) <= Convert.ToInt32(valueGolsetnv2) && vperiodeAktif != "")
                {
                    vinputPermission = "1";
                }

                if (vgolongan != "")
                {
                    result.VNRP = request.vnrp;
                    result.VGOLONGAN = vgolongan;
                    result.VPERIODEAKTIF = vperiodeAktif;
                    result.settingGolongan = settingGolongan.ToString();
                    result.VINPUTPERMISSION = vinputPermission;

                }


                return result;


            }
        }

    }
}
