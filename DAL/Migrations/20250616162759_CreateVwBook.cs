using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateVwBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string createViewSql = @"
    CREATE VIEW VwBook AS
    SELECT 
        dbo.TbBook.Id, 
        dbo.TbBook.TitleAr, 
        dbo.TbBook.TitleEn, 
        dbo.TbBook.DescriptionAr, 
        dbo.TbBook.DescriptionEn, 
        dbo.TbBook.PublishDate, 
        dbo.TbBook.ImagePath, 
        dbo.TbCategory.TitleAr AS CategoryTitleAr, 
        dbo.TbCategory.TitleEn AS CategoryTitleEn, 
        dbo.TbAuthor.NameAr    AS AuthorNameAr, 
        dbo.TbAuthor.NameEn    AS AuthorNameEn
    FROM dbo.TbBook
    INNER JOIN dbo.TbAuthor ON dbo.TbBook.AuthorId = dbo.TbAuthor.Id
    INNER JOIN dbo.TbCategory ON dbo.TbBook.CategoryId = dbo.TbCategory.Id
    WHERE dbo.TbBook.CurrentState = 1;
    ";

            migrationBuilder.Sql(createViewSql);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbRefreshToken_AspNetUsers_VwBookViewId",
                table: "TbRefreshToken");

            migrationBuilder.RenameColumn(
                name: "VwBookViewId",
                table: "TbRefreshToken",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TbRefreshToken_VwBookViewId",
                table: "TbRefreshToken",
                newName: "IX_TbRefreshToken_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbRefreshToken_AspNetUsers_ApplicationUserId",
                table: "TbRefreshToken",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
