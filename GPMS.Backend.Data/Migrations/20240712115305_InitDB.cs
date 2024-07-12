using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GPMS.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 72, DateTimeKind.Utc).AddTicks(6710)),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConsumptionUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SizeWidthUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staffs_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Staffs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sizes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Colors = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURLs = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 104, DateTimeKind.Utc).AddTicks(2732)),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_Staffs_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_Staffs_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedStartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStartingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 115, DateTimeKind.Utc).AddTicks(6392)),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentProductionPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionPlan_ProductionPlan_ParentProductionPlanId",
                        column: x => x.ParentProductionPlanId,
                        principalTable: "ProductionPlan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionPlan_Staffs_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionPlan_Staffs_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductProductionProcess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductionProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductProductionProcess_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InventoryQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecification_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductSpecification_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SemiFinishedProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemiFinishedProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemiFinishedProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionProcessStep",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    StandardTime = table.Column<float>(type: "real", nullable: false),
                    OutputPerHour = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductionProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProcessStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProcessStep_ProductProductionProcess_ProductionProcessId",
                        column: x => x.ProductionProcessId,
                        principalTable: "ProductProductionProcess",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BillOfMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SizeWidth = table.Column<float>(type: "real", nullable: false),
                    Consumption = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillOfMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillOfMaterial_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BillOfMaterial_ProductSpecification_ProductSpecificationId",
                        column: x => x.ProductSpecificationId,
                        principalTable: "ProductSpecification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Measurement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Measure = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurement_ProductSpecification_ProductSpecificationId",
                        column: x => x.ProductSpecificationId,
                        principalTable: "ProductSpecification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionRequirement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OvertimeQuantity = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Batch = table.Column<int>(type: "int", nullable: true),
                    Day = table.Column<int>(type: "int", nullable: true),
                    ProductSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionRequirement_ProductSpecification_ProductSpecificationId",
                        column: x => x.ProductSpecificationId,
                        principalTable: "ProductSpecification",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionRequirement_ProductionPlan_ProductionPlanId",
                        column: x => x.ProductionPlanId,
                        principalTable: "ProductionPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QualityStandard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityStandard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityStandard_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QualityStandard_ProductSpecification_ProductSpecificationId",
                        column: x => x.ProductSpecificationId,
                        principalTable: "ProductSpecification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionProcessStepIO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Consumption = table.Column<float>(type: "real", nullable: true),
                    IsProduct = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SemiFinishedProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductionProcessStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProcessStepIO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepIO_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepIO_ProductionProcessStep_ProductionProcessStepId",
                        column: x => x.ProductionProcessStepId,
                        principalTable: "ProductionProcessStep",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepIO_SemiFinishedProduct_SemiFinishedProductId",
                        column: x => x.SemiFinishedProductId,
                        principalTable: "SemiFinishedProduct",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionEstimation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OvertimeQuantity = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Batch = table.Column<int>(type: "int", nullable: true),
                    DayNumber = table.Column<int>(type: "int", nullable: true),
                    ProductionRequirementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionEstimation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionEstimation_ProductionRequirement_ProductionRequirementId",
                        column: x => x.ProductionRequirementId,
                        principalTable: "ProductionRequirement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 146, DateTimeKind.Utc).AddTicks(1088)),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductionRequirementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseRequest_ProductionRequirement_ProductionRequirementId",
                        column: x => x.ProductionRequirementId,
                        principalTable: "ProductionRequirement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseRequest_Staffs_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseRequest_Staffs_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FaultyQuantity = table.Column<int>(type: "int", nullable: true),
                    CurrentProcess = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProductionEstimationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionSeries_ProductionEstimation_ProductionEstimationId",
                        column: x => x.ProductionEstimationId,
                        principalTable: "ProductionEstimation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTicket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 151, DateTimeKind.Utc).AddTicks(6917)),
                    Type = table.Column<int>(type: "int", nullable: false),
                    WarehouseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseTicket_ProductSpecification_ProductSpecificationId",
                        column: x => x.ProductSpecificationId,
                        principalTable: "ProductSpecification",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseTicket_WarehouseRequest_WarehouseRequestId",
                        column: x => x.WarehouseRequestId,
                        principalTable: "WarehouseRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseTicket_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InspectionRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 84, DateTimeKind.Utc).AddTicks(9397)),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductionSeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionRequest_ProductionSeries_ProductionSeriesId",
                        column: x => x.ProductionSeriesId,
                        principalTable: "ProductionSeries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InspectionRequest_Staffs_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InspectionRequest_Staffs_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InspectionRequestResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FaultyQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 95, DateTimeKind.Utc).AddTicks(2283)),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionRequestResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionRequestResult_InspectionRequest_InspectionRequestId",
                        column: x => x.InspectionRequestId,
                        principalTable: "InspectionRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InspectionRequestResult_Staffs_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FaultyProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductOrderNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 82, DateTimeKind.Utc).AddTicks(7843)),
                    InspectionRequestResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaultyProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaultyProduct_InspectionRequestResult_InspectionRequestResultId",
                        column: x => x.InspectionRequestResultId,
                        principalTable: "InspectionRequestResult",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FaultyProduct_ProductSpecification_SpecificationId",
                        column: x => x.SpecificationId,
                        principalTable: "ProductSpecification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionProcessStepResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 7, 12, 11, 53, 5, 130, DateTimeKind.Utc).AddTicks(4788)),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionSeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionRequestResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductionProcessStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProcessStepResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepResult_InspectionRequestResult_InspectionRequestResultId",
                        column: x => x.InspectionRequestResultId,
                        principalTable: "InspectionRequestResult",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepResult_ProductionProcessStep_ProductionProcessStepId",
                        column: x => x.ProductionProcessStepId,
                        principalTable: "ProductionProcessStep",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepResult_ProductionSeries_ProductionSeriesId",
                        column: x => x.ProductionSeriesId,
                        principalTable: "ProductionSeries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepResult_Staffs_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Staffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductFault",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FaultyProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QualityStandardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionProcessStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFault", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFault_FaultyProduct_FaultyProductId",
                        column: x => x.FaultyProductId,
                        principalTable: "FaultyProduct",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFault_Measurement_ProductMeasurementId",
                        column: x => x.ProductMeasurementId,
                        principalTable: "Measurement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFault_ProductionProcessStep_ProductionProcessStepId",
                        column: x => x.ProductionProcessStepId,
                        principalTable: "ProductionProcessStep",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFault_QualityStandard_QualityStandardId",
                        column: x => x.QualityStandardId,
                        principalTable: "QualityStandard",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionProcessStepIOResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Consumption = table.Column<float>(type: "real", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    StepResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepIOId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProcessStepIOResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepIOResult_ProductionProcessStepIO_StepIOId",
                        column: x => x.StepIOId,
                        principalTable: "ProductionProcessStepIO",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionProcessStepIOResult_ProductionProcessStepResult_StepResultId",
                        column: x => x.StepResultId,
                        principalTable: "ProductionProcessStepResult",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Code",
                table: "Accounts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillOfMaterial_MaterialId",
                table: "BillOfMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BillOfMaterial_ProductSpecificationId",
                table: "BillOfMaterial",
                column: "ProductSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_FaultyProduct_InspectionRequestResultId",
                table: "FaultyProduct",
                column: "InspectionRequestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_FaultyProduct_SpecificationId",
                table: "FaultyProduct",
                column: "SpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequest_CreatorId",
                table: "InspectionRequest",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequest_ProductionSeriesId",
                table: "InspectionRequest",
                column: "ProductionSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequest_ReviewerId",
                table: "InspectionRequest",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequestResult_Code",
                table: "InspectionRequestResult",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequestResult_CreatorId",
                table: "InspectionRequestResult",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRequestResult_InspectionRequestId",
                table: "InspectionRequestResult",
                column: "InspectionRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Material_Code",
                table: "Material",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_ProductSpecificationId",
                table: "Measurement",
                column: "ProductSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Code",
                table: "Product",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CreatorId",
                table: "Product",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ReviewerId",
                table: "Product",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFault_FaultyProductId",
                table: "ProductFault",
                column: "FaultyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFault_ProductionProcessStepId",
                table: "ProductFault",
                column: "ProductionProcessStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFault_ProductMeasurementId",
                table: "ProductFault",
                column: "ProductMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFault_QualityStandardId",
                table: "ProductFault",
                column: "QualityStandardId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionEstimation_ProductionRequirementId",
                table: "ProductionEstimation",
                column: "ProductionRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlan_Code",
                table: "ProductionPlan",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlan_CreatorId",
                table: "ProductionPlan",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlan_ParentProductionPlanId",
                table: "ProductionPlan",
                column: "ParentProductionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionPlan_ReviewerId",
                table: "ProductionPlan",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStep_Code",
                table: "ProductionProcessStep",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStep_ProductionProcessId",
                table: "ProductionProcessStep",
                column: "ProductionProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepIO_MaterialId",
                table: "ProductionProcessStepIO",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepIO_ProductionProcessStepId",
                table: "ProductionProcessStepIO",
                column: "ProductionProcessStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepIO_SemiFinishedProductId",
                table: "ProductionProcessStepIO",
                column: "SemiFinishedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepIOResult_StepIOId",
                table: "ProductionProcessStepIOResult",
                column: "StepIOId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepIOResult_StepResultId",
                table: "ProductionProcessStepIOResult",
                column: "StepResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepResult_CreatorId",
                table: "ProductionProcessStepResult",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepResult_InspectionRequestResultId",
                table: "ProductionProcessStepResult",
                column: "InspectionRequestResultId",
                unique: true,
                filter: "[InspectionRequestResultId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepResult_ProductionProcessStepId",
                table: "ProductionProcessStepResult",
                column: "ProductionProcessStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProcessStepResult_ProductionSeriesId",
                table: "ProductionProcessStepResult",
                column: "ProductionSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRequirement_ProductionPlanId",
                table: "ProductionRequirement",
                column: "ProductionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRequirement_ProductSpecificationId",
                table: "ProductionRequirement",
                column: "ProductSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionSeries_Code",
                table: "ProductionSeries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionSeries_ProductionEstimationId",
                table: "ProductionSeries",
                column: "ProductionEstimationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductionProcess_Code",
                table: "ProductProductionProcess",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductionProcess_ProductId",
                table: "ProductProductionProcess",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecification_ProductId",
                table: "ProductSpecification",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecification_WarehouseId",
                table: "ProductSpecification",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityStandard_MaterialId",
                table: "QualityStandard",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityStandard_ProductSpecificationId",
                table: "QualityStandard",
                column: "ProductSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SemiFinishedProduct_Code",
                table: "SemiFinishedProduct",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemiFinishedProduct_ProductId",
                table: "SemiFinishedProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_AccountId",
                table: "Staffs",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_Code",
                table: "Staffs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_DepartmentId",
                table: "Staffs",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequest_CreatorId",
                table: "WarehouseRequest",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequest_ProductionRequirementId",
                table: "WarehouseRequest",
                column: "ProductionRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequest_ReviewerId",
                table: "WarehouseRequest",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTicket_ProductSpecificationId",
                table: "WarehouseTicket",
                column: "ProductSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTicket_WarehouseId",
                table: "WarehouseTicket",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTicket_WarehouseRequestId",
                table: "WarehouseTicket",
                column: "WarehouseRequestId",
                unique: true,
                filter: "[WarehouseRequestId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillOfMaterial");

            migrationBuilder.DropTable(
                name: "ProductFault");

            migrationBuilder.DropTable(
                name: "ProductionProcessStepIOResult");

            migrationBuilder.DropTable(
                name: "WarehouseTicket");

            migrationBuilder.DropTable(
                name: "FaultyProduct");

            migrationBuilder.DropTable(
                name: "Measurement");

            migrationBuilder.DropTable(
                name: "QualityStandard");

            migrationBuilder.DropTable(
                name: "ProductionProcessStepIO");

            migrationBuilder.DropTable(
                name: "ProductionProcessStepResult");

            migrationBuilder.DropTable(
                name: "WarehouseRequest");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "SemiFinishedProduct");

            migrationBuilder.DropTable(
                name: "InspectionRequestResult");

            migrationBuilder.DropTable(
                name: "ProductionProcessStep");

            migrationBuilder.DropTable(
                name: "InspectionRequest");

            migrationBuilder.DropTable(
                name: "ProductProductionProcess");

            migrationBuilder.DropTable(
                name: "ProductionSeries");

            migrationBuilder.DropTable(
                name: "ProductionEstimation");

            migrationBuilder.DropTable(
                name: "ProductionRequirement");

            migrationBuilder.DropTable(
                name: "ProductSpecification");

            migrationBuilder.DropTable(
                name: "ProductionPlan");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
