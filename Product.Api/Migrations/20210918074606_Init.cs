using Microsoft.EntityFrameworkCore.Migrations;

namespace Product.Api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(16)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Name", "Stock" },
                values: new object[] { 1, "ThinkPad", 100 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Name", "Stock" },
                values: new object[] { 2, "Mac", 100 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
