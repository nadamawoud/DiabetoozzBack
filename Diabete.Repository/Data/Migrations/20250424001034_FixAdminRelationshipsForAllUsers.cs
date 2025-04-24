using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diabetes.Repository.Data.Migrations
{
    public partial class FixAdminRelationshipsForAllUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CasualUsers_Admins_AdminID",
                table: "CasualUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Clerks_Admins_AdminID",
                table: "Clerks");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Admins_AdminID",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "AdminID",
                table: "Doctors",
                type: "int",
                nullable: true,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AdminID",
                table: "Clerks",
                type: "int",
                nullable: true,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AdminID",
                table: "CasualUsers",
                type: "int",
                nullable: true,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CasualUsers_Admins_AdminID",
                table: "CasualUsers",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Clerks_Admins_AdminID",
                table: "Clerks",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Admins_AdminID",
                table: "Doctors",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CasualUsers_Admins_AdminID",
                table: "CasualUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Clerks_Admins_AdminID",
                table: "Clerks");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Admins_AdminID",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "AdminID",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "AdminID",
                table: "Clerks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "AdminID",
                table: "CasualUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_CasualUsers_Admins_AdminID",
                table: "CasualUsers",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clerks_Admins_AdminID",
                table: "Clerks",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Admins_AdminID",
                table: "Doctors",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
