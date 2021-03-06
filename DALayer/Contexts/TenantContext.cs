namespace DALayer
{
    using Entities;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    public class TenantContext : IdentityDbContext<Usuario>, IDbModelCacheKeyProvider
    {

        public String SchemaName;
        public TenantContext(string connection, String TennantId)
            : base(connection)
        {  
            this.SchemaName = TennantId; 
        }

        public TenantContext()
           : base("Admin")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().Configure(c => c.ToTable(c.ClrType.Name, this.SchemaName));
            modelBuilder.Entity<IdentityUser>().ToTable("Usuario", this.SchemaName); 
            modelBuilder.Entity<IdentityUserRole>().ToTable("UsuarioRol", this.SchemaName).HasKey(r => new { r.RoleId, r.UserId }); 
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin", this.SchemaName).HasKey<string>(l => l.UserId); 
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UsuarioClaims", this.SchemaName);
            modelBuilder.Entity<IdentityRole>().ToTable("UsuarioRoles", this.SchemaName).HasKey<string>(l => l.Id);
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<UserLogin> Login { get; set; }
        public virtual DbSet<ActividadJugador> ActividadJugador { get; set; }
        public virtual DbSet<Jugador> Jugador { get; set; }
        public virtual DbSet<Recurso> Recurso { get; set; }
        public virtual DbSet<MapaNode> MapaNode { get; set; }
        public virtual DbSet<Alianza> Alianza { get; set; }
        public virtual DbSet<Dependencia> Dependencia { get; set; }
        public virtual DbSet<Destacamento> Destacamento { get; set; }
        public virtual DbSet<Edificio> Edificio { get; set; }
        public virtual DbSet<Costo> Costo { get; set; }
        public virtual DbSet<Capacidad> Capacidad { get; set; }
        public virtual DbSet<Produce> Produce { get; set; }
        public virtual DbSet<Investigacion> Investigacion { get; set; }
        public virtual DbSet<PaquetePaypal> PaquetePaypal { get; set; }
        public virtual DbSet<HistorialVentas> HistorialVentas { get; set; }
        public virtual DbSet<Unidad> Unidad { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<RelJugadorDestacamento> RelJugadorDestacamento { get; set; }
        public virtual DbSet<RelJugadorEdificio> RelJugadorEdificio { get; set; }
        public virtual DbSet<RelJugadorInvestigacion> RelJugadorInvestigacion { get; set; }
        public virtual DbSet<RelJugadorRecurso> RelJugadorRecurso { get; set; }
        public virtual DbSet<RelJugadorMapa> RelJugadorMapa { get; set; }
        public virtual DbSet<RelJugadorAlianza> RelJugadorAlianza { get; set; }
        public virtual DbSet<Configuracion> Configuracion { get; set; }

        public virtual DbSet<Interaction> Interaction{get; set;}
        public string CacheKey
        {
            get
            {
                return this.SchemaName;
            }
        }
    }
}
