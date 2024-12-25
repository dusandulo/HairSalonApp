using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSalon.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a temporary column to store the integer values
            migrationBuilder.AddColumn<int>(
                name: "RoleTemp",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Copy the values from the old column to the temporary column
            migrationBuilder.Sql(
                @"
                UPDATE ""Users""
                SET ""RoleTemp"" = 
                CASE ""Role""
                    WHEN 'Client' THEN 0
                    WHEN 'Stylist' THEN 1
                    WHEN 'Admin' THEN 2
                    ELSE 0
                END
                ");

            // Drop the old column
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            // Rename the temporary column to the original column name
            migrationBuilder.RenameColumn(
                name: "RoleTemp",
                table: "Users",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add the old column back
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Copy the values from the new column to the old column
            migrationBuilder.Sql(
                @"
                UPDATE ""Users""
                SET ""Role"" = 
                CASE ""Role""
                    WHEN 0 THEN 'Client'
                    WHEN 1 THEN 'Stylist'
                    WHEN 2 THEN 'Admin'
                    ELSE 'Client'
                END
                ");

            // Drop the new column
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            // Rename the old column back to the original column name
            migrationBuilder.RenameColumn(
                name: "RoleTemp",
                table: "Users",
                newName: "Role");
        }
    }
}