using AHM.Common.Helper.Encryption;
using AHM.Domain.Ahmhrirs.Entities;
using AHM.Domain.Ahmhrirs.Entities.Innovation;
using AHM.Domain.Ahmhrmtn.Entities;
using AHM.Domain.Ahmhrntm.Entities;

using Core.Application.Interfaces.Innovation;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts.Innovation
{
    public class HRTxnInnovationContext : DbContext, IHRTxnInnovationContext
    {
       
        public HRTxnInnovationContext(DbContextOptions<HRTxnInnovationContext> options) : base(options)
        {

        }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new AhmhrtmoMstholidaysConfiguration());

            modelBuilder.Entity<GetGolonganKaryawanViewModel>().HasNoKey();
            modelBuilder.Entity<AHMHRD_REC00_SETTINGS>().HasNoKey();
            modelBuilder.Entity<AHMHRNTM_MSTOUHEADS>().HasNoKey();
            modelBuilder.Entity<AHMHRIRS021_InfoSect>().HasNoKey();
            modelBuilder.Entity<FMHRD_EMPLOYMENTS>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<AhmhrirsTxnip> AhmhrirsTxnipDb => Set<AhmhrirsTxnip>();
        //public DbSet<AhmhrirsDtlemp> AhmhrirsDtlempDb => Set<AhmhrirsDtlemp>();
        public DbSet<GetGolonganKaryawanViewModel> GetGolonganKaryawanViewModelDb => Set<GetGolonganKaryawanViewModel>();

        public DbSet<AHMHRD_REC00_SETTINGS> AHMHRD_REC00_SETTINGSDb => Set<AHMHRD_REC00_SETTINGS>();

        public DbSet<AHMHRNTM_MSTOUHEADS> AHMHRNTM_MSTOUHEADSDb => Set<AHMHRNTM_MSTOUHEADS>();

        public DbSet<AHMHRIRS021_InfoSect> AHMHRIRS021_InfoSectDb => Set<AHMHRIRS021_InfoSect>();

        public DbSet<FMHRD_EMPLOYMENTS> FMHRD_EMPLOYMENTSDb => Set<FMHRD_EMPLOYMENTS>();

        




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=t13438.astra-honda.com)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=devhrtxn)));User Id=MSY_B2E_DV;Password=newvario160;");
            }
        }


        public async Task<string> GetForbiddenCharacter(CancellationToken cancellationToken)
        {
            string resultAtt = "";

            var getData = await AHMHRD_REC00_SETTINGSDb.FromSqlInterpolated<AHMHRD_REC00_SETTINGS>
                          ($@"SELECT VALUE1
                            FROM AHMHRD_REC00_SETTINGS
                            WHERE ID   = 'AHM'
                            AND PARAM1 = 'IRS021'
                            AND PARAM2 = 'ATTACH'
                            AND PARAM3 = 'TRIM'").Select(x=> new AHMHRD_REC00_SETTINGS { VALUE1= x.VALUE1 }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            if(getData != null)
            {
                resultAtt = getData.VALUE1;
            }

            return resultAtt;
        }


        public async Task<string> getGolonganKaryawan(string NRP, CancellationToken cancellationToken)
        {
            string grade = "";
            var get = await GetGolonganKaryawanViewModelDb.
               FromSqlInterpolated<GetGolonganKaryawanViewModel>
               ($@"
                       select SUBSTR(GRADE,1,1) GRADE from FMHRD_EMPLOYMENTS where GENDAT_NRP = {NRP} and GENDAT_VEND_VND_CODE = 'AHM'
                ").Select(x => new GetGolonganKaryawanViewModel()
               {
                   GRADE = x.GRADE
               }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (get != null)
            {
                grade = get.GRADE;
            }

            return grade;

        }


        public async Task<int> GetRunNo(Int64 _inrp,CancellationToken cancellationToken)
        {
            int RunningNumber = 1;

            var pidName = new OracleParameter("PIDNAME", OracleDbType.Varchar2) { Value = "AHMIC_IP_REG" };
            var preset = new OracleParameter("PRESET", OracleDbType.Varchar2) { Value = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") };
            var userId = new OracleParameter("VUSERID", OracleDbType.Varchar2) { Value = _inrp };

            var returnValue = new OracleParameter("VVALUE", OracleDbType.Varchar2, 10) { Direction = ParameterDirection.Output };

            await this.Database.ExecuteSqlInterpolatedAsync($"BEGIN FUNCAHMITSYSGETRUNNO ({pidName}, {preset}, {userId},{returnValue} );END;").ConfigureAwait(false);

            return Convert.ToInt32(returnValue);

                        
        }


        public async Task<string> GetRegistrationNum(Int64 _inrp,string themeType,string AreaId, CancellationToken cancellationToken)
        {
            string regno = "";

            var getData = await GetRunNo(_inrp, cancellationToken);

            regno = getData.ToString().PadLeft(5, '0') + "/" + themeType + "/" + AreaId + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");

            return regno;
        }

        public async Task<string> GetPathAttachment(string param4, CancellationToken cancellationToken)
        {

            string grade = "";
            var get = await AHMHRD_REC00_SETTINGSDb.
               FromSqlInterpolated<AHMHRD_REC00_SETTINGS>
               ($@"
                      SELECT VALUE1 ||'' ||VALUE2 PATHATT, PARAM4
                            FROM AHMHRD_REC00_SETTINGS
                            WHERE ID   = 'AHM'
                            AND PARAM1 = 'IRS021'
                            AND PARAM2 = 'ATTACH'
                            AND PARAM3 = 'LINK'
                            AND PARAM4 = {param4} 
                ").Select(x => new AHMHRD_REC00_SETTINGS()
               {
                   PATHATT = x.PATHATT
               }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (get != null)
            {
                grade = get.PATHATT;
            }

            return grade;

        }

        public async Task<string> GetDownloadLinkIp(string fileName, CancellationToken cancellationToken)
        { //nitip update

          //  string userFolder = zone + "_" + _encryptionHelper.MD5Hash(domain + "\\" + username);

            string path = await GetPathAttachment("1", cancellationToken);
            // path = Path.Combine(path, userFolder);

            string fullPath = Path.Combine(path, fileName);

            return "handlers/file.ashx?FilePathIp=" + fullPath + "&urlparam=FilePathIp";
        }


        public async Task<string> GetNrpAtasan(string pnrp, Int64 _inrp, CancellationToken cancellationToken)
        {
            string nrpatasan = "";

            var get = await AHMHRNTM_MSTOUHEADSDb.
              FromSqlInterpolated<AHMHRNTM_MSTOUHEADS>
              ($@"
                     SELECT OUHEAD.VNRP FROM AHMHRNTM_MSTOUHEADS OUHEAD, FMHRD_EMPLOYMENTS EMP WHERE EMP.GENDAT_VEND_VND_CODE = 'AHM' AND OUHEAD.VOUID = EMP.SECTION_ID AND TO_CHAR(EMP.GENDAT_NRP) = TO_CHAR{_inrp} 
                ").Select(x => new AHMHRNTM_MSTOUHEADS()
              {
                  vnrp = x.vnrp
              }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (get != null)
            {
                nrpatasan = get.vnrp;
            }

            return nrpatasan;

        }

        public async Task<AHMHRIRS021_InfoSect> GetInfoSectionByNrp(string pnrp, int usr, Int64 _inrp, CancellationToken cancellationToken)
        {
            string vareaID = "";
            string vareaDesc = "";
            var getData = await AHMHRIRS021_InfoSectDb.
             FromSqlInterpolated<AHMHRIRS021_InfoSect>
             ($@"
                  SELECT GENDAT_NRP AS NRP,
                         1 AS VROLE, 
                        (SELECT SECT_TYPE FROM FMHRD_SECTIONS
                        WHERE SECTN_ID = CA.SECTION_ID AND VEND_VND_CODE = 'AHM') SECTYPE, 
                        (SELECT EMP_STATUS FROM FMHRD_EMPLOYMENTS WHERE GENDAT_NRP = {pnrp} AND 
                        GENDAT_VEND_VND_CODE = 'AHM') EMP_STAT,
                        (SELECT NAME FROM FMHRD_GENERAL_DATAS
                         WHERE NRP = GENDAT_NRP AND VEND_VND_CODE = 'AHM') VNAMA, 
        (SELECT SECTN_ID FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'SC' AND VEND_VND_CODE = 'AHM' 
AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD AND ROWNUM = 1 CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID AND VEND_VND_CODE = 'AHM' 
        START WITH SECTN_ID = CA.SECTION_ID AND VEND_VND_CODE = 'AHM' ) SEC_ID, (SELECT SEC_NAME FROM
                FMHRD_SECTIONS WHERE SECT_TYPE = 'SC' AND VEND_VND_CODE = 'AHM' 
                AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD AND ROWNUM = 1 CONNECT BY 
                PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID 
                AND VEND_VND_CODE = 'AHM' START WITH SECTN_ID = CA.SECTION_ID 
                AND VEND_VND_CODE = 'AHM' ) SEC_NAME, 
            (SELECT SECTN_ID FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'SD' AND VEND_VND_CODE
            = 'AHM' AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD AND ROWNUM = 1 
        CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID 
        AND VEND_VND_CODE = 'AHM' START WITH SECTN_ID = CA.SECTION_ID 
                AND VEND_VND_CODE = 'AHM' ) SUBDEPT_ID, 
            (SELECT SEC_NAME FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'SD' 
        AND VEND_VND_CODE = 'AHM' AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD 
                AND ROWNUM = 1 CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID AND VEND_VND_CODE = 'AHM' START WITH SECTN_ID = CA.SECTION_ID 
AND VEND_VND_CODE = 'AHM' ) SUBDEPT_NAME, (SELECT SECTN_ID FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'DP' AND VEND_VND_CODE = 'AHM' AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD AND ROWNUM = 1 CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID AND VEND_VND_CODE = 'AHM' 
            START WITH SECTN_ID = CA.SECTION_ID AND VEND_VND_CODE = 'AHM' )
            DEPT_ID, (SELECT SEC_NAME FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'DP' 
        AND VEND_VND_CODE = 'AHM' AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD AND ROWNUM = 1 
            CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID AND 
        VEND_VND_CODE = 'AHM' START WITH SECTN_ID = CA.SECTION_ID AND VEND_VND_CODE = 'AHM' )
        DEPT_NAME, (SELECT SECTN_ID FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'DV' 
        AND VEND_VND_CODE = 'AHM' AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD 
        AND ROWNUM = 1 CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND 
        SECT_SECTN_ID != SECTN_ID AND VEND_VND_CODE = 'AHM' START
        WITH SECTN_ID = CA.SECTION_ID AND VEND_VND_CODE = 'AHM' ) DIV_ID,
        (SELECT SEC_NAME FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'DV' 
            AND VEND_VND_CODE = 'AHM' AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD 
        AND ROWNUM = 1 CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID AND VEND_VND_CODE = 'AHM' START
        WITH SECTN_ID = CA.SECTION_ID AND VEND_VND_CODE = 'AHM' ) DIV_NAME, (SELECT SECTN_ID FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'DR' 
AND VEND_VND_CODE = 'AHM' AND SYSDATE BETWEEN BGN_EFFD AND LST_EFFD AND ROWNUM = 1 CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID AND VEND_VND_CODE = 'AHM'
START WITH SECTN_ID = CA.SECTION_ID AND VEND_VND_CODE = 'AHM' ) DIR_ID, (SELECT SEC_NAME FROM FMHRD_SECTIONS WHERE SECT_TYPE = 'DR' AND VEND_VND_CODE = 'AHM' AND SYSDATE
BETWEEN BGN_EFFD AND LST_EFFD AND ROWNUM = 1 CONNECT BY PRIOR SECT_SECTN_ID = SECTN_ID AND SECT_SECTN_ID != SECTN_ID AND VEND_VND_CODE = 'AHM' START WITH SECTN_ID = CA.SECTION_ID
AND VEND_VND_CODE = 'AHM' ) DIR_NAME FROM FMHRD_EMPLOYMENTS CA WHERE GENDAT_VEND_VND_CODE = 'AHM' AND TO_CHAR(GENDAT_NRP) = TO_CHAR({_inrp})

                ").Select(x => new AHMHRIRS021_InfoSect()
             {
                 VDSUBMITTED = DateTime.Now.ToString("dd-MMM-yyyy"),
                 NRP = x.DEPT_NAME,
                 AREA = vareaID,
                 AREADESC = vareaDesc,
                 VNAMA = x.VNAMA,
                 SEC_ID = x.SEC_ID,
                 SEC_NAME = x.SEC_NAME,
                     SUBDEPT_ID = x.SUBDEPT_ID,
                     SUBDEPT_NAME = x.SUBDEPT_NAME,
                 DEPT_ID = x.DEPT_ID,
                 DEPT_NAME = x.DEPT_NAME,
                 DIV_ID = x.DIV_ID,
                 DIV_NAME = x.DIV_NAME,
                 DIR_ID = x.DIR_ID,
                 DIR_NAME = x.DIR_NAME,
                 EMP_STAT = x.EMP_STAT,
                 SECTYPE = x.SECTYPE
             }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);



            return getData; 
        }

        public async Task<string> GetStatusByNrp(string _inrp,CancellationToken cancellationToken)
        {
            string empStatus = "";
            var getData = await FMHRD_EMPLOYMENTSDb.
            FromSqlInterpolated<FMHRD_EMPLOYMENTS>
            ($@"
                  SELECT EMP_STATUS FROM FMHRD_EMPLOYMENTS WHERE GENDAT_NRP = {_inrp} AND GENDAT_VEND_VND_CODE = 'AHM'
                ").Select(x => new FMHRD_EMPLOYMENTS()
            {
                EMP_STATUS = x.EMP_STATUS,
            }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (getData != null)
            {
                empStatus = getData.EMP_STATUS;
            }


            return empStatus;
        }

        public async Task<string> GetAreaByNrp(Int64 _inrp, CancellationToken cancellationToken)
        {
            return await GetAreaByNrp(_inrp.ToString(), cancellationToken);
        }

        public async Task<string> GetAreaByNrp(string _inrp,CancellationToken cancellationToken)
        {
            string area = "";

            var getData = await FMHRD_EMPLOYMENTSDb.FromSqlInterpolated<FMHRD_EMPLOYMENTS>
                           ($@"
                                SELECT BRANCH as Branch  FROM FMHRD_EMPLOYMENTS WHERE TO_CHAR(GENDAT_NRP) = TO_CHAR({_inrp}) AND GENDAT_VEND_VND_CODE = 'AHM'

                            ").Select(x => new FMHRD_EMPLOYMENTS { }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if(getData != null)
            {
                area = getData.Branch;
            }

            return area;

        }
    }
}
