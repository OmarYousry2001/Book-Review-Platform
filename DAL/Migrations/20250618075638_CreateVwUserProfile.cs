using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateVwUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE VIEW VwUserProfile AS
                            SELECT 
                            Id, 
                            FirstName, 
                            LastName, 
                            City, 
                            Email,
                            PhoneNumber
                            FROM 
                            dbo.AspNetUsers
                            WHERE 
                            CurrentState = 1;";

            migrationBuilder.Sql(query);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
