﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductCatalogService.Data;

#nullable disable

namespace ProductCatalogService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProductCatalogService.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("ProductCatalogService.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Clothing & Apparel"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Home & Kitchen"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Health & Beauty"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Sports & Outdoors"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Books & Media"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Toys & Games"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Automotive"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Pets"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Jewelry & Accessories"
                        });
                });

            modelBuilder.Entity("ProductCatalogService.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 3,
                            Description = "Spiky Cactus",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/78/charles-deluvio-695736-unsplash__preview.jpg?w=200&h=200",
                            Name = "Spiky Cactus",
                            Price = 1000.0
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 3,
                            Description = "Tulip Pot",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/14/natalia-y-345738-unsplash__preview.jpg?w=200&h=200",
                            Name = "Tulip Pot",
                            Price = 1000.0
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Description = "Hanging Plant",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/5b/alex-rodriguez-santibanez-200278-unsplash__preview.jpg?w=200&h=200",
                            Name = "Hanging Plant",
                            Price = 1900.0
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 3,
                            Description = "Aloe Vera",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/29/silvia-agrasar-227575-unsplash__preview.jpg?w=200&h=200",
                            Name = "Aloe Vera",
                            Price = 1900.0
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 3,
                            Description = "Fern Blechnum Gibbum",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/6d/caleb-george-536388-unsplash__preview.jpg?w=200&h=200",
                            Name = "Fern Blechnum Gibbum",
                            Price = 1900.0
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 3,
                            Description = "Assorted Indoor Succulents",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/81/annie-spratt-78044-unsplash__preview.jpg?w=200&h=200",
                            Name = "Assorted Indoor Succulents",
                            Price = 1000.0
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 3,
                            Description = "Orchid",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/88/zoltan-kovacs-642412-unsplash__preview.jpg?w=200&h=200",
                            Name = "Orchid",
                            Price = 1000.0
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 3,
                            Description = "Bonsai Tree",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/f3/mark-tegethoff-667351-unsplash__preview.jpg?w=200&h=200",
                            Name = "Bonsai Tree",
                            Price = 1000.0
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 3,
                            Description = "Guardian Lion Statue",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/44/vincent-liu-525429-unsplash__preview.jpg?w=200&h=200",
                            Name = "Guardian Lion Statue",
                            Price = 1000.0
                        },
                        new
                        {
                            Id = 10,
                            CategoryId = 3,
                            Description = "Hand Trowel",
                            ImageUrl = "https://readonlydemo.vendure.io/assets/preview/7d/neslihan-gunaydin-3493-unsplash__preview.jpg?w=200&h=200",
                            Name = "Hand Trowel",
                            Price = 1600.0
                        });
                });

            modelBuilder.Entity("ProductCatalogService.Models.Cart", b =>
                {
                    b.HasOne("ProductCatalogService.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProductCatalogService.Models.Product", b =>
                {
                    b.HasOne("ProductCatalogService.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ProductCatalogService.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
