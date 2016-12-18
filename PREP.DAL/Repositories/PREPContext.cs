using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PREP.DAL.Models;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using PREP.DAL.Functions;


namespace PREP.DAL.Repositories
{
    /// <summary>
    /// DBACCount to initalize Database by Code First
    /// </summary>
    public class PREPContext : DbContext
    {
        public PREPContext() : base("name=PREPContext")
        {
            //write Sql Scripts in Debug
            Database.Log = s => Debug.WriteLine(s);
            // Database.Log = s => Errors.SaveLastSQLScript(s);
            //Configuration.ProxyCreationEnabled = false;

            this.Configuration.LazyLoadingEnabled = false;
            //not dropcreate use the current Data whithout Change it
            Database.SetInitializer<PREPContext>(null);
            //for derbug-dropCreateDB
            //Database.SetInitializer<PREPContext>(new PREPDBInitializer());
        }

        /// <summary>
        /// Override base Model to change the defalt Properties of EntityFramework 
        /// This method is called when the model for a derived context has been initialized,
        ///  but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder that defines the model for the context being created.
        /// </param>
        /// <remarks>
        /// Reslash the Remarks of the method before adding Migrations
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //To the Migrations 
            modelBuilder.Entity<Account>();
            modelBuilder.Entity<ActivityLog>();
            modelBuilder.Entity<Area>();
            modelBuilder.Entity<AreaScore>();
            modelBuilder.Entity<CalculatedField>();
            modelBuilder.Entity<Characteristic>();
            modelBuilder.Entity<ColumnMapping>();
            modelBuilder.Entity<CP>();
            modelBuilder.Entity<CPReviewMode>();
            modelBuilder.Entity<CPReviewModeQ>();
            modelBuilder.Entity<CriteriaCPCreation>();
            modelBuilder.Entity<Employee>();
            modelBuilder.Entity<FieldValueData>();
            modelBuilder.Entity<FamilyProduct>();
            modelBuilder.Entity<HRMSFields>();
            modelBuilder.Entity<HRMSInterface>();
            modelBuilder.Entity<History>();
            modelBuilder.Entity<Milestone>();
            modelBuilder.Entity<OurVision>();
            modelBuilder.Entity<Permission>();
            modelBuilder.Entity<PermissionType>();
            modelBuilder.Entity<Product>();
            modelBuilder.Entity<Publication>();
            modelBuilder.Entity<Question>();
            //modelBuilder.Entity<AreaScoreTemp>();
            //modelBuilder.Entity<SubAreaScoreTemp>();
            //modelBuilder.Entity<PublicationTemp>();
            //modelBuilder.Entity<ActivityLog>().HasMany(History).

            modelBuilder.Entity<Question>()
                .HasRequired<Milestone>(s => s.Milestone)
                .WithMany(s => s.QuestionMilestones)
                .HasForeignKey(s => s.MilestoneID);

            modelBuilder.Entity<Question>()
                .HasRequired<Milestone>(s => s.PreviousMilestone)
                .WithMany(s => s.QuestionPreviousMilestones)
                .HasForeignKey(s => s.PreviousMilestoneID);

            //modelBuilder.Entity<Question>()
            //    .HasOptional(x => x.Milestone);

            modelBuilder.Entity<Question>()
                .HasOptional(x => x.PreviousMilestone);

            modelBuilder.Entity<QuestionArea>();
            modelBuilder.Entity<QuestionCPRevMode>();
            modelBuilder.Entity<QuestionCPRevModeQ>();
            modelBuilder.Entity<QuestionFamilyProduct>();
            modelBuilder.Entity<QuestionProduct>();
            modelBuilder.Entity<QuestionStakeholder>();
            modelBuilder.Entity<Release>();
            modelBuilder.Entity<ReleaseAreaOwner>();
            modelBuilder.Entity<ReleaseCharacteristic>();
            modelBuilder.Entity<ReleaseChecklistAnswer>();
            modelBuilder.Entity<ReleaseChecklistAnswerArcive>();
            modelBuilder.Entity<ReleaseCP>();
            modelBuilder.Entity<ReleaseCPReviewMode>();
            modelBuilder.Entity<ReleaseCPReviewModeQ>();
            modelBuilder.Entity<ReleaseFamilyProduct>();
            modelBuilder.Entity<ReleaseMilestone>();
            modelBuilder.Entity<ReleaseProduct>();
            modelBuilder.Entity<ReleaseStakeholder>();
            modelBuilder.Entity<ReleaseVendor>();
            modelBuilder.Entity<Stakeholder>();
            modelBuilder.Entity<StatusText>();
            modelBuilder.Entity<StatusAreaText>();
            modelBuilder.Entity<SubArea>();
            modelBuilder.Entity<SubAreaScore>();
            modelBuilder.Entity<SystemConfig>();
            modelBuilder.Entity<Table>();
            modelBuilder.Entity<UseFullLinks>();
            modelBuilder.Entity<Vendor>();
            modelBuilder.Entity<VendorAreas>();
            modelBuilder.Entity<EmployeeInterface>();
            modelBuilder.Entity<Links>();
            // modelBuilder.Entity<History>().(r=>r.Histories);
            // modelBuilder.Entity<History>().HasMany<ActivityLog>(n => n.ActivityLogs).WithRequired(n=> n.History).HasForeignKey(n=>n.HistoryID);
            //        modelBuilder.Entity<Question>()
            //.HasRequired(c => c.SubArea)
            //.WithMany()
            //.WillCascadeOnDelete(false);

            //modelBuilder.Entity<Side>()
            //    .HasRequired(s => s.Stage)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);
            //   modelBuilder.Types<Release>().Configure(r => r.Property(a => a.ReleaseMilestones));
            //   modelBuilder.Entity<History>();
            //relationship Question to  Milestones
            //modelBuilder.Entity<Question>()
            //       .HasRequired(qr => qr.Milestone)
            //       .WithMany(m => m.QuestionMilestones)
            //       .HasForeignKey(qr => qr.MilestoneID)
            //       .WillCascadeOnDelete(false);

            //relationship Question to PreviousMilestones
            //modelBuilder.Entity<Question>()
            //            .HasRequired(qr => qr.PreviousMilestone)
            //            .WithMany(m => m.QuestionPreviousMilestones)
            //            .HasForeignKey(qr => qr.PreviousMilestoneID)
            //            .WillCascadeOnDelete(false);

            //not complited
            //modelBuilder.Entity<ReleaseChecklistAnswer>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Release> Releases { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<ReleaseMilestone> ReleaseMilestones { get; set; }
        public DbSet<ReleaseFamilyProduct> ReleaseFamilyProducts { get; set; }
        public DbSet<ReleaseChecklistAnswerArcive> ReleaseChecklistAnswerArcives { get; set; }
        public DbSet<FamilyProduct> FamilyProducts { get; set; }
        public DbSet<ReleaseProduct> ReleaseProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReleaseStakeholder> ReleaseStakeHolders { get; set; }
        public DbSet<ReleaseAreaOwner> ReleaseAreaOwners { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Stakeholder> Stakeholder { get; set; }
        public DbSet<ReleaseCP> ReleaseCP { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<SubArea> SubArea { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<History> History { get; set; }
    }

    /// <summary>
    /// ovverload Config of initialize of Context, Only For debug!
    /// </summary>
    public class PREPDBInitializer : DropCreateDatabaseIfModelChanges<PREPContext>
    {
        protected override void Seed(PREPContext context)
        {
            base.Seed(context);
        }
    }

}
