using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OutOfOfficeSolution.Models.Context;

public partial class OutOfOfficeSolutionDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public OutOfOfficeSolutionDbContext()
    {
    }

    public OutOfOfficeSolutionDbContext(DbContextOptions<OutOfOfficeSolutionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApprovalRequest> ApprovalRequests { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApprovalRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC27553357A7");

            entity.HasIndex(e => e.LeaveRequestId, "UQ__Approval__6094218F8C819CE5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApproverId).HasColumnName("ApproverID");
            entity.Property(e => e.LeaveRequestId).HasColumnName("LeaveRequestID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Новий");

            entity.HasOne(d => d.Approver).WithMany(p => p.ApprovalRequests)
                .HasForeignKey(d => d.ApproverId)
                .HasConstraintName("FK_ApprovalRequests_ApproverID");

            entity.HasOne(d => d.LeaveRequest).WithOne(p => p.ApprovalRequest)
                .HasForeignKey<ApprovalRequest>(d => d.LeaveRequestId)
                .HasConstraintName("FK_ApprovalRequests_LeaveRequestID");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27A6A53A22");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Photo).HasMaxLength(255);
            entity.Property(e => e.Position).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Subdivision).HasMaxLength(255);

            entity.HasOne(d => d.PeoplePartnerNavigation).WithMany(p => p.InversePeoplePartnerNavigation)
                .HasForeignKey(d => d.PeoplePartner)
                .HasConstraintName("FK_Employees_PeoplePartner");

            entity.HasMany(d => d.Projects).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeProject",
                    r => r.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeProjects_ProjectID"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeProjects_EmployeeID"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "ProjectId");
                        j.ToTable("EmployeeProjects");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");
                        j.IndexerProperty<int>("ProjectId").HasColumnName("ProjectID");
                    });
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveReq__3214EC272255D7A2");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AbsenceReason).HasMaxLength(255);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Новий");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_LeaveRequests_EmployeeID");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC27E306AD0A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProjectManagerId).HasColumnName("ProjectManagerID");
            entity.Property(e => e.ProjectType).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.ProjectManager).WithMany(p => p.ProjectsNavigation)
                .HasForeignKey(d => d.ProjectManagerId)
                .HasConstraintName("FK_Projects_ProjectManagerID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

public partial class OutOfOfficeSolutionDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public virtual DbSet<User> User { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
