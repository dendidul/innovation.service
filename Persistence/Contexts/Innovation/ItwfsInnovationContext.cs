using AHM.Domain.Ahmhrirs.Entities.Innovation;
using AHM.Domain.Ahmitwfs.Entities;
using AHM.Domain.AHMITWFS.Entities;
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
    public class ItwfsInnovationContext : DbContext, IItwfsInnovationContext
    {
        public ItwfsInnovationContext(DbContextOptions<ItwfsInnovationContext> options) : base(options)
        {

        }

        public ItwfsInnovationContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new AhmhrtmoMstholidaysConfiguration());
            modelBuilder.Entity<AHMITWFS_MSTWFDLG>().HasNoKey();
            modelBuilder.Entity<AhmitwfsMstwfinbox>().HasNoKey();
            modelBuilder.Entity<AhmitwfsMstwfdocstat>().HasNoKey();
            modelBuilder.Entity<AhmitwfsMstwfdochist>().HasNoKey();
            modelBuilder.Entity<AhmitwfsMstwftask>().HasNoKey();
            modelBuilder.Entity<GetWFIDByNodocViewModel>().HasNoKey();

            modelBuilder.Entity<HistoryTable>().HasNoKey();

            modelBuilder.Ignore<AhmitwfsMstwfdocacc>();
            modelBuilder.Ignore<AhmitwfsMstwfdocatt>();
            modelBuilder.Ignore<AhmitwfsMstwfdocvar>();
            modelBuilder.Ignore<AhmitwfsMstwfinboxacc>();
           // modelBuilder.Entity<AhmitwfsMstwftaskcond>().HasNoKey();






            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<AhmhrirsTxnip> AhmhrirsTxnipDb => Set<AhmhrirsTxnip>();

        //public DbSet<AhmhrirsDtlemp> AhmhrirsDtlempDb => Set<AhmhrirsDtlemp>();



        //public virtual DbSet<AhmhrirsDtlip> AhmhrirsDtlipDb { get; set; } = null!;

        //public virtual DbSet<AhmhrirsAreapic> AhmhrirsAreapicDb { get; set; } = null!;
        public virtual DbSet<HistoryTable> HistoryTableDb { get; set; } = null!;
        public virtual DbSet<AHMITWFS_MSTWFDLG> AHMITWFS_MSTWFDLGDb { get; set; } = null!;

        public virtual DbSet<AhmitwfsMstwfinbox> AhmitwfsMstwfinboxDb { get; set; } = null!;

        public virtual DbSet<AhmitwfsMstwfdocstat> AhmitwfsMstwfdocstatDb { get; set; } = null!;

        public virtual DbSet<AhmitwfsMstwfdochist> AhmitwfsMstwfdochistDb { get; set; } = null!;

        public virtual DbSet<AhmitwfsMstwftask> AhmitwfsMstwftaskDb { get; set; } = null!;
        public virtual DbSet<GetWFIDByNodocViewModel> GetWFIDByNodocViewModelDb { get; set; } = null!;


        //public virtual DbSet<AHMHRIRS_HDRFORMSETS> AHMHRIRS_HDRFORMSETSDb { get; set; } = null!;





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=t13438.astra-honda.com)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=devhrtxn)));User Id=MSY_B2E_DV;Password=newvario160;");
            }
        }


        public async Task<string> CekDelegasiApproval(string pnrp,CancellationToken cancellationToken)
        {
            // Logger.WriteLog("#CekDelegasiApproval");

            string Delegate = "";
            
            var getData = await AHMITWFS_MSTWFDLGDb.FromSqlInterpolated<AHMITWFS_MSTWFDLG>
                          ($@"
                                SELECT CONVERT(VARCHAR(5),COUNT(*)) CNT FROM AHMITWFS_MSTWFDLG
                                WHERE VUSERID =(SELECT VUSERID FROM AHMITB2E_MSTPROFILES WHERE VNRP = {pnrp} )
                                AND CONVERT(date, GETDATE()) BETWEEN DBGNEFF AND DLSTEFF
                                AND VWFID='WF_AHMHRIRS021'
                            ").Select(x=>new AHMITWFS_MSTWFDLG()
                          { 
                            CNT = x.CNT,
                          }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if(getData != null)
            {;
                Delegate = getData.CNT;
            }
            return Delegate;




        }

       


        public async Task<List<HistoryTable>> GetHistoryWorkflow(string vwfguid,string noreg, CancellationToken cancellationToken)
        {
           
         

            List<HistoryTable> obj = new List<HistoryTable>();

            var getx = await (from x in AhmitwfsMstwfdochistDb
                              where x.Vwfguid == vwfguid && x.Vcrea != "system"
                              select x).OrderBy(s => s.Dcrea).ToListAsync(cancellationToken).ConfigureAwait(false);

            foreach(var i in getx)
            {
                HistoryTable mdl = new HistoryTable();
                mdl.VTASKNAME = GetTaskIde(noreg, i.Veventtype, i.Vtaskid);
                mdl.VTASKID = i.Vtaskid;
                mdl.VCREA = i.Vcrea;
                mdl.DCREA = i.Dcrea;
                mdl.VRESULT = i.Vtaskresult;
                mdl.VCOMMENT = i.Vnote;
                obj.Add(mdl);
            }

            //var getx = await (from x in AhmitwfsMstwfdochistDb
            //                  where x.Vwfguid == vwfguid && x.Vcrea != "system"
            //            select new HistoryTable
            //            {
            //                Vtaskid = x.Vtaskid,
            //                //VTASKNAME = GetTaskIde(noreg,x.Veventtype,x.Vtaskid),
            //                Vcrea = x.Vcrea,
            //                Dcrea = x.Dcrea,
            //                Vtaskresult = x.Vtaskresult,
            //                Vnote = x.Vnote

            //            }).OrderBy(s => s.Dcrea).ToListAsync(cancellationToken).ConfigureAwait(false);
            return obj;


        }



        public string GetTaskIde(string wfId, string eventType, string taskId)
        {
            int WorkflowVersion = 0;
           // var getdata = this.AhmitwfsMstwfdocstatDb

                var getdata = (from d in AhmitwfsMstwfdocstatDb
                              where d.Vwfguid == wfId
                              select d).FirstOrDefault();
                
            if(getdata !=null)
            {
                WorkflowVersion = getdata.Vwfversion;

                if (eventType == "0")
                {
                    return "Start Workflow";
                }
                else if (eventType =="1")
                {
                    return "End Workflow";
                }
                else if (eventType == "4")
                {
                    return "Submit Document";
                }
                else
                {
                    var sa = (from s in AhmitwfsMstwftaskDb where s.VWFID == getdata.Vwfid && s.VWFVERSION == getdata.Vwfversion && s.VTASKID == taskId select s.VTASKNAME).FirstOrDefault();
                    return sa == null ? "" : sa;
                }

            }
            else
            {
                return "kosong";
            }

        }


        public async Task<string> GetLastTaskid(string WFGuid,CancellationToken cancellationToken)
        {

            return await (from h in AhmitwfsMstwfinboxDb
                    where h.Vwfguid == WFGuid
                    && h.Vcrea != "system"
                    orderby h.Dcrea descending
                    select h.Vtaskid)
                    .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        }

    }
}
