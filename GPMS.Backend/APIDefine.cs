using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend
{
    public static class APIDefine
    {
        //Default Route
        public static const string API_VERSION_1 = "/api/v1";

        //Authentication
        public static const string AUTHENTICATION_V1 = API_VERSION_1 + "/authentication";
        public static const string AUTHENTICATION_CREDENTIALS_V1 = AUTHENTICATION_V1 + "/credentials";

        //Accounts
        public static const string ACCOUNTS_V1 = API_VERSION_1 + "/accounts";
        public static const string ACCOUNTS_ID_V1 = ACCOUNTS_V1 + "/{id}";

        //Departments 
        public static const string DEPARTMENTS_V1 = API_VERSION_1 + "/departments";
        public static const string DEPARTMENTS_ID_V1 = DEPARTMENTS_V1 + "/{id}";
        
        //Staffs
        public static const string STAFFS_V1 = API_VERSION_1 + "/staffs";
        public static const string STAFFS_ID_V1 = STAFFS_V1 + "/{id}";
        public static const string STAFFS_OF_DEPARTMENT_ID_V1 = DEPARTMENTS_ID_V1 + "/staffs";

        //Warehouses
        public static const string WAREHOUSES_V1 = API_VERSION_1 + "/warehouses";
        public static const string WAREHOUSE_ID_V1 = WAREHOUSES_V1 + "/{id}";

        #region Products
        //Products 
        public static const string PRODUCTS_V1 = API_VERSION_1 + "/products";
        public static const string PRODUCTS_ID_V1 = PRODUCTS_V1 + "/{id}";
        
        //Specifications
        public static const string SPECIFICATIONS_OF_PRODUCT_ID_V1 = PRODUCTS_ID_V1 + "/specifications";
        public static const string SPECIFICATION_ID_OF_PRODUCT_ID_v1 = SPECIFICATIONS_OF_PRODUCT_ID_V1 + "/{specificationId}";
        //Bill Of Materials 
        public static const string BILL_OF_MATERIALS_OF_SPECIFICATION_ID_V1 = SPECIFICATION_ID_OF_PRODUCT_ID_v1 + "/bill-of-materials";
        public static const string BILL_OF_MATERIAL_ID_OF_SPECIFICATION_ID_V1 = BILL_OF_MATERIALS_OF_SPECIFICATION_ID_V1 + "/{billOfMaterialId}";
        //Processes 
        public static const string PROCESSES_OF_PRODUCT_ID_V1 = PRODUCTS_ID_V1 + "/processes";
        public static const string PROCESS_ID_OF_PRODUCT_ID_V1 = PROCESSES_OF_PRODUCT_ID_V1 + "/{processId}";
        //Steps 
        public static const string STEPS_OF_PROCESS_ID_V1 = PROCESS_ID_OF_PRODUCT_ID_V1 + "/steps";
        public static const string STEP_ID_OF_PROCESS_ID_V1 = STEPS_OF_PROCESS_ID_V1 + "/{stepId}";

        #endregion Products

        #region Production Plans
        //Production Plans
        public static const string PRODUCTION_PLANS_V1 = API_VERSION_1 + "/production-plans";
        public static const string PRODUCTION_PLANS_ID_V1 = PRODUCTION_PLANS_V1 + "/{id}";
        //Requirements
        public static const string REQUIREMENTS_OF_PRODUCTION_PLAN_ID_V1 = PRODUCTION_PLANS_ID_V1 + "/reqirements";
        public static const string REQUIREMENT_ID_OF_PRODUCTION_PLAN_ID_V1 = REQUIREMENTS_OF_PRODUCTION_PLAN_ID_V1 + "/{requirementId}";
        //Estimations
        public static const string ESTIMATIONS_OF_REQUIREMENT_ID_V1 = REQUIREMENT_ID_OF_PRODUCTION_PLAN_ID_V1 + "/estimations";
        public static const string ESTIMATION_ID_OF_REQUIREMENT_ID_V1 = ESTIMATIONS_OF_PRODUCTION_PLAN_ID_V1 + "/{estimationId}";
        //Series
        public static const string SERIES_OF_ESTIMATION_ID_V1 = ESTIMATION_ID_OF_PRODUCTION_PLAN_ID_V1 + "/series";
        public static const string SERIES_ID_OF_ESTIMATION_ID_V1 = SERIES_OF_ESTIMATION_ID_V1 + "/{seriesId}";
        //Production Process Step Results 
        public static const string PRODUCTION_PROCESS_STEP_RESULTS_OF_SERIES_ID_V1 = API_VERSION_1 + "/production-process-step-results";
        public static const string PRODUCTION_PROCESS_STEP_RESULT_ID_OF_SERIES_ID_V1 = PRODUCTION_PROCESS_STEP_RESULTS_OF_SERIES_ID_V1 + "/{productionProcessStepResultId}";
        //Inspection Requests
        public static const string INSPECTION_REQUESTS_OF_SERIES_ID_V1 = SERIES_ID_OF_ESTIMATION_ID_V1 + "/inspection-requests";
        public static const string INSPECTION_REQUEST_ID_OF_SERIES_ID_V1 = INSPECTION_REQUESTS_OF_SERIES_ID_V1 + "/{inspectionRequestId}";
        //Inspection Reports
        public static const string INSPECTION_REQUEST_RESULTS_OF_INSPECTION_REQUEST_ID_V1 = INSPECTION_REQUEST_ID_OF_SERIES_ID_V1 + "/inspection-request-results";
        public static const string INSPECTION_REUQUEST_RESULT_ID_OF_INSPECTION_REQUEST_ID_V1 = INSPECTION_REQUEST_RESULTS_OF_INSPECTION_REQUEST_ID_V1 + "/{inspectionRequestResultId}";
        //Faulty Products
        public static const string FAULTY_PRODUCTS_OF_INSPECTION_REQUEST_RESULT_ID_V1 = INSPECTION_REUQUEST_RESULT_ID_OF_INSPECTION_REQUEST_ID_V1 + "/faulty-products";
        public static const string FAULTY_PRODUCT_ID_OF_INSPECTION_REQUEST_RESULT_ID_V1 = FAULTY_PRODUCTS_OF_INSPECTION_REQUEST_RESULT_ID_V1 + "/{faultyProductId}";
        //Warehouse Requests
        public static const string WAREHOUSE_REQUESTS_OF_REQUIREMENT_ID_V1 = REQUIREMENT_ID_OF_PRODUCTION_PLAN_ID_V1 + "/warehouse-requests";
        public static const string WAREHOUSE_REQUEST_ID_OF_REQUIREMENT_ID_V1 = WAREHOUSE_REQUESTS_OF_REQUIREMENT_ID_V1 + "/warehouseRequestId";

        #endregion Production Plans
    }
}