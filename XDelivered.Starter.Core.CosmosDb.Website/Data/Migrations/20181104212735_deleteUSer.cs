using Microsoft.EntityFrameworkCore.Migrations;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Data.Migrations
{
    public partial class deleteUSer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AspNetUsers");
        }
    }
}
