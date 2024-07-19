using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteestimationfieldinproductionRequirement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionRequest_ProductionSeries_ProductionSeriesId",
                table: "InspectionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionProcessStepResult_ProductionSeries_ProductionSeriesId",
                table: "ProductionProcessStepResult");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionSeries_ProductionEstimation_ProductionEstimationId",
                table: "ProductionSeries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionSeries",
                table: "ProductionSeries");

            migrationBuilder.DropColumn(
                name: "Batch",
                table: "ProductionRequirement");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "ProductionRequirement");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "ProductionRequirement");

            migrationBuilder.DropColumn(
                name: "OvertimeQuantity",
                table: "ProductionRequirement");

            migrationBuilder.DropColumn(
                name: "Quarter",
                table: "ProductionRequirement");

            migrationBuilder.RenameTable(
                name: "ProductionSeries",
                newName: "ProductionSerie");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionSeries_ProductionEstimationId",
                table: "ProductionSerie",
                newName: "IX_ProductionSerie_ProductionEstimationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionSeries_Code",
                table: "ProductionSerie",
                newName: "IX_ProductionSerie_Code");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 491, DateTimeKind.Utc).AddTicks(9929),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 865, DateTimeKind.Utc).AddTicks(4154));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 485, DateTimeKind.Utc).AddTicks(771),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 857, DateTimeKind.Utc).AddTicks(9513));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 479, DateTimeKind.Utc).AddTicks(2501),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 851, DateTimeKind.Utc).AddTicks(3669));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 463, DateTimeKind.Utc).AddTicks(7441),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 839, DateTimeKind.Utc).AddTicks(5531));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 457, DateTimeKind.Utc).AddTicks(9567),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 831, DateTimeKind.Utc).AddTicks(4069));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 456, DateTimeKind.Utc).AddTicks(4550),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 829, DateTimeKind.Utc).AddTicks(4304));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 451, DateTimeKind.Utc).AddTicks(8220),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 822, DateTimeKind.Utc).AddTicks(9258));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 451, DateTimeKind.Utc).AddTicks(2284),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 821, DateTimeKind.Utc).AddTicks(9701));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 450, DateTimeKind.Utc).AddTicks(720),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 820, DateTimeKind.Utc).AddTicks(2832));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionSerie",
                table: "ProductionSerie",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionRequest_ProductionSerie_ProductionSeriesId",
                table: "InspectionRequest",
                column: "ProductionSeriesId",
                principalTable: "ProductionSerie",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionProcessStepResult_ProductionSerie_ProductionSeriesId",
                table: "ProductionProcessStepResult",
                column: "ProductionSeriesId",
                principalTable: "ProductionSerie",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionSerie_ProductionEstimation_ProductionEstimationId",
                table: "ProductionSerie",
                column: "ProductionEstimationId",
                principalTable: "ProductionEstimation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionRequest_ProductionSerie_ProductionSeriesId",
                table: "InspectionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionProcessStepResult_ProductionSerie_ProductionSeriesId",
                table: "ProductionProcessStepResult");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionSerie_ProductionEstimation_ProductionEstimationId",
                table: "ProductionSerie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionSerie",
                table: "ProductionSerie");

            migrationBuilder.RenameTable(
                name: "ProductionSerie",
                newName: "ProductionSeries");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionSerie_ProductionEstimationId",
                table: "ProductionSeries",
                newName: "IX_ProductionSeries_ProductionEstimationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionSerie_Code",
                table: "ProductionSeries",
                newName: "IX_ProductionSeries_Code");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseTicket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 865, DateTimeKind.Utc).AddTicks(4154),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 491, DateTimeKind.Utc).AddTicks(9929));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "WarehouseRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 857, DateTimeKind.Utc).AddTicks(9513),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 485, DateTimeKind.Utc).AddTicks(771));

            migrationBuilder.AddColumn<int>(
                name: "Batch",
                table: "ProductionRequirement",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "ProductionRequirement",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "ProductionRequirement",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OvertimeQuantity",
                table: "ProductionRequirement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quarter",
                table: "ProductionRequirement",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionProcessStepResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 851, DateTimeKind.Utc).AddTicks(3669),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 479, DateTimeKind.Utc).AddTicks(2501));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 839, DateTimeKind.Utc).AddTicks(5531),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 463, DateTimeKind.Utc).AddTicks(7441));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 831, DateTimeKind.Utc).AddTicks(4069),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 457, DateTimeKind.Utc).AddTicks(9567));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequestResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 829, DateTimeKind.Utc).AddTicks(4304),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 456, DateTimeKind.Utc).AddTicks(4550));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "InspectionRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 822, DateTimeKind.Utc).AddTicks(9258),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 451, DateTimeKind.Utc).AddTicks(8220));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FaultyProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 821, DateTimeKind.Utc).AddTicks(9701),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 451, DateTimeKind.Utc).AddTicks(2284));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 20, 54, 32, 820, DateTimeKind.Utc).AddTicks(2832),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 10, 31, 8, 450, DateTimeKind.Utc).AddTicks(720));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionSeries",
                table: "ProductionSeries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionRequest_ProductionSeries_ProductionSeriesId",
                table: "InspectionRequest",
                column: "ProductionSeriesId",
                principalTable: "ProductionSeries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionProcessStepResult_ProductionSeries_ProductionSeriesId",
                table: "ProductionProcessStepResult",
                column: "ProductionSeriesId",
                principalTable: "ProductionSeries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionSeries_ProductionEstimation_ProductionEstimationId",
                table: "ProductionSeries",
                column: "ProductionEstimationId",
                principalTable: "ProductionEstimation",
                principalColumn: "Id");
        }
    }
}
