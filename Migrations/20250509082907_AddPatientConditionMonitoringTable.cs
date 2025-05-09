using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_Administration_System.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientConditionMonitoringTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientConditionMonitoring_Nurses_NurseId",
                table: "PatientConditionMonitoring");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientConditionMonitoring_Patients_PatientId",
                table: "PatientConditionMonitoring");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientConditionMonitoring",
                table: "PatientConditionMonitoring");

            migrationBuilder.RenameTable(
                name: "PatientConditionMonitoring",
                newName: "PatientConditionMonitorings");

            migrationBuilder.RenameIndex(
                name: "IX_PatientConditionMonitoring_PatientId",
                table: "PatientConditionMonitorings",
                newName: "IX_PatientConditionMonitorings_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientConditionMonitoring_NurseId",
                table: "PatientConditionMonitorings",
                newName: "IX_PatientConditionMonitorings_NurseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientConditionMonitorings",
                table: "PatientConditionMonitorings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientConditionMonitorings_Nurses_NurseId",
                table: "PatientConditionMonitorings",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "NurseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientConditionMonitorings_Patients_PatientId",
                table: "PatientConditionMonitorings",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientConditionMonitorings_Nurses_NurseId",
                table: "PatientConditionMonitorings");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientConditionMonitorings_Patients_PatientId",
                table: "PatientConditionMonitorings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientConditionMonitorings",
                table: "PatientConditionMonitorings");

            migrationBuilder.RenameTable(
                name: "PatientConditionMonitorings",
                newName: "PatientConditionMonitoring");

            migrationBuilder.RenameIndex(
                name: "IX_PatientConditionMonitorings_PatientId",
                table: "PatientConditionMonitoring",
                newName: "IX_PatientConditionMonitoring_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientConditionMonitorings_NurseId",
                table: "PatientConditionMonitoring",
                newName: "IX_PatientConditionMonitoring_NurseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientConditionMonitoring",
                table: "PatientConditionMonitoring",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientConditionMonitoring_Nurses_NurseId",
                table: "PatientConditionMonitoring",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "NurseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientConditionMonitoring_Patients_PatientId",
                table: "PatientConditionMonitoring",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
