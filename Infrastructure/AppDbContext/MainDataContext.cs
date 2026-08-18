using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AppDbContext;

public class MainDataContext : DbContext
{
    public MainDataContext(DbContextOptions<MainDataContext> options) : base(options) { }

    public DbSet<Eps> Eps => Set<Eps>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<ProfesionalSalud> Profesionales => Set<ProfesionalSalud>();
    public DbSet<DisponibilidadAgenda> Disponibilidades => Set<DisponibilidadAgenda>();
    public DbSet<CitaMedica> Citas => Set<CitaMedica>();
    public DbSet<Admision> Admisiones => Set<Admision>();
    public DbSet<HistoriaClinica> HistoriasClinicas => Set<HistoriaClinica>();
    public DbSet<DiagnosticoConsulta> DiagnosticosConsulta => Set<DiagnosticoConsulta>();
    public DbSet<ProcedimientoConsulta> ProcedimientosConsulta => Set<ProcedimientoConsulta>();
    public DbSet<NotaAclaratoria> NotasAclaratorias => Set<NotaAclaratoria>();
    public DbSet<FacturaSalud> Facturas => Set<FacturaSalud>();
    public DbSet<RipsTransaccion> RipsTransacciones => Set<RipsTransaccion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Eps
        modelBuilder.Entity<Eps>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Nit).HasMaxLength(30);

            // Seed Data para EPS en Colombia
            builder.HasData(
                new Eps { Id = 1, Codigo = "EPS001", Nombre = "SURA EPS", Nit = "800088702-2", Activo = true },
                new Eps { Id = 2, Codigo = "EPS002", Nombre = "SANITAS EPS", Nit = "800251440-6", Activo = true },
                new Eps { Id = 3, Codigo = "EPS003", Nombre = "NUEVA EPS", Nit = "900156264-2", Activo = true },
                new Eps { Id = 4, Codigo = "EPS004", Nombre = "SALUD TOTAL EPS", Nit = "800130907-4", Activo = true },
                new Eps { Id = 5, Codigo = "EPS005", Nombre = "COMPENSAR EPS", Nit = "860066942-7", Activo = true },
                new Eps { Id = 6, Codigo = "EPS006", Nombre = "COOSALUD EPS", Nit = "900226715-3", Activo = true },
                new Eps { Id = 7, Codigo = "EPS007", Nombre = "PARTICULAR", Nit = "000000000-0", Activo = true }
            );
        });

        // Paciente
        modelBuilder.Entity<Paciente>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.TipoDocumento).HasConversion<string>().HasMaxLength(10);
            builder.Property(p => p.NumeroDocumento).HasMaxLength(30).IsRequired();
            builder.HasIndex(p => new { p.TipoDocumento, p.NumeroDocumento }).IsUnique();
            builder.Property(p => p.PrimerNombre).HasMaxLength(50).IsRequired();
            builder.Property(p => p.SegundoNombre).HasMaxLength(50);
            builder.Property(p => p.PrimerApellido).HasMaxLength(50).IsRequired();
            builder.Property(p => p.SegundoApellido).HasMaxLength(50);
            builder.Property(p => p.Genero).HasConversion<string>().HasMaxLength(20);
            builder.Property(p => p.RegimenSalud).HasConversion<string>().HasMaxLength(30);
            builder.Property(p => p.Estado).HasConversion<int>();

            builder.HasOne(p => p.EPS)
                .WithMany()
                .HasForeignKey(p => p.EpsId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ProfesionalSalud
        modelBuilder.Entity<ProfesionalSalud>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.TipoDocumento).HasConversion<string>().HasMaxLength(10);
            builder.Property(p => p.NumeroDocumento).HasMaxLength(30).IsRequired();
            builder.Property(p => p.NumeroRethus).HasMaxLength(50).IsRequired();
            builder.HasIndex(p => p.NumeroRethus).IsUnique();
            builder.Property(p => p.PrimerNombre).HasMaxLength(50).IsRequired();
            builder.Property(p => p.SegundoNombre).HasMaxLength(50);
            builder.Property(p => p.PrimerApellido).HasMaxLength(50).IsRequired();
            builder.Property(p => p.SegundoApellido).HasMaxLength(50);
            builder.Property(p => p.Especialidad).HasMaxLength(100).IsRequired();
        });

        // DisponibilidadAgenda
        modelBuilder.Entity<DisponibilidadAgenda>(builder =>
        {
            builder.HasKey(d => d.Id);
            builder.HasOne(d => d.Profesional)
                .WithMany()
                .HasForeignKey(d => d.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // CitaMedica
        modelBuilder.Entity<CitaMedica>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Estado).HasConversion<string>().HasMaxLength(30);
            builder.Property(c => c.MotivoConsulta).HasMaxLength(500).IsRequired();

            builder.HasOne(c => c.Paciente)
                .WithMany()
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Profesional)
                .WithMany()
                .HasForeignKey(c => c.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Admision
        modelBuilder.Entity<Admision>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.TipoContrato).HasConversion<string>().HasMaxLength(30);
            builder.Property(a => a.ValorConsulta).HasColumnType("decimal(18,2)");
            builder.Property(a => a.CopagoOCuotaModeradora).HasColumnType("decimal(18,2)");
            builder.Property(a => a.ValorNeto).HasColumnType("decimal(18,2)");

            builder.HasOne(a => a.CitaMedica)
                .WithOne()
                .HasForeignKey<Admision>(a => a.CitaMedicaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Paciente)
                .WithMany()
                .HasForeignKey(a => a.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // HistoriaClinica
        modelBuilder.Entity<HistoriaClinica>(builder =>
        {
            builder.HasKey(h => h.Id);
            
            builder.HasOne(h => h.Paciente)
                .WithMany()
                .HasForeignKey(h => h.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Profesional)
                .WithMany()
                .HasForeignKey(h => h.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.CitaMedica)
                .WithOne()
                .HasForeignKey<HistoriaClinica>(h => h.CitaMedicaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(h => h.Diagnosticos)
                .WithOne()
                .HasForeignKey(d => d.HistoriaClinicaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.Procedimientos)
                .WithOne()
                .HasForeignKey(p => p.HistoriaClinicaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.NotasAclaratorias)
                .WithOne()
                .HasForeignKey(n => n.HistoriaClinicaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DiagnosticoConsulta
        modelBuilder.Entity<DiagnosticoConsulta>(builder =>
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.CodigoCIE11).HasMaxLength(20).IsRequired();
            builder.Property(d => d.TipoDiagnostico).HasConversion<string>().HasMaxLength(30);
        });

        // ProcedimientoConsulta
        modelBuilder.Entity<ProcedimientoConsulta>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CodigoCUPS).HasMaxLength(20).IsRequired();
        });

        // NotaAclaratoria
        modelBuilder.Entity<NotaAclaratoria>(builder =>
        {
            builder.HasKey(n => n.Id);
            builder.HasOne(n => n.Profesional)
                .WithMany()
                .HasForeignKey(n => n.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // FacturaSalud
        modelBuilder.Entity<FacturaSalud>(builder =>
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.ValorBruto).HasColumnType("decimal(18,2)");
            builder.Property(f => f.ValorCopago).HasColumnType("decimal(18,2)");
            builder.Property(f => f.ValorDescuento).HasColumnType("decimal(18,2)");
            builder.Property(f => f.ValorNeto).HasColumnType("decimal(18,2)");

            builder.HasOne(f => f.Admision)
                .WithOne()
                .HasForeignKey<FacturaSalud>(f => f.AdmisionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // RipsTransaccion
        modelBuilder.Entity<RipsTransaccion>(builder =>
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.JsonRips).HasColumnType("jsonb");
            builder.HasOne(r => r.Factura)
                .WithOne()
                .HasForeignKey<RipsTransaccion>(r => r.FacturaId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}