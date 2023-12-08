﻿// <auto-generated />
using LimaniaStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LimaniaStore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LimaniaStore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Aksiyon"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Bilim Kurgu"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "Macera"
                        },
                        new
                        {
                            Id = 4,
                            DisplayOrder = 4,
                            Name = "Komedi"
                        },
                        new
                        {
                            Id = 5,
                            DisplayOrder = 5,
                            Name = "Romantik"
                        },
                        new
                        {
                            Id = 6,
                            DisplayOrder = 6,
                            Name = "Korku"
                        },
                        new
                        {
                            Id = 7,
                            DisplayOrder = 7,
                            Name = "Gerilim"
                        },
                        new
                        {
                            Id = 8,
                            DisplayOrder = 8,
                            Name = "Çocuk"
                        },
                        new
                        {
                            Id = 9,
                            DisplayOrder = 9,
                            Name = "Yetişkin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
