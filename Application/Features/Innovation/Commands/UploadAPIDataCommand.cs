using AHM.Common.Helper.Config;
using AHM.Common.Helper.Encryption;
using AHM.Domain.Ahmhrirs.Entities.Innovation;
using Core.Application.Interfaces.Innovation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Features.Innovation.Commands
{
    public class UploadAPIDataCommand : IRequest<UploadAPIResponseViewModel>
    {
        public string vfile { get; set; }
        public string vname { get; set; }
        public string vflag { get; set; }
        public string vattachNameId { get; set; }
        public string vnote { get; set; }
        public string vtype { get; set; }

        public string zone { get; set; }

        public string username { get; set; }
        public string domain { get; set; }

        public class UploadAPIDataCommandHandler : IRequestHandler<UploadAPIDataCommand, UploadAPIResponseViewModel>
        {
            private readonly IHRTxnInnovationContext _context;

            private readonly IHrirsInnovationContext _hrirscontext;
            private readonly IEncryptionHelper _encryptionHelper;
            private IConfigCreator _configCreator;

            public UploadAPIDataCommandHandler(IConfigCreator configCreator, IEncryptionHelper encryptionHelper, IHRTxnInnovationContext context, IHrirsInnovationContext hrirscontext)
            {
                _context = context;
                _hrirscontext = hrirscontext; ;
                _encryptionHelper = encryptionHelper;
                _configCreator = configCreator;
            }


            public async Task<UploadAPIResponseViewModel> Handle(UploadAPIDataCommand request, CancellationToken cancellationToken)
            {
                UploadAPIResponseViewModel result = new UploadAPIResponseViewModel();

                string vfile = request.vfile ?? "";
                string vname = request.vname ?? "";
                string vflag = request.vflag ?? "";
                string vattachNameId = request.vattachNameId ?? "";
                string vnote = request.vnote ?? "";
                string vtype = request.vtype ?? "";
                string fn = "";

                string userFolder = request.zone + "_" + _encryptionHelper.MD5Hash(request.domain + "\\" + request.username);
                string path = _configCreator.Get("_CONFIG_UPLOAD_PATH");
                path = Path.Combine(path, userFolder);

                fn = Guid.NewGuid().ToString() + "_" + vname;

                if (vfile != null && vname != null)
                {
                    string filePath = Path.Combine(path, fn);
                    byte[] fileBytes = Convert.FromBase64String(vfile);

                    string pathFolder = await _context.GetPathAttachment("1", cancellationToken);

                    if (vflag.Equals("IN"))
                    {
                        System.IO.File.WriteAllBytes(filePath, fileBytes);

                        result.VNOTE = vnote;
                        result.VATTCHNAME = vname;
                        result.VATTCHNAMEONLY = vname;
                        result.VATTCHNAMEID = fn;
                        //result.VSEQ = 
                        result.VFLAG = vflag;
                        result.VTYPE = vtype;
                        // result.VPATH = 
                        result.VFILE = vname;
                        return result;
                    }
                    else if(vflag.Equals("DEL"))
                    {
                        File.Delete(Path.Combine(path, vattachNameId));
                        return result;
                    }
                    else
                    {
                        return result;
                    }

                }
                else
                {
                    return result;
                }

              

            }

        }

    }
}
