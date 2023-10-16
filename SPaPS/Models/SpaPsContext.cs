using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SPaPS.Models;

public partial class SpaPsContext : DbContext
{
    public SpaPsContext()
    {
    }

    public SpaPsContext(DbContextOptions<SpaPsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClientService> ClientServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-P14QNC2;DataBase=SPaPS;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientService>(entity =>
        {
            entity.ToTable("ClientService");

            entity.Property(e => e.ClientServiceId).HasColumnName("ClientService_Id");
            entity.Property(e => e.ClientId).HasColumnName("Client_Id");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
