using Microsoft.EntityFrameworkCore;

namespace TibiaPixelBot.DataModels
{
    public partial class TibiaDBContext : DbContext
    {
        public virtual DbSet<ConfiguracaoEmail> ConfiguracaoEmail { get; set; }
        public virtual DbSet<Mensagem> Mensagem { get; set; }
        public virtual DbSet<ParametroSistema> ParametroSistema { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<VinculoUsuarioMensagem> VinculoUsuarioMensagem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=TibiaDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfiguracaoEmail>(entity =>
            {
                entity.Property(e => e.Ssl).HasColumnName("SSl");
            });

            modelBuilder.Entity<Mensagem>(entity =>
            {
                entity.Property(e => e.Cabecalho).IsRequired();

                entity.Property(e => e.Corpo).IsRequired();

                entity.Property(e => e.Imagem)
                    .IsRequired()
                    .HasColumnType("image");
            });

            modelBuilder.Entity<ParametroSistema>(entity =>
            {
                entity.Property(e => e.Descricao).IsRequired();

                entity.Property(e => e.Nome).IsRequired();

                entity.Property(e => e.Valor).IsRequired();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Senha).IsRequired();
            });

            modelBuilder.Entity<VinculoUsuarioMensagem>(entity =>
            {
                entity.HasOne(d => d.IdMensagemNavigation)
                    .WithMany(p => p.VinculoUsuarioMensagem)
                    .HasForeignKey(d => d.IdMensagem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VinculoUsuarioMensagem_Mensagem");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.VinculoUsuarioMensagem)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VinculoUsuarioMensagem_Usuario");
            });
        }
    }
}
