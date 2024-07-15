using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SistemaFacturacionAppteLink.Models;

public partial class SistemaFacturacionContext : DbContext
{
    public SistemaFacturacionContext()
    {
    }

    public SistemaFacturacionContext(DbContextOptions<SistemaFacturacionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<CorreosEnviado> CorreosEnviados { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<ItemsFactura> ItemsFacturas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D5946642E01B2D88");

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.RucDni).HasMaxLength(15);
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.HasKey(e => e.IdConfiguracion).HasName("PK__Configur__F6E145D00FB21255");

            entity.ToTable("Configuracion");
        });

        modelBuilder.Entity<CorreosEnviado>(entity =>
        {
            entity.HasKey(e => e.IdCorreo).HasName("PK__CorreosE__872F8EAE5A94AF49");

            entity.Property(e => e.CodigoTemporal).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.CorreosEnviados)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CorreosEnviados_Usuarios");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__Facturas__50E7BAF1AE119492");

            entity.HasIndex(e => e.NumeroFactura, "UQ__Facturas__CF12F9A6DAC678B0").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Igv)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("IGV");
            entity.Property(e => e.NumeroFactura).HasMaxLength(50);
            entity.Property(e => e.PorcentajeIgv)
                .HasDefaultValueSql("((18.00))")
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("PorcentajeIGV");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Facturas_Clientes");
        });

        modelBuilder.Entity<ItemsFactura>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PK__ItemsFac__51E842623C52F2F5");

            entity.ToTable("ItemsFactura", tb => tb.HasTrigger("TR_ActualizarStock"));

            entity.Property(e => e.Cantidad).HasDefaultValueSql("((1))");
            entity.Property(e => e.CodigoProducto).HasMaxLength(15);
            entity.Property(e => e.NombreProducto).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SubtotalF).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CodigoProductoNavigation).WithMany(p => p.ItemsFacturas)
                .HasPrincipalKey(p => p.Codigo)
                .HasForeignKey(d => d.CodigoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemsFactura_Productos");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.ItemsFacturas)
                .HasForeignKey(d => d.IdFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemsFactura_Facturas");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__098892102117266A");

            entity.HasIndex(e => e.Codigo, "UQ__Producto__06370DAC720AED4C").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Codigo).HasMaxLength(15);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97D25600B2");

            entity.ToTable(tb => tb.HasTrigger("TR_ControlIntentosLogin"));

            entity.Property(e => e.Bloqueado).HasDefaultValueSql("((0))");
            entity.Property(e => e.Contrasena).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.IntentosFallidos).HasDefaultValueSql("((0))");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .HasColumnName("Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
