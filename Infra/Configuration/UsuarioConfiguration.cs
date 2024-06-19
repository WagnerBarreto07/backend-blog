
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();
        builder.Property(o => o.Nome).HasColumnName("Nome").HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
        builder.Property(o => o.Senha).HasColumnName("Senha").HasColumnType("varchar(128)").HasMaxLength(128).IsRequired();
        builder.Property(o => o.Role).HasColumnName("Role").HasColumnType("varchar(20)").HasMaxLength(20).IsRequired();
        builder.Property(o => o.UltimoLogin).HasColumnName("UltimoLogin").HasColumnType("datetime").IsRequired();
        builder.Property(o => o.Email).HasColumnName("Email").HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
    }
}
