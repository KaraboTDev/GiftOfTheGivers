using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftOfTheGivers.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Volunteers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectManagerUserId",
                table: "ReliefProjects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_UserId",
                table: "Volunteers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerAssignments_ProjectId",
                table: "VolunteerAssignments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ReliefProjects_ProjectManagerUserId",
                table: "ReliefProjects",
                column: "ProjectManagerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReliefProjects_AspNetUsers_ProjectManagerUserId",
                table: "ReliefProjects",
                column: "ProjectManagerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerAssignments_ReliefProjects_ProjectId",
                table: "VolunteerAssignments",
                column: "ProjectId",
                principalTable: "ReliefProjects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerAssignments_Volunteers_VolunteerId",
                table: "VolunteerAssignments",
                column: "VolunteerId",
                principalTable: "Volunteers",
                principalColumn: "VolunteerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_AspNetUsers_UserId",
                table: "Volunteers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReliefProjects_AspNetUsers_ProjectManagerUserId",
                table: "ReliefProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerAssignments_ReliefProjects_ProjectId",
                table: "VolunteerAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerAssignments_Volunteers_VolunteerId",
                table: "VolunteerAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_AspNetUsers_UserId",
                table: "Volunteers");

            migrationBuilder.DropIndex(
                name: "IX_Volunteers_UserId",
                table: "Volunteers");

            migrationBuilder.DropIndex(
                name: "IX_VolunteerAssignments_ProjectId",
                table: "VolunteerAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ReliefProjects_ProjectManagerUserId",
                table: "ReliefProjects");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectManagerUserId",
                table: "ReliefProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
