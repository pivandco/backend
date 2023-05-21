using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityAccessControl.Migrations
{
    /// <inheritdoc />
    public partial class RuleCascadeDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Areas_AreaId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Groups_GroupId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Passages_PassageId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Subjects_SubjectId",
                table: "Rules");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Areas_AreaId",
                table: "Rules",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Groups_GroupId",
                table: "Rules",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Passages_PassageId",
                table: "Rules",
                column: "PassageId",
                principalTable: "Passages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Subjects_SubjectId",
                table: "Rules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Areas_AreaId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Groups_GroupId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Passages_PassageId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Subjects_SubjectId",
                table: "Rules");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Areas_AreaId",
                table: "Rules",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Groups_GroupId",
                table: "Rules",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Passages_PassageId",
                table: "Rules",
                column: "PassageId",
                principalTable: "Passages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Subjects_SubjectId",
                table: "Rules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }
    }
}
