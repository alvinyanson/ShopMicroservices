using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductCatalogService.Migrations
{
    /// <inheritdoc />
    public partial class seedProductTableAndCategory : Migration
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

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
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
                    { 1, 3, "Spiky Cactus", "https://readonlydemo.vendure.io/assets/preview/78/charles-deluvio-695736-unsplash__preview.jpg?w=200&h=200", "Spiky Cactus", 1000.0 },
                    { 2, 3, "Tulip Pot", "https://readonlydemo.vendure.io/assets/preview/14/natalia-y-345738-unsplash__preview.jpg?w=200&h=200", "Tulip Pot", 1000.0 },
                    { 3, 3, "Hanging Plant", "https://readonlydemo.vendure.io/assets/preview/5b/alex-rodriguez-santibanez-200278-unsplash__preview.jpg?w=200&h=200", "Hanging Plant", 1900.0 },
                    { 4, 3, "Aloe Vera", "https://readonlydemo.vendure.io/assets/preview/29/silvia-agrasar-227575-unsplash__preview.jpg?w=200&h=200", "Aloe Vera", 1900.0 },
                    { 5, 3, "Fern Blechnum Gibbum", "https://readonlydemo.vendure.io/assets/preview/6d/caleb-george-536388-unsplash__preview.jpg?w=200&h=200", "Fern Blechnum Gibbum", 1900.0 },
                    { 6, 3, "Assorted Indoor Succulents", "https://readonlydemo.vendure.io/assets/preview/81/annie-spratt-78044-unsplash__preview.jpg?w=200&h=200", "Assorted Indoor Succulents", 1000.0 },
                    { 7, 3, "Orchid", "https://readonlydemo.vendure.io/assets/preview/88/zoltan-kovacs-642412-unsplash__preview.jpg?w=200&h=200", "Orchid", 1000.0 },
                    { 8, 3, "Bonsai Tree", "https://readonlydemo.vendure.io/assets/preview/f3/mark-tegethoff-667351-unsplash__preview.jpg?w=200&h=200", "Bonsai Tree", 1000.0 },
                    { 9, 3, "Guardian Lion Statue", "https://readonlydemo.vendure.io/assets/preview/44/vincent-liu-525429-unsplash__preview.jpg?w=200&h=200", "Guardian Lion Statue", 1000.0 },
                    { 10, 3, "Hand Trowel", "https://readonlydemo.vendure.io/assets/preview/7d/neslihan-gunaydin-3493-unsplash__preview.jpg?w=200&h=200", "Hand Trowel", 1600.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
