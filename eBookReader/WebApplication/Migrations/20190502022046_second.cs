using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_GenreID",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "GenreID",
                table: "Books",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_GenreID",
                table: "Books",
                column: "GenreID",
                principalTable: "Genres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_GenreID",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "GenreID",
                table: "Books",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_GenreID",
                table: "Books",
                column: "GenreID",
                principalTable: "Genres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
