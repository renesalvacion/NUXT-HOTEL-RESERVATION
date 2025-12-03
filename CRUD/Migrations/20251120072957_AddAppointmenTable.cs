using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberBed = table.Column<int>(type: "int", nullable: false),
                    HeadPerson = table.Column<int>(type: "int", nullable: false),
                    CrudId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Cruds_CrudId",
                        column: x => x.CrudId,
                        principalTable: "Cruds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CrudId",
                table: "Appointments",
                column: "CrudId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
