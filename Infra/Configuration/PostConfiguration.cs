
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Configuration;
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();
        builder.Property(o => o.Conteudo).HasColumnName("Conteudo").HasColumnType("text").IsRequired();
        builder.Property(o => o.UserId).HasColumnName("UserId").HasColumnType("int").IsRequired();
        builder.Property(o => o.DataCriacao).HasColumnName("DataCriacao").HasColumnType("datetime").IsRequired();
        builder.Property(o => o.Titulo).HasColumnName("Titulo").HasColumnType("varchar(200)").IsRequired();
    }
}
