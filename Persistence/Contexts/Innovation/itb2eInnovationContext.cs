using AHM.Domain.Ahmhrirs.Entities.Innovation;
using AHM.Domain.Ahmitb2e.Entities;
using Core.Application.Interfaces.Innovation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts.Innovation
{
    public class itb2eInnovationContext : DbContext, Iitb2eInnovationContext
    {
        public itb2eInnovationContext(DbContextOptions<itb2eInnovationContext> options) : base(options)
        {

        }

        public itb2eInnovationContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new AhmhrtmoMstholidaysConfiguration());
            modelBuilder.Entity<Ahmitb2eMstprofile>().HasNoKey();
            modelBuilder.Entity<Ahmitb2eMstusrrole>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<AhmhrirsTxnip> AhmhrirsTxnipDb => Set<AhmhrirsTxnip>();

        //public DbSet<AhmhrirsDtlemp> AhmhrirsDtlempDb => Set<AhmhrirsDtlemp>();



        //public virtual DbSet<AhmhrirsDtlip> AhmhrirsDtlipDb { get; set; } = null!;

        //public virtual DbSet<AhmhrirsAreapic> AhmhrirsAreapicDb { get; set; } = null!;

        public virtual DbSet<Ahmitb2eMstprofile> Ahmitb2eMstprofileDb { get; set; } = null!;

        public virtual DbSet<Ahmitb2eMstusrrole> Ahmitb2eMstusrroleDb { get; set; } = null!;

        

        //public virtual DbSet<AHMHRIRS_HDRFORMSETS> AHMHRIRS_HDRFORMSETSDb { get; set; } = null!;





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=t13438.astra-honda.com)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=devhrtxn)));User Id=MSY_B2E_DV;Password=newvario160;");
            }
        }


        

        public async Task<string> GetNRPDelegasi(string pnrp,CancellationToken cancellationToken)
        {
            string vnrp = "";
            var getData = await Ahmitb2eMstprofileDb.FromSqlInterpolated<Ahmitb2eMstprofile>
                                ($@"
                                    SELECT VNRP
                                                    FROM AHMITB2E_MSTPROFILES
                                                    WHERE VUSERID = (SELECT VOBJECTID FROM AHMITWFS_MSTWFDLG
                                                                   WHERE VUSERID = (SELECT VUSERID FROM AHMITB2E_MSTPROFILES WHERE VNRP =  {pnrp} )
                                                   AND CONVERT(date, GETDATE()) BETWEEN DBGNEFF AND DLSTEFF AND VWFID='WF_AHMHRIRS021'
                                ").Select(x=>new Ahmitb2eMstprofile()
                                {
                                    Vnrp = x.Vnrp
                                }).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            if(getData != null)
            {
                vnrp = getData.Vnrp;
            }
            return vnrp;
        }

        public async Task<List<string>> GetUserRoles(string user, CancellationToken cancellationToken)
        {
            List<string> roles = new List<string>();
            roles = await (from x in Ahmitb2eMstusrroleDb where x.Vuserid == user select x.Vroleid).ToListAsync(cancellationToken).ConfigureAwait(false);
            return roles;
        }

        public async Task<string> GetUserIdByNrp(string pnrp,CancellationToken cancellationToken)
        {
            string userid = "";

            var i = (from x in Ahmitb2eMstprofileDb where x.Vnrp == pnrp select x).FirstOrDefault();
            if (i != null) userid = i.Vuserid.ToString();


            return userid;
        }

        



    }
}
