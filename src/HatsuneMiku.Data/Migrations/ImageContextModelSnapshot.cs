﻿// <auto-generated />
using HatsuneMiku.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HatsuneMiku.Data.Migrations
{
    [DbContext(typeof(ImageContext))]
    partial class ImageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HatsuneMiku.Data.Entities.Image.ImageQueryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ImageType")
                        .HasColumnType("int");

                    b.Property<string>("Query")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SafeSearchLevel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ImageQueries");
                });

            modelBuilder.Entity("HatsuneMiku.Data.Entities.Image.ImageQueryResultEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ImageQueryId")
                        .HasColumnType("int");

                    b.Property<int>("ImageResultId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImageQueryId");

                    b.HasIndex("ImageResultId")
                        .IsUnique();

                    b.ToTable("ImageQueryResults");
                });

            modelBuilder.Entity("HatsuneMiku.Data.Entities.Image.ImageResultEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ImageResults");
                });

            modelBuilder.Entity("HatsuneMiku.Data.Entities.Image.ImageQueryResultEntity", b =>
                {
                    b.HasOne("HatsuneMiku.Data.Entities.Image.ImageQueryEntity", "ImageQuery")
                        .WithMany("ImageResults")
                        .HasForeignKey("ImageQueryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HatsuneMiku.Data.Entities.Image.ImageResultEntity", "ImageResult")
                        .WithOne("ImageQuery")
                        .HasForeignKey("HatsuneMiku.Data.Entities.Image.ImageQueryResultEntity", "ImageResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ImageQuery");

                    b.Navigation("ImageResult");
                });

            modelBuilder.Entity("HatsuneMiku.Data.Entities.Image.ImageQueryEntity", b =>
                {
                    b.Navigation("ImageResults");
                });

            modelBuilder.Entity("HatsuneMiku.Data.Entities.Image.ImageResultEntity", b =>
                {
                    b.Navigation("ImageQuery")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
