using Microsoft.EntityFrameworkCore;
using SISAP.INFRAESTRUCTURA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SISAP.CORE.Entities;
 

namespace SISAP.INFRAESTRUCTURA
{
    public class SISAPDBContext : DbContext
    {
        public SISAPDBContext(DbContextOptions<SISAPDBContext> options) : base(options)
        {


        }
 
        public SISAPDBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=SISAPCORE;Integrated Security=True");
            }
        }
        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Factura> Factura { get; set; }
        public DbSet<Facturacion> Facturacion { get; set; }
        public DbSet<Urbanizacion> Urbanizacion { get; set; }
        public DbSet<Tarifario> Tarifario { get; set; }
        public DbSet<Meses> Meses { get; set; }
        public DbSet<TipoCategoria> TipoCategoria { get; set; }
        public DbSet<Servicio> servicio { get; set; }
        public DbSet<Pago> Pago { get; set; }
        public DbSet<Medidor> Medidor { get; set; }
        public DbSet<Lectura> Lectura { get; set; }
        public DbSet<Manzana> Manzana { get; set; }
        public DbSet<EstadoPago> EstadoPago { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<EstadoServicio> EstadoServicio { get; set; }
        public DbSet<DireccionDato> DireccionDato { get; set; }
        public DbSet<Direccion> Direccion { get; set; }
        public DbSet<DetalleFactura> DetalleFactura { get; set; }
        public DbSet<DetalleEstadoPago> DetalleEstadoPago { get; set; }
        public DbSet<ConsumoServicio> ConsumoServicio { get; set; }
        public DbSet<Ciclos> Ciclos { get; set; }
    
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Opcion> Opcion { get; set; }
        public DbSet<OpcionUsuario> OpcionUsuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           

            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Configurations.Add(new UsuarioConfiguration());
            //modelBuilder.Configurations.Add(new FacturacionConfiguration());
            //modelBuilder.Configurations.Add(new TarifarioConfiguration());
            //modelBuilder.Configurations.Add(new MesesConfiguration());
            //modelBuilder.Configurations.Add(new ClienteConfiguration());
            //modelBuilder.Configurations.Add(new OpcionConfiguracion());
            //modelBuilder.Configurations.Add(new OpcionUsuarioConfiguracion());
            //modelBuilder.Configurations.Add(new UrbanizacionConfiguration());
            //modelBuilder.Configurations.Add(new TipoCategoriaConfiguration());
            //modelBuilder.Configurations.Add(new ServicioConfiguration());
            //modelBuilder.Configurations.Add(new PagoConfiguration());
            //modelBuilder.Configurations.Add(new MedidorConfiguration());
            //modelBuilder.Configurations.Add(new LecturaConfiguration());
            //modelBuilder.Configurations.Add(new ManzanaConfiguration());
            //modelBuilder.Configurations.Add(new FacturaConfiguration());
            //modelBuilder.Configurations.Add(new EstadoPagoConfiguration());
            //modelBuilder.Configurations.Add(new EstadoServicioConfiguration());
            //modelBuilder.Configurations.Add(new EstadoConfiguration());
            //modelBuilder.Configurations.Add(new DireccionDatoConfiguration());
            //modelBuilder.Configurations.Add(new DireccionConfiguration());
            //modelBuilder.Configurations.Add(new DetalleFacturaConfiguration());
            //modelBuilder.Configurations.Add(new DetalleEstadoPagoConfiguration());
            //modelBuilder.Configurations.Add(new ConsumoServicioConfiguration());
            //modelBuilder.Configurations.Add(new CiclosConfiguration());
            //modelBuilder.Configurations.Add(new CategoriaConfiguration());




            modelBuilder.Entity<Categoria>().HasKey(c => new { c.CategoriaId });

           
        }
    }
}
