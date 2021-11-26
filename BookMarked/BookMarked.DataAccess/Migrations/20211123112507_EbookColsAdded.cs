using Microsoft.EntityFrameworkCore.Migrations;

namespace BookMarked.DataAccess.Migrations
{
    public partial class EbookColsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "EBooks");

            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "EBooks",
                newName: "CoverImageUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "EBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "EBooks",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EBooks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BookPdfUrl",
                table: "EBooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "EBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EBooks_CategoryId",
                table: "EBooks",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EBooks_Categories_CategoryId",
                table: "EBooks",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EBooks_Categories_CategoryId",
                table: "EBooks");

            migrationBuilder.DropIndex(
                name: "IX_EBooks_CategoryId",
                table: "EBooks");

            migrationBuilder.DropColumn(
                name: "BookPdfUrl",
                table: "EBooks");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "EBooks");

            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "EBooks",
                newName: "ISBN");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "EBooks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "EBooks",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EBooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "EBooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
