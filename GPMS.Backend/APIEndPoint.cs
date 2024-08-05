using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend
{
    public static class APIEndPoint
    {
        //Default Route
        public const string API_VERSION_1 = "/api/v1";
        public const string CREATE_PRODUCTION_PLAN = "/create-production-plan";
        public const string FILTER = "/filter";
        public const string APPROVE = "/approve";
        public const string DECLINE = "/decline";

        //Authentication
        public const string AUTH_V1 = API_VERSION_1 + "/auth";
        public const string AUTH_SIGN_IN_V1 = AUTH_V1 + "/sign-in";
        public const string AUTH_SIGN_IN_WITH_TOKEN_V1 = AUTH_V1 + "/sign-in-with-token";

        //Accounts
        public const string ACCOUNTS_V1 = API_VERSION_1 + "/accounts";
        public const string ACCOUNTS_ID_V1 = ACCOUNTS_V1 + "/{id}";

        //Categories
        public const string CATEGORY_V1 = API_VERSION_1 + "/categories";
        public const string CATEGORY_ID_V1 = CATEGORY_V1 + "/{id}";

        //Materials
        public const string MATERIAL_V1 = API_VERSION_1 + "/materials";
        public const string MATERIAL_ID_V1 = MATERIAL_V1 + "/{id}";

        //Departments 
        public const string DEPARTMENTS_V1 = API_VERSION_1 + "/departments";
        public const string DEPARTMENTS_ID_V1 = DEPARTMENTS_V1 + "/{id}";

        //Staffs
        public const string STAFFS_V1 = API_VERSION_1 + "/staffs";
        public const string STAFFS_ID_V1 = STAFFS_V1 + "/{id}";
        public const string STAFFS_OF_DEPARTMENT_ID_V1 = DEPARTMENTS_ID_V1 + "/staffs";

        //Warehouses
        public const string WAREHOUSES_V1 = API_VERSION_1 + "/warehouses";
        public const string WAREHOUSE_ID_V1 = WAREHOUSES_V1 + "/{id}";

        #region Products
        //Products 
        public const string PRODUCTS_V1 = API_VERSION_1 + "/products";
        public const string PRODUCTS_ID_V1 = PRODUCTS_V1 + "/{id}";
        public const string PRODUCTS_ID_CREATE_PRODUCTION_PLAN_V1 = PRODUCTS_V1 + "/{id}" + CREATE_PRODUCTION_PLAN;


        //Specifications
        public const string SPECIFICATIONS_OF_PRODUCT_ID_V1 = PRODUCTS_ID_V1 + "/specifications";
        public const string SPECIFICATION_ID_OF_PRODUCT_ID_v1 = SPECIFICATIONS_OF_PRODUCT_ID_V1 + "/{specificationId}";
        public const string SPECIFICATIONS_OF_PRODUCT_ID_CREATE_PRODUCTION_PLAN_V1 = SPECIFICATIONS_OF_PRODUCT_ID_V1 + "/{specificationId}";
        public const string SPECIFICATIONS_V1 = API_VERSION_1 + "/specifications";
        public const string SPECIFICATIONS_ID_V1 = API_VERSION_1 + "/specifications" + "/{id}";
        //Measurements
        public const string MEASUREMENT_V1 = API_VERSION_1 + "/measurements";
        public const string MEASUREMENT_OF_PRODUCT_ID_V1 = SPECIFICATIONS_ID_V1 + "/measurements";

        //Bill Of Materials 
        public const string BILL_OF_MATERIALS_OF_SPECIFICATION_ID_V1 = SPECIFICATIONS_ID_V1 + "/bill-of-materials";
        public const string BILL_OF_MATERIAL_ID_OF_SPECIFICATION_ID_V1 = BILL_OF_MATERIALS_OF_SPECIFICATION_ID_V1 + "/{billOfMaterialId}";
        /*public const string BILL_OF_MATERIALS_ID_OF_SPECIFICATION_ID_V1 = */
        //Processes 
        public const string PROCESSES_OF_PRODUCT_ID_V1 = PRODUCTS_ID_V1 + "/processes";
        public const string PROCESS_ID_OF_PRODUCT_ID_V1 = PROCESSES_OF_PRODUCT_ID_V1 + "/{processId}";
        public const string PROCESS_V1 = API_VERSION_1 + "/processes";
        public const string PROCESS_ID_V1 = PROCESS_V1 + "/{id}";
        //Steps 
        public const string STEPS_OF_PROCESS_ID_V1 = PROCESS_ID_OF_PRODUCT_ID_V1 + "/steps";
        public const string STEP_ID_OF_PROCESS_ID_V1 = STEPS_OF_PROCESS_ID_V1 + "/{stepId}";
        public const string STEPS_V1 = API_VERSION_1 + "/steps";
        public const string STEPS_OF_PROCESS_ID_OF_PROCESS_V1 = PROCESS_ID_V1 + "/steps";
        //StepInputOutput
        public const string STEP_INPUT_OUTPUT_V1 = API_VERSION_1 + "/step-input-outputs";
        public const string STEP_INPUT_OUTPUT_OF_STEP_ID_V1 = STEPS_V1 + "/{id}" + "/step-input-outputs";
        //SemiFinishedProduct
        public const string SEMI_FINISHED_PRODUCT_V1 = API_VERSION_1 + "/semi-finished-products";
        public const string SEMI_FINISHED_PRODUCT_OF_PRODUCT_ID_V1 = PRODUCTS_ID_V1 + "/semi-finished-products";
        #endregion Products

        //QualityStandard
        public const string QUALITY_STANDARD_V1 = API_VERSION_1 + "/quality-standards";
        public const string QUALITY_STANDARD_OF_SPECIFICATION_V1 = SPECIFICATIONS_ID_V1 + "/quality-standards";
        #region Production Plans
        //Production Plans
        public const string PRODUCTION_PLANS_V1 = API_VERSION_1 + "/production-plans";
        public const string PRODUCTION_PLANS_ID_V1 = PRODUCTION_PLANS_V1 + "/{id}";
        public const string PRODUCTION_PLANS_ID_V1_START = PRODUCTION_PLANS_ID_V1 + "/start";
        //Requirements
        public const string REQUIREMENTS_OF_PRODUCTION_PLAN_ID_V1 = PRODUCTION_PLANS_ID_V1 + "/reqirements";
        public const string REQUIREMENT_ID_OF_PRODUCTION_PLAN_ID_V1 = REQUIREMENTS_OF_PRODUCTION_PLAN_ID_V1 + "/{requirementId}";
        //Estimations
        public const string ESTIMATIONS_OF_REQUIREMENT_ID_V1 = REQUIREMENT_ID_OF_PRODUCTION_PLAN_ID_V1 + "/estimations";
        public const string ESTIMATION_ID_OF_REQUIREMENT_ID_V1 = ESTIMATIONS_OF_REQUIREMENT_ID_V1 + "/{estimationId}";
        //Series
        public const string SERIES_OF_ESTIMATION_ID_V1 = ESTIMATION_ID_OF_REQUIREMENT_ID_V1 + "/series";
        public const string SERIES_ID_OF_ESTIMATION_ID_V1 = SERIES_OF_ESTIMATION_ID_V1 + "/{seriesId}";
        //Production Process Step Results 
        public const string PRODUCTION_PROCESS_STEP_RESULTS_OF_SERIES_ID_V1 = API_VERSION_1 + "/production-process-step-results";
        public const string PRODUCTION_PROCESS_STEP_RESULT_ID_OF_SERIES_ID_V1 = PRODUCTION_PROCESS_STEP_RESULTS_OF_SERIES_ID_V1 + "/{productionProcessStepResultId}";
        //Inspection Requests
        public const string INSPECTION_REQUESTS_OF_SERIES_ID_V1 = SERIES_ID_OF_ESTIMATION_ID_V1 + "/inspection-requests";
        public const string INSPECTION_REQUEST_ID_OF_SERIES_ID_V1 = INSPECTION_REQUESTS_OF_SERIES_ID_V1 + "/{inspectionRequestId}";
        //Inspection Request Result
        public const string INSPECTION_REQUEST_RESULTS_OF_INSPECTION_REQUEST_ID_V1 = INSPECTION_REQUEST_ID_OF_SERIES_ID_V1 + "/inspection-request-results";
        public const string INSPECTION_REUQUEST_RESULT_ID_OF_INSPECTION_REQUEST_ID_V1 = INSPECTION_REQUEST_RESULTS_OF_INSPECTION_REQUEST_ID_V1 + "/{inspectionRequestResultId}";
        //Faulty Products
        public const string FAULTY_PRODUCTS_OF_INSPECTION_REQUEST_RESULT_ID_V1 = INSPECTION_REUQUEST_RESULT_ID_OF_INSPECTION_REQUEST_ID_V1 + "/faulty-products";
        public const string FAULTY_PRODUCT_ID_OF_INSPECTION_REQUEST_RESULT_ID_V1 = FAULTY_PRODUCTS_OF_INSPECTION_REQUEST_RESULT_ID_V1 + "/{faultyProductId}";
        //Warehouse Requests
        public const string WAREHOUSE_REQUESTS_OF_REQUIREMENT_ID_V1 = API_VERSION_1 + "/warehouse-requests";
        public const string WAREHOUSE_REQUEST_ID_OF_REQUIREMENT_ID_V1 =  WAREHOUSE_REQUESTS_OF_REQUIREMENT_ID_V1 + "/{id}";
        public const string WAREHOUSE_REQUESTS_V1 = API_VERSION_1 + "/warehouse-requests";

        #endregion Production Plans

        //Inspection Requests
        public const string INSPECTION_REQUESTS_V1 = API_VERSION_1 + "/inspection-requests";
        public const string INSPECTION_REQUEST_ID_V1 = INSPECTION_REQUESTS_V1 + "/{id}";
    }
}