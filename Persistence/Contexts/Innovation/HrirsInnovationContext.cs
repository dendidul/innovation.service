using AHM.Domain.Ahmhrirs.Entities;
using AHM.Domain.Ahmhrirs.Entities.Innovation;
using AHM.Persistence.Ahmhrirs.Configurations.Innovation;
using Core.Application.Interfaces.Innovation;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts.Innovation
{
    public class HrirsInnovationContext : DbContext, IHrirsInnovationContext
    {
        public HrirsInnovationContext(DbContextOptions<HrirsInnovationContext> options) : base(options)
        {

        }

        public HrirsInnovationContext()
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //  modelBuilder.ApplyConfiguration(new AhmhrirsGetWFIDByNodocViewModelConfiguration());
            modelBuilder.Entity<AhmhrirsTxnip>().HasNoKey();
            modelBuilder.Entity<AhmhrirsDtlemp>().HasNoKey();
            modelBuilder.Entity<AhmhrirsDtlip>().HasNoKey();
            modelBuilder.Entity<AhmhrirsHdrperiod>().HasNoKey();
            modelBuilder.Entity<AhmhrirsAreapic>().HasNoKey();
            modelBuilder.Entity<AHMHRIRS021_LISTIP>().HasNoKey();
            modelBuilder.Entity<AHMHRIRS_HDRFORMSETS>().HasNoKey();
            modelBuilder.Entity<AhmhrirsHdrsetting>().HasNoKey();
            modelBuilder.Entity<AHMHRIRS021_AREA>().HasNoKey();
            modelBuilder.Entity<AhmhrirsDtlformset>().HasNoKey();
            //modelBuilder.Ignore<ICollection<AhmhrirsAreapic>>();
            modelBuilder.Entity<GetWFIDByNodocData>().HasNoKey();


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AhmhrirsTxnip> AhmhrirsTxnipDb => Set<AhmhrirsTxnip>();

        public DbSet<AhmhrirsDtlemp> AhmhrirsDtlempDb => Set<AhmhrirsDtlemp>();



        public virtual DbSet<AhmhrirsDtlip> AhmhrirsDtlipDb { get; set; } = null!;

        public virtual DbSet<AhmhrirsHdrperiod> AhmhrirsHdrperiodDb { get; set; } = null!;

        public virtual DbSet<AhmhrirsAreapic> AhmhrirsAreapicDb { get; set; } = null!;

        public virtual DbSet<AHMHRIRS021_LISTIP> AHMHRIRS021_LISTIPDb { get; set; } = null!;
        public virtual DbSet<AHMHRIRS_HDRFORMSETS> AHMHRIRS_HDRFORMSETSDb { get; set; } = null!;

        public virtual DbSet<AhmhrirsHdrsetting> AhmhrirsHdrsettingDb { get; set; } = null!;

        public virtual DbSet<AHMHRIRS021_AREA> AHMHRIRS021_AREADb { get; set; } = null!;

        public virtual DbSet<AhmhrirsDtlformset> AhmhrirsDtlformsetDb { get; set; } = null!;
          public virtual DbSet<GetWFIDByNodocData> GetWFIDByNodocViewModelDb { get; set; } = null!;



      














        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=t13438.astra-honda.com)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=devhrtxn)));User Id=MSY_B2E_DV;Password=newvario160;");
            }
        }

        public async Task<string> GetWFIDByNodoc(string nodoc, CancellationToken cancellationToken)
        {
            var getData = await GetWFIDByNodocViewModelDb.FromSqlInterpolated<GetWFIDByNodocData>
                 ($@"select VWFGUID from AHMHRIRS_TXNIPS where VNODOC = {nodoc}").Select(x => new GetWFIDByNodocData
                 {
                     VWFGUID = x.VWFGUID
                 }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            return getData != null ? getData.VWFGUID : "";

        }


        public async Task<List<AhmhrirsTxnip>> GetStatusIp( string v_vnrp, string v_vstatus, CancellationToken cancellationToken)
        {

            // var result = (from i in _context.AhmhrirsTxnips where i.Vnrp == v_vnrp && i.Vstatus == Convert.ToInt16(v_vstatus) orderby i.Dcrea descending select i).ToList();

            var result = await AhmhrirsTxnipDb.Where(x => x.Vnrp == v_vnrp && x.Vstatus == Convert.ToInt16(v_vstatus)).OrderByDescending(x => x.Dcrea).ToListAsync(cancellationToken).ConfigureAwait(false);

            return result;

        }


        public async Task<AhmhrirsDtlformset> GetGolonganSets(CancellationToken cancellationToken)
        {
            var getData = await AhmhrirsDtlformsetDb.FromSqlInterpolated<AhmhrirsDtlformset>
                          ($@"
                            select DTL.NVALUE1, DTL.NVALUE2 from
				AHMHRIRS_HDRFORMSETS HDR,
				AHMHRIRS_DTLFORMSETS DTL
				where
				HDR.MHRTEMA_VTEMATYPE = DTL.RHRFORMS_VTEMATYPE
				and HDR.VTYPE = 'HDRF001'
				and getdate() between HDR.DBEGINEFF and HDR.DENDEFF
				and DTL.VTYPEDTLFORM = 'DTLF002'
				and upper(DTL.VVALUE1) = upper('Creator')
                            ").Select(x=> new AhmhrirsDtlformset
                          {
                                  Nvalue1 = x.Nvalue1,
                                  Nvalue2 = x.Nvalue2,
                          }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            return getData;

        }


            public async Task<string> getPhone(string vnrp, string vnoreg, CancellationToken cancellationToken)
        {
            string phone = "";
            var result = await AhmhrirsDtlempDb.Where(x => x.Vnodoc == vnoreg).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);


            return result != null ? result.Vphone : ""; ;

        }

        public async Task<AhmhrirsTxnip> getDataIP(string vnoreg, CancellationToken cancellationToken)
        {
            return await AhmhrirsTxnipDb.Where(x => x.Vnrp == vnoreg).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        }


        public async Task<List<string>> getSettingValue(string key, CancellationToken cancellationToken)
        {
            List<string> str = new List<string>();
            var i = (from d in AhmhrirsHdrsettingDb
                     where d.Vid == key
                     select d
                     ).FirstOrDefault();

            if (i != null)
            {
                if (!string.IsNullOrEmpty(i.Vval1)) str.Add(i.Vval1);
                if (!string.IsNullOrEmpty(i.Vval2)) str.Add(i.Vval2);
            }
            return str;

        }




        public async Task<string> GetUploadedFilePath(string encrypted, string username, string domain, string zone, string fileId, string path, CancellationToken cancellationToken)
        {
            string userFolder = zone + "_" + encrypted;
            path = Path.Combine(path, userFolder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, fileId);
            if (File.Exists(path)) return path;
            else return string.Empty;

        }


        public async Task<List<string>> GetArea(string varea, CancellationToken cancellationToken)
        {
            var picarea = await (from a in AhmhrirsAreapicDb
                                 where a.Vareaid == varea &&
                                  a.Istatus == 1
                                 select a.Vuserid).Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            return picarea;
        }

        public async Task<List<AHMHRIRS021_LISTIP>> GetBeforeAfter(string pnoreg, string pnrp, CancellationToken cancellationToken)
        {
            var getData = await AHMHRIRS021_LISTIPDb.FromSqlInterpolated<AHMHRIRS021_LISTIP>
                          ($@"
                            SELECT D.XIP_VNODOC, D.NSEQ, D.NBEFAFT, D.VCOND, H.VNRP,D.VFILE
                            FROM AHMHRIRS_DTLIPS D, AHMHRIRS_TXNIPS H
                            WHERE D.XIP_VNODOC = H.VNODOC
                                AND D.XIP_NID = H.NID
                                AND XIP_VNODOC = {pnoreg}
                            ").Select(x => new AHMHRIRS021_LISTIP()
                          {
                              XIP_VNODOC = x.XIP_VNODOC,
                              NSEQ = x.NSEQ,
                              NBEFAFT = x.NBEFAFT,
                              VCOND = x.VCOND,
                              VNRP = x.VNRP,
                              VFILE = x.VFILE,

                          }).ToListAsync(cancellationToken).ConfigureAwait(false);

            return getData;
        }

        public async Task<string> GetAttachmentId(string vname, string vnodoc, CancellationToken cancellationToken)
        {
            string vpath = "";
            var getData = await AhmhrirsDtlipDb.FromSqlInterpolated<AhmhrirsDtlip>
                            ($@"
                                SELECT D.VPATH
                            FROM AHMHRIRS_DTLIPS D
                            WHERE D.XIP_VNODOC = {vnodoc}
                            AND D.VFILE = {vname}
                            ").Select(x => new AhmhrirsDtlip()
                            {
                                Vpath = x.Vpath
                            }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (getData != null)
            {
                vpath = getData.Vpath;
            }
            return vpath;

        }

        public async Task<string> GetThemeType(CancellationToken cancellationToken)
        {
            string vthemetype = "";
            var getData = await AHMHRIRS_HDRFORMSETSDb.FromSqlInterpolated<AHMHRIRS_HDRFORMSETS>
                          ($@"
                            sELECT MHRTEMA_VTEMATYPE 
				        FROM AHMHRIRS_HDRFORMSETS
				        WHERE GETDATE() BETWEEN DBEGINEFF AND DENDEFF
				        AND VTYPE = 'HDRF001'
                            ").Select(x => new AHMHRIRS_HDRFORMSETS()
                          {
                              MHRTEMA_VTEMATYPE = x.MHRTEMA_VTEMATYPE
                          }).FirstOrDefaultAsync().ConfigureAwait(false);

            if (getData != null)
            {
                vthemetype = getData.MHRTEMA_VTEMATYPE;
            }
            return vthemetype;

        }


        public async Task<AHMHRIRS021_AREA> GetAreaIDByNrp(Int64 _inrp, string VVAL1, CancellationToken cancellationToken)
        {

            var getData = await AHMHRIRS021_AREADb.FromSqlInterpolated<AHMHRIRS021_AREA>
                        ($@"
                            SELECT AREA.VAREAID, AREA.VAREADESC
                        FROM
                           AHMHRIRS_HDRSETTINGS A,
                           AHMHRIRS_HDRAREAS AREA
                        WHERE
                           A.VVAL2 = AREA.VAREAID
                           AND A.VID LIKE 'AREA_%'
                           AND A.VVAL1 = {VVAL1} ")
                          .Select(x => new AHMHRIRS021_AREA()
                          {
                              VAREAID = x.VAREAID,
                              VAREADESC = x.VAREADESC


                          }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            return getData;
        }


        public async Task<string> GetPeriodID(CancellationToken cancellationToken)
        {
            string PeriodId = "";
            var getData = await AhmhrirsHdrperiodDb.Where(x => x.Istatus == 1).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);



            return getData != null ? getData.Vperiodid : "";

        }

        public string GetPeriodAktif(CancellationToken cancellationToken)
        {
            string vperaktif = "-";
            vperaktif = AhmhrirsHdrperiodDb.Where(x => x.Dstart >= DateTime.Now && x.Dend <= DateTime.Now).Count().ToString();
            return vperaktif == "0" ? "" : "1";

        }

       

        public decimal GetNid(string noreg)
        {
            decimal inid = 0;

            string[] splitnoreg = noreg.Split('/');

            string vnid = splitnoreg[0] + splitnoreg[3] + splitnoreg[4];

            // Logger.WriteLog("GetNID vnid : " + vnid);

            decimal.TryParse(vnid, out inid);

            return inid;
        }


        public async Task<DateTime> GetDateSubmitByVwfguid(string VWFGUID, CancellationToken cancellationToken)
        {
            DateTime? DSUBMIT = null;
            if (string.IsNullOrEmpty(VWFGUID))
                VWFGUID = "-";

            var getData = await AhmhrirsTxnipDb.FromSqlInterpolated<AhmhrirsTxnip>
                          ($@"").Select(x => new AhmhrirsTxnip
                          {
                              Dsubmitted = x.Dsubmitted
                          }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (getData != null)
            {
                DSUBMIT = getData.Dsubmitted;
            }

            return DSUBMIT.Value;



        }


        public async Task<decimal> GetNseq(decimal nid, string period, Int32 beaf, CancellationToken cancellationToken)
        {


            decimal nseq = 0;

            var getData = await AhmhrirsDtlipDb.FromSqlInterpolated<AhmhrirsDtlip>
                          ($@" SELECT NSEQ
                            FROM AHMHRIRS_DTLIPS
                            WHERE XIP_NID = {nid}
                            AND XIP_VPERIODID = {period}
                            AND NBEFAFT = {beaf}
                            ORDER BY NSEQ DESC").
                          FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (getData != null)
            {
                nseq = +getData.Nseq + 1;
            }

            return (nseq + 1);
        }


        public async void UpdateAttachmentId(AHMHRIRS021_IPDATA dataip, List<AHMHRIRS021_Attachment> detail, bool isSend,CancellationToken cancellationToken)
        {
            int i = 0;

            foreach (AHMHRIRS021_Attachment dtl in detail)
            {
                i++;
                int NBEFAFT = 0;
                if (dtl.VTYPE != "std")
                {
                    NBEFAFT =  dtl.VTYPE.Equals("bf") ? 1 : 2;
                }
                else
                {
                    NBEFAFT = 3;
                }

                await this.Database.ExecuteSqlInterpolatedAsync($@"
                   UPDATE AHMHRIRS_DTLIPS
                   SET VPATH = {dtl.VPATH}
                   WHERE XIP_VNODOC = {dataip.VNODOC}
                   AND NSEQ = {dtl.VSEQ}
                   AND NBEFAFT = {NBEFAFT}
                   AND VFILE = { dtl.VFILE}
                ").ConfigureAwait(false);

            }

        }


        public async void UpdateDataHeaderDetail(string lastTaskId, AHMHRIRS021_IPDATA dataip, List<AHMHRIRS021_Attachment> detail, bool isSend, string wfguid, Int64 _inrp, CancellationToken cancellationToken)
        {
            List<AHMHRIRS021_LISTIP> listdata = new List<AHMHRIRS021_LISTIP>();

            //DSUBMITED only se first kirim ke atasan
            var DSUBMITED = await GetDateSubmitByVwfguid(wfguid, cancellationToken);

            if (DSUBMITED == DateTime.MinValue)
            {
                DSUBMITED = DateTime.Now;
            }

            await this.Database.ExecuteSqlInterpolatedAsync(
                    $@"
                    UPDATE AHMHRIRS_TXNIPS
                    SET  VNRP = {dataip.VNRP}
                        , VTEMAIP = {dataip.VTEMAIP}
                        , VPROCESS = {dataip.VPROCESS}
                        , VIMPACT = {dataip.VIMPACT}
                        , VQUALITY = {dataip.VQUALITY }
                        , VCOST = {dataip.VCOST}
                        , VDELIVERY = {dataip.VDELIVERY}
                        , VSAFETY = {dataip.VSAFETY}
                        , VMORAL = {dataip.VMORAL}
                        , VSTANDARD = {dataip.VSTANDARD}
                        , VMODI = {_inrp}
                        , DMODI = {DateTime.Now}
                        ,DSUBMITTED= {DSUBMITED}
                    WHERE VNODOC = {dataip.VTEMAIP}
                    "
                   ).ConfigureAwait(false);

            string periodid = await GetPeriodID(cancellationToken);

            int i = 0;
            foreach (AHMHRIRS021_Attachment dtl in detail)
            {
                i++;
                int NBEFAFT = 0;
                decimal NSEQ = 0;

                if (dtl.VFLAG.Equals("UP"))
                {

                    if (dtl.VTYPE != "std")
                    {
                        NBEFAFT = dtl.VTYPE.Equals("bf") ? 1 : 2;
                    }
                    else
                    {
                        NBEFAFT = 3;
                    }

                    await this.Database.ExecuteSqlInterpolatedAsync($@"
                        UPDATE AHMHRIRS_DTLIPS
                        SET VCOND = {dtl.VNOTE}, 
                        VMODI = {_inrp}, 
                        DMODI = {DateTime.Now},
                        VFILE = { dtl.VFILE}
                        WHERE XIP_VNODOC = {dataip.VNODOC}
                        AND NSEQ = {dtl.VSEQ}
                        AND NBEFAFT = {NBEFAFT}
                    ").ConfigureAwait(false);
                }
                else if (dtl.VFLAG.Equals("DEL"))
                {
                    if (dtl.VTYPE != "std")
                    {
                        NBEFAFT = dtl.VTYPE.Equals("bf") ? 1 : 2;
                    }
                    else
                    {
                        NBEFAFT = 3;
                    }

                    await this.Database.ExecuteSqlInterpolatedAsync($@"
                       DELETE FROM AHMHRIRS_DTLIPS
                        WHERE XIP_VNODOC = {dataip.VNODOC}
                            AND NSEQ = {dtl.VSEQ}
                            AND NBEFAFT = @NBEFAFT 
                    ").ConfigureAwait(false);
                }
                else if (dtl.VFLAG.Equals("IN"))
                {
                    if (dtl.VTYPE != "std")
                    {
                        NBEFAFT = dtl.VTYPE.Equals("bf") ? 1 : 2;
                        NSEQ = await GetNseq(dataip.NID, periodid, NBEFAFT, cancellationToken);

                        //comm.Parameters.AddWithValue("NSEQ", GetNseq(dataip.NID, periodid, dtl.VTYPE.Equals("bf") ? 1 : 2).ToString());
                        //comm.Parameters.AddWithValue("NBEFAFT", dtl.VTYPE.Equals("bf") ? 1 : 2);
                    }
                    else
                    {
                        //comm.Parameters.AddWithValue("NSEQ", GetNseq(dataip.NID, periodid, dtl.VTYPE.Equals("std") ? 3 : 0).ToString());
                        //comm.Parameters.AddWithValue("NBEFAFT", 3);

                        NBEFAFT  =3;
                        NSEQ = await GetNseq(dataip.NID, periodid, NBEFAFT, cancellationToken);
                    }


                    await this.Database.ExecuteSqlInterpolatedAsync($@"
                      INSERT INTO AHMHRIRS_DTLIPS
                        (XIP_NID, XIP_VPERIODID, XIP_VNODOC, NSEQ ,NBEFAFT ,VCOND ,VCREA ,DCREA, VPATH, VFILE)
                        VALUES (@XIP_NID, 
                                {periodid}, 
                                {dataip.VNODOC},
                                {NSEQ},
                                {NBEFAFT},
                                {dtl.VNOTE},
                                {_inrp},
                                {DateTime.Now}, 
                                '', 
                                {dtl.VFILE}) 
                    ").ConfigureAwait(false);
                }






            }

            await this.Database.ExecuteSqlRawAsync
                (
                    $@"UPDATE AHMHRIRS_DTLEMPS
                    SET VPHONE = {dataip.VPHONE},
                        VMODI = {_inrp},
                        DMODI = {DateTime.Now},
                        NGOLONGAN = {dataip.NGOLONGAN}
                    WHERE VNODOC = {dataip.VNODOC}"
                ).ConfigureAwait(false);

            if (isSend)
            {
                if (lastTaskId == "task2")
                    dataip.VSTATUS = "3";
                else
                    dataip.VSTATUS = "2";

                await this.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE AHMHRIRS_TXNIPS
                    SET VSTATUS = {dataip.VSTATUS}, VWFGUID = {wfguid}, VMODI = {_inrp}, DMODI = { DateTime.Now}
                    WHERE VNODOC = {dataip.VNODOC}
                ").ConfigureAwait(false);
            }

            await this.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE AHMHRIRS_DTLIPS SET VFILE='' WHERE VFILE='-'
                ").ConfigureAwait(false);


        }

        public string GetDownloadLinkIp(string userFolder,string path, string domain, string username, string zone, string fileName,CancellationToken cancellationToken)
        { //nitip update

//            string userFolder = zone + "_" + _encryptionHelper.MD5Hash(domain + "\\" + username);
//,ca
            //string path =await GetPathAttachment("1",cancellationToken);
            // path = Path.Combine(path, userFolder);

            string fullPath = Path.Combine(path, fileName);

            return "handlers/file.ashx?FilePathIp=" + fullPath + "&urlparam=FilePathIp";
        }


        public async Task<List<AhmhrirsTxnip>> GetTotalGrade (string vnrp,string periodId,CancellationToken cancellationToken)
        {
            var getdata = (from i in AhmhrirsTxnipDb
                           where i.Vnrp == vnrp && i.Vperiodid == periodId
                           select i).ToList();
            return getdata;
        }



        public async void InserDataHeaderDetail(string employeeStatus,AHMHRIRS021_IPDATA dataip, List<AHMHRIRS021_Attachment> detail, bool isSend, string wfguid, Int64 _inrp, CancellationToken cancellationToken)
        {
            List<AHMHRIRS021_LISTIP> listdata = new List<AHMHRIRS021_LISTIP>();
            try
            {
                string periodid = await GetPeriodID(cancellationToken);
                decimal nid = (GetNid(dataip.VNODOC) + 1);

                await this.Database.ExecuteSqlInterpolatedAsync($@"
                   INSERT INTO AHMHRIRS_TXNIPS
                (NID, VPERIODID, VNRP, VTEMAIP, DSUBMITTED, VNODOC, VSTATUS, VPROCESS, VIMPACT, VQUALITY, VCOST, VDELIVERY, VSAFETY, VMORAL, VSTANDARD, VCREA, DCREA)
                VALUES ({nid}, {periodid} ,{dataip.VNRP} ,{dataip.VTEMAIP} ,{DateTime.Now} , {dataip.VNODOC} ,1 ,{dataip.VPROCESS} ,{dataip.VIMPACT} ,{dataip.VQUALITY} ,{dataip.VCOST} , {dataip.VDELIVERY} , {dataip.VSAFETY} , {dataip.VMORAL} , {dataip.VSTANDARD} , {dataip.VNRP} ,{DateTime.Now})
                ").ConfigureAwait(false);

                foreach (AHMHRIRS021_Attachment dtl in detail)
                {
                    int NBEFAFT = 0;
                    decimal NSEQ = 0;

                    if (dtl.VFLAG != "DEL")
                    {
                        if (dtl.VTYPE != "std")
                        {
                            NBEFAFT = dtl.VTYPE.Equals("bf") ? 1 : 2;
                            NSEQ = await GetNseq(dataip.NID, periodid, NBEFAFT, cancellationToken);

                            //comm.Parameters.AddWithValue("NSEQ", GetNseq(dataip.NID, periodid, dtl.VTYPE.Equals("bf") ? 1 : 2).ToString());
                            //comm.Parameters.AddWithValue("NBEFAFT", dtl.VTYPE.Equals("bf") ? 1 : 2);
                        }
                        else
                        {
                            //comm.Parameters.AddWithValue("NSEQ", GetNseq(dataip.NID, periodid, dtl.VTYPE.Equals("std") ? 3 : 0).ToString());
                            //comm.Parameters.AddWithValue("NBEFAFT", 3);

                            NBEFAFT = 3;
                            NSEQ = await GetNseq(dataip.NID, periodid, NBEFAFT, cancellationToken);
                        }

                        await this.Database.ExecuteSqlInterpolatedAsync($@"
                            INSERT INTO AHMHRIRS_DTLIPS
                            (XIP_NID, XIP_VPERIODID, XIP_VNODOC, NSEQ ,NBEFAFT ,VCOND,VCREA ,DCREA, VPATH, VFILE)
                            VALUES ({nid}, {periodid} , {dataip.VDOCNO} , {NSEQ} , {NBEFAFT} , {dtl.VNOTE}, {_inrp}, {DateTime.Now}, '', {dtl.VFILE})
                        ").ConfigureAwait(false);


                    }



                }


                await this.Database.ExecuteSqlInterpolatedAsync($@"
                            INSERT INTO AHMHRIRS_DTLEMPS
                (VNODOC, VNRP, VNAMA, NAREA, VDIREKTORAT, VDIVISI, VDEPARTMENT, VSUBDEPT, VSECTION, VPHONE, VACTIVE, VCREA, DCREA, VWORKCONTRACT, NGOLONGAN)
                VALUES ({dataip.VNODOC}, {dataip.VNRP}, {dataip.VNAME}, {dataip.NAREA}, {dataip.VDIREKTORAT}, {dataip.VDIVISI}, {dataip.VDEPARTMENT}, {dataip.VSUBDEPT}, {dataip.VSECTION}, {dataip.VPHONE}, 1, {_inrp}, {DateTime.Now},{employeeStatus}, {dataip.NGOLONGAN})
                ").ConfigureAwait(false);

                if (isSend)
                {
                    await this.Database.ExecuteSqlInterpolatedAsync($@"
                            UPDATE AHMHRIRS_TXNIPS
                    SET VSTATUS = '2', VWFGUID = {wfguid}, VMODI = {_inrp}, DMODI = {DateTime.Now}
                    WHERE VNODOC = {dataip.VNODOC}
                ").ConfigureAwait(false);
                }



                }
            catch (Exception ex)
            {


            }
        }


    }
}
