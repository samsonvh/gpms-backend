using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImageURLstospecification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ImageURLs",
                table: "Product");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 567, DateTimeKind.Utc).AddTicks(6220),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 879, DateTimeKind.Utc).AddTicks(225));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 562, DateTimeKind.Utc).AddTicks(3947),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 873, DateTimeKind.Utc).AddTicks(9798));

            migrationBuilder.AddColumn<string>(
                name: "ImageURLs",
                table: "ProductSpecification",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 557, DateTimeKind.Utc).AddTicks(3250),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 869, DateTimeKind.Utc).AddTicks(6723));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 548, DateTimeKind.Utc).AddTicks(6028),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 861, DateTimeKind.Utc).AddTicks(5167));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 541, DateTimeKind.Utc).AddTicks(9387),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 855, DateTimeKind.Utc).AddTicks(3316));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 540, DateTimeKind.Utc).AddTicks(3086),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 853, DateTimeKind.Utc).AddTicks(7565));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 535, DateTimeKind.Utc).AddTicks(839),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 848, DateTimeKind.Utc).AddTicks(6832));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 534, DateTimeKind.Utc).AddTicks(3754),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 848, DateTimeKind.Utc).AddTicks(347));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 533, DateTimeKind.Utc).AddTicks(146),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 846, DateTimeKind.Utc).AddTicks(7413));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionRequirement_ProductionPlan_ProductionPlanId",
                table: "ProductionRequirement",
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

            migrationBuilder.DropColumn(
                name: "ImageURLs",
                table: "ProductSpecification");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 879, DateTimeKind.Utc).AddTicks(225),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 567, DateTimeKind.Utc).AddTicks(6220));

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
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 562, DateTimeKind.Utc).AddTicks(3947));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 869, DateTimeKind.Utc).AddTicks(6723),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 557, DateTimeKind.Utc).AddTicks(3250));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 861, DateTimeKind.Utc).AddTicks(5167),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 548, DateTimeKind.Utc).AddTicks(6028));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 855, DateTimeKind.Utc).AddTicks(3316),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 541, DateTimeKind.Utc).AddTicks(9387));

            migrationBuilder.AddColumn<string>(
                name: "ImageURLs",
                table: "Product",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 853, DateTimeKind.Utc).AddTicks(7565),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 540, DateTimeKind.Utc).AddTicks(3086));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 848, DateTimeKind.Utc).AddTicks(6832),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 535, DateTimeKind.Utc).AddTicks(839));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 848, DateTimeKind.Utc).AddTicks(347),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 534, DateTimeKind.Utc).AddTicks(3754));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 23, 10, 11, 32, 846, DateTimeKind.Utc).AddTicks(7413),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 5, 9, 22, 58, 533, DateTimeKind.Utc).AddTicks(146));

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
    }
}
