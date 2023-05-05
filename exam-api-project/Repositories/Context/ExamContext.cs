using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using exam_api_project.models.Entities;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories.Context;

public class ExamContext : DbContext
{
    // Add Encryption Provider to context
    private readonly IEncryptionProvider _provider;
    public ExamContext(DbContextOptions<ExamContext> options) : base(options)
    {
        // Add Encryption Provider to context
        _provider = new GenerateEncryptionProvider(Environment.GetEnvironmentVariable("DATABASE_ENCRYPTION_KEY"));
    }

    // Add DbSet for each model
    public DbSet<UserModel> Users { get; set; }
    public DbSet<MedicineModel> Medicines { get; set; }
    public DbSet<PatientTodoModel> PatientTodos { get; set; }
    public DbSet<PatientMedicineModel> PatientMedicines { get; set; }
    public DbSet<PatientModel> Patients { get; set; }
    public DbSet<DepartmentModel> Departments { get; set; }
    public DbSet<DeviceModel> Devices { get; set; }

    public DbSet<PatientJournalModel> PatientJournals { get; set; }


    /// <summary>
    ///     Override SaveChanges to set CreatedAt and UpdatedAt
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var insertedEntities = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity)
            .OfType<Model>()
            .ToList();

        foreach (var insertedEntity in insertedEntities) insertedEntity.CreatedAt = DateTime.UtcNow;

        var modifiedEntities = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity)
            .OfType<Model>()
            .ToList();

        foreach (var modifiedEntity in modifiedEntities) modifiedEntity.UpdatedAt = DateTime.UtcNow;

        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    ///     Field that sets Email address Unique
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Enable encryption on selected columns
        builder.UseEncryption(_provider);
        // Field that sets Email address Unique
        builder.Entity<UserModel>()
            .HasIndex(u => u.Email)
            .IsUnique();
        // Field that sets Social Security Number Unique And create a index on the Social security number
        builder.Entity<PatientModel>()
            .HasIndex(p => p.SocialSecurityNumber)
            .IsUnique()
            .HasDatabaseName("IX_Patient_SocialSecurityNumber");
    }
}