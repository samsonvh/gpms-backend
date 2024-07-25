using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Removerelationshipbetweenwarehouserequestandproductionrequirement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionRequirement_ProductionPlan_ProductionPlanId",
                table: "ProductionRequirement");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequest_ProductionRequirement_ProductionRequirementId",
                table: "WarehouseRequest");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseRequest_ProductionRequirementId",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "ProductionRequirementId",
                table: "WarehouseRequest");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 879, DateTimeKind.Utc).AddTicks(225),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 773, DateTimeKind.Utc).AddTicks(9379));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionPlanId",
                table: "WarehouseRequestRequirements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 873, DateTimeKind.Utc).AddTicks(9798),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 769, DateTimeKind.Utc).AddTicks(2425));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 869, DateTimeKind.Utc).AddTicks(6723),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 765, DateTimeKind.Utc).AddTicks(4672));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 861, DateTimeKind.Utc).AddTicks(5167),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 758, DateTimeKind.Utc).AddTicks(1579));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 855, DateTimeKind.Utc).AddTicks(3316),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 753, DateTimeKind.Utc).AddTicks(512));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 853, DateTimeKind.Utc).AddTicks(7565),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 751, DateTimeKind.Utc).AddTicks(8054));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 848, DateTimeKind.Utc).AddTicks(6832),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 747, DateTimeKind.Utc).AddTicks(5845));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 848, DateTimeKind.Utc).AddTicks(347),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 746, DateTimeKind.Utc).AddTicks(9256));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 846, DateTimeKind.Utc).AddTicks(7413),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 744, DateTimeKind.Utc).AddTicks(5451));

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestRequirements_ProductionPlanId",
                table: "WarehouseRequestRequirements",
                column: "ProductionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionRequirement_ProductionPlan_ProductionPlanId",
                table: "ProductionRequirement",
                column: "ProductionPlanId",
                principalTable: "ProductionPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequestRequirements_ProductionPlan_ProductionPlanId",
                table: "WarehouseRequestRequirements",
                column: "ProductionPlanId",
                principalTable: "ProductionPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionRequirement_ProductionPlan_ProductionPlanId",
                table: "ProductionRequirement");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequestRequirements_ProductionPlan_ProductionPlanId",
                table: "WarehouseRequestRequirements");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseRequestRequirements_ProductionPlanId",
                table: "WarehouseRequestRequirements");

            migrationBuilder.DropColumn(
                name: "ProductionPlanId",
                table: "WarehouseRequestRequirements");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 773, DateTimeKind.Utc).AddTicks(9379),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 879, DateTimeKind.Utc).AddTicks(225));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 769, DateTimeKind.Utc).AddTicks(2425),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 873, DateTimeKind.Utc).AddTicks(9798));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionRequirementId",
                table: "WarehouseRequest",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 765, DateTimeKind.Utc).AddTicks(4672),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 869, DateTimeKind.Utc).AddTicks(6723));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 758, DateTimeKind.Utc).AddTicks(1579),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 861, DateTimeKind.Utc).AddTicks(5167));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 753, DateTimeKind.Utc).AddTicks(512),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 855, DateTimeKind.Utc).AddTicks(3316));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 751, DateTimeKind.Utc).AddTicks(8054),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 853, DateTimeKind.Utc).AddTicks(7565));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 747, DateTimeKind.Utc).AddTicks(5845),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 848, DateTimeKind.Utc).AddTicks(6832));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 746, DateTimeKind.Utc).AddTicks(9256),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 848, DateTimeKind.Utc).AddTicks(347));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 22, 8, 11, 36, 744, DateTimeKind.Utc).AddTicks(5451),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 846, DateTimeKind.Utc).AddTicks(7413));

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequest_ProductionRequirementId",
                table: "WarehouseRequest",
                column: "ProductionRequirementId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionRequirement_ProductionPlan_ProductionPlanId",
                table: "ProductionRequirement",
                column: "ProductionPlanId",
                principalTable: "ProductionPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequest_ProductionRequirement_ProductionRequirementId",
                table: "WarehouseRequest",
                column: "ProductionRequirementId",
                principalTable: "ProductionRequirement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
