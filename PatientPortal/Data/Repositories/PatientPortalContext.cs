using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Xml.Serialization;

namespace PatientPortalApp.Data
{
    public class PatientPortalContext: DbContext
    {
        public PatientPortalContext(DbContextOptions<PatientPortalContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TblSlot>().ToTable("tblSlot", schema: "dbo");
            //modelBuilder.Entity<tblSlot>().HasOne(x => x.Location);
            //modelBuilder.Entity<tblSlot>().HasOne(x => x.Doctor);


            modelBuilder.Entity<TblBooking>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<TblBooking>().ToTable("tblBooking", schema: "dbo");
                

            modelBuilder.Entity<TblPatient>().ToTable("tblPatient", schema: "dbo");

            modelBuilder.Entity<TblDoctor>().ToTable("tblDoctor", schema: "dbo");

            //modelBuilder.Entity<TblLocation>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<TblLocation>().ToTable("tblLocation", schema: "dbo");
            modelBuilder.Entity<TblUser>().ToTable("tblUser", schema: "dbo");
        }

        public virtual DbSet<TblSlot> Slots { get; set; }
        public virtual DbSet<TblDoctor> Doctors { get; set; }
        public virtual DbSet<TblLocation> Locations { get; set; }
        public virtual DbSet<TblUser> Users { get; set; }
        public virtual DbSet<TblPatient> Patients { get; set; }

        public virtual DbSet<TblBooking> Bookings { get; set; }
    }
}
