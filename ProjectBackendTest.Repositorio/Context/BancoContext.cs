using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectBackendTest.Model;
using System.IO;

namespace ProjectBackendTest.Repository.Context
{
    public class BancoContext : DbContext
    {
        public BancoContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                dbContextOptionsBuilder.UseSqlServer(connectionString);
            }
        }



        public BancoContext(DbContextOptions<BancoContext> options)
                : base(options) { }

        public virtual DbSet<Pessoa> Pessoas { get; set; }
        public virtual DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //  base.OnModelCreating(builder);
            //builder.Entity<Pessoa>().ToTable("Pessoa");
            //builder.Entity<Endereco>().ToTable("Endereco");
            builder.Entity<Pessoa>()
                   .HasOne<Endereco>(s => s.Endereco)
                   .WithOne(ad => ad.Pessoa)
                   .HasForeignKey<Endereco>(ad => ad.IdPessoaFK);
        }


    }
}
