using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatetenat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubscriptionPlan",
                table: "Tenant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanySize",
                table: "Tenant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Tenant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Tenant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeLocation",
                table: "Tenant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Tenant",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanySize",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "OfficeLocation",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Tenant");

            migrationBuilder.AlterColumn<string>(
                name: "SubscriptionPlan",
                table: "Tenant",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
