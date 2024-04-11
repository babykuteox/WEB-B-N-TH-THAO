using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CNWEB.Models
{
    public partial class Data : DbContext
    {
        public Data()
            : base("name=Data")
        {
        }

        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<bill> bills { get; set; }
        public virtual DbSet<bill_detail> bill_detail { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<employee> employees { get; set; }
        public virtual DbSet<manufacturer> manufacturers { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<unit> units { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<account>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.customers)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.id_account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<account>()
                .HasMany(e => e.employees)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.id_account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<bill>()
                .HasMany(e => e.bill_detail)
                .WithRequired(e => e.bill)
                .HasForeignKey(e => e.id_bill)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.products)
                .WithRequired(e => e.category)
                .HasForeignKey(e => e.id_category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<customer>()
                .HasMany(e => e.bills)
                .WithRequired(e => e.customer)
                .HasForeignKey(e => e.id_customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<manufacturer>()
                .HasMany(e => e.products)
                .WithRequired(e => e.manufacturer)
                .HasForeignKey(e => e.id_manufacturer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<product>()
                .Property(e => e.image_main)
                .IsUnicode(false);

            modelBuilder.Entity<product>()
                .Property(e => e.image1)
                .IsUnicode(false);

            modelBuilder.Entity<product>()
                .Property(e => e.image2)
                .IsUnicode(false);

            modelBuilder.Entity<product>()
                .Property(e => e.image3)
                .IsUnicode(false);

            modelBuilder.Entity<product>()
                .HasMany(e => e.bill_detail)
                .WithRequired(e => e.product)
                .HasForeignKey(e => e.id_product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<unit>()
                .HasMany(e => e.products)
                .WithRequired(e => e.unit)
                .HasForeignKey(e => e.id_unit)
                .WillCascadeOnDelete(false);
        }
    }
}
