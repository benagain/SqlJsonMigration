﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SqlJsonMigration;

#nullable disable

namespace SqlJsonMigration.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SqlJsonMigration.MyDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("SqlJsonMigration.MyDocument", b =>
                {
                    b.OwnsOne("SqlJsonMigration.TheJsonDocument", "Json", b1 =>
                        {
                            b1.Property<int>("MyDocumentId")
                                .HasColumnType("int");

                            b1.Property<string>("NewProperty")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Property1")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Property2")
                                .HasColumnType("int");

                            b1.HasKey("MyDocumentId");

                            b1.ToTable("Documents");

                            b1.ToJson("Json");

                            b1.WithOwner()
                                .HasForeignKey("MyDocumentId");
                        });

                    b.Navigation("Json")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
