using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductCatalogService.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCatalogAndSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Clothing & Apparel" },
                    { 2, "Electronics" },
                    { 3, "Home & Kitchen" },
                    { 4, "Health & Beauty" },
                    { 5, "Sports & Outdoors" },
                    { 6, "Books & Media" },
                    { 7, "Toys & Games" },
                    { 8, "Automotive" },
                    { 9, "Pets" },
                    { 10, "Jewelry & Accessories" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Twin Lens Camera", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Twin Lens Camera", 1000.0 },
                    { 2, 2, "Compact SLR Camera", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Compact SLR Camera", 1000.0 },
                    { 3, 3, "Nikkormat SLR Camera", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Nikkormat SLR Camera", 1900.0 },
                    { 4, 4, "Compact Digital Camera", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Compact Digital Camera", 1900.0 },
                    { 5, 5, "Instamatic Camera", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Instamatic Camera", 1900.0 },
                    { 6, 6, "Tripod", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Tripod", 1000.0 },
                    { 7, 7, "Vintage Folding Camera", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Vintage Folding Camera", 1000.0 },
                    { 8, 8, "Camera Lens", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Camera Lens", 1000.0 },
                    { 9, 9, "Instant Camera", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Instant Camera", 1000.0 },
                    { 10, 10, "USB Cable", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "USB Cable", 1600.0 },
                    { 11, 1, "Ethernet Cable", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Ethernet Cable", 100.0 },
                    { 12, 2, "Clacky Keyboard", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Clacky Keyboard", 1000.0 },
                    { 13, 3, "Hard Drive", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Hard Drive", 1000.0 },
                    { 14, 4, "Gaming PC", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Gaming PC", 1000.0 },
                    { 15, 5, "High Performance RAM", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "High Performance RAM", 1000.0 },
                    { 16, 6, "Curvy Monitor", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Curvy Monitor", 1000.0 },
                    { 17, 7, "32-Inch Monitor", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "32-Inch Monitor", 1900.0 },
                    { 18, 8, "Wireless Optical Mouse", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Wireless Optical Mouse", 1000.0 },
                    { 19, 9, "Tablet", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Tablet", 1000.0 },
                    { 20, 10, "Laptop", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Laptop", 1000.0 },
                    { 21, 1, "Grey Fabric Sofa", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Grey Fabric Sofa", 1000.0 },
                    { 22, 2, "Hi-Top Basketball Shoe", "https://readonlydemo.vendure.io/assets/preview/3c/xavier-teo-469050-unsplash__preview.jpg?w=200&h=200", "Hi-Top Basketball Shoe", 250.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
