using StateMachine;

namespace StateMachine
{
    /// <summary>
    /// Examples showing how to configure different workflow scenarios
    /// </summary>
    public static class WorkflowExamples
    {
        /// <summary>
        /// Example: E-commerce order processing workflow
        /// </summary>
        public static GenericWorkflowStateMachine CreateECommerceWorkflow()
        {
            return new WorkflowBuilder()
                // Initial validation
                .AddStep("VALIDATE_CART", "Validate Cart", "Validate cart items and availability")
                .AddStep("CHECK_INVENTORY", "Check Inventory", "Verify product availability", "VALIDATE_CART")

                // Payment processing
                .AddStep("VALIDATE_PAYMENT", "Validate Payment", "Validate payment method", "VALIDATE_CART")
                .AddStep("CHARGE_PAYMENT", "Charge Payment", "Process payment transaction", "VALIDATE_PAYMENT", "CHECK_INVENTORY")

                // Order fulfillment
                .AddStep("CREATE_ORDER", "Create Order", "Create order record", "CHARGE_PAYMENT")
                .AddStep("RESERVE_INVENTORY", "Reserve Inventory", "Reserve products for order", "CREATE_ORDER")
                .AddStep("SHIP_ORDER", "Ship Order", "Prepare and ship order", "RESERVE_INVENTORY")

                // Notifications
                .AddStep("SEND_CONFIRMATION", "Send Confirmation", "Send order confirmation email", "CREATE_ORDER")
                .AddStep("SEND_TRACKING", "Send Tracking", "Send tracking information", "SHIP_ORDER")
                .Build();
        }

        /// <summary>
        /// Example: Software deployment pipeline
        /// </summary>
        public static GenericWorkflowStateMachine CreateDeploymentPipeline()
        {
            return new WorkflowBuilder()
                // Source control
                .AddStep("CHECKOUT", "Checkout Code", "Checkout source code from repository")

                // Build process
                .AddStep("COMPILE", "Compile", "Compile source code", "CHECKOUT")
                .AddStep("UNIT_TESTS", "Unit Tests", "Run unit tests", "COMPILE")
                .AddStep("STATIC_ANALYSIS", "Static Analysis", "Run code quality analysis", "COMPILE")

                // Package and test
                .AddStep("BUILD_PACKAGE", "Build Package", "Create deployment package", "UNIT_TESTS", "STATIC_ANALYSIS")
                .AddStep("INTEGRATION_TESTS", "Integration Tests", "Run integration tests", "BUILD_PACKAGE")

                // Deployment stages
                .AddStep("DEPLOY_STAGING", "Deploy to Staging", "Deploy to staging environment", "INTEGRATION_TESTS")
                .AddStep("SMOKE_TESTS", "Smoke Tests", "Run smoke tests on staging", "DEPLOY_STAGING")
                .AddStep("DEPLOY_PRODUCTION", "Deploy to Production", "Deploy to production environment", "SMOKE_TESTS")

                // Post-deployment
                .AddStep("HEALTH_CHECK", "Health Check", "Verify production deployment", "DEPLOY_PRODUCTION")
                .AddStep("NOTIFY_STAKEHOLDERS", "Notify Stakeholders", "Send deployment notifications", "HEALTH_CHECK")
                .Build();
        }

        /// <summary>
        /// Example: Data processing pipeline
        /// </summary>
        public static GenericWorkflowStateMachine CreateDataProcessingPipeline()
        {
            return new WorkflowBuilder()
                // Data ingestion
                .AddStep("EXTRACT_DATA", "Extract Data", "Extract data from source systems")
                .AddStep("VALIDATE_SCHEMA", "Validate Schema", "Validate data schema and format", "EXTRACT_DATA")

                // Data transformation (parallel branches)
                .AddStep("CLEAN_CUSTOMER_DATA", "Clean Customer Data", "Clean and normalize customer data", "VALIDATE_SCHEMA")
                .AddStep("CLEAN_PRODUCT_DATA", "Clean Product Data", "Clean and normalize product data", "VALIDATE_SCHEMA")
                .AddStep("CLEAN_TRANSACTION_DATA", "Clean Transaction Data", "Clean and normalize transaction data", "VALIDATE_SCHEMA")

                // Data integration
                .AddStep("MERGE_DATA", "Merge Data", "Merge cleaned data sets", "CLEAN_CUSTOMER_DATA", "CLEAN_PRODUCT_DATA", "CLEAN_TRANSACTION_DATA")
                .AddStep("APPLY_BUSINESS_RULES", "Apply Business Rules", "Apply business transformation rules", "MERGE_DATA")

                // Data quality and loading
                .AddStep("QUALITY_CHECK", "Quality Check", "Perform data quality validation", "APPLY_BUSINESS_RULES")
                .AddStep("LOAD_WAREHOUSE", "Load Data Warehouse", "Load data into warehouse", "QUALITY_CHECK")
                .AddStep("UPDATE_INDEXES", "Update Indexes", "Update database indexes and statistics", "LOAD_WAREHOUSE")

                // Reporting
                .AddStep("GENERATE_REPORTS", "Generate Reports", "Generate summary reports", "UPDATE_INDEXES")
                .Build();
        }

        /// <summary>
        /// Example: Your original File -> Fact -> Price workflow with more detailed steps
        /// </summary>
        public static GenericWorkflowStateMachine CreateDetailedFileFactPriceWorkflow()
        {
            return new WorkflowBuilder()
                // File processing steps
                .AddStep("VALIDATE_FILE", "Validate File", "Check file exists and is readable")
                .AddStep("PARSE_FILE", "Parse File", "Parse file content and structure", "VALIDATE_FILE")
                .AddStep("VALIDATE_DATA", "Validate Data", "Validate data integrity and format", "PARSE_FILE")

                // Fact processing steps (depends on file being processed)
                .AddStep("EXTRACT_FACTS", "Extract Facts", "Extract business facts from data", "VALIDATE_DATA")
                .AddStep("VALIDATE_FACTS", "Validate Facts", "Validate extracted facts", "EXTRACT_FACTS")
                .AddStep("ENRICH_FACTS", "Enrich Facts", "Enrich facts with additional data", "VALIDATE_FACTS")

                // Price calculation steps (depends on facts being ready)
                .AddStep("LOAD_PRICING_RULES", "Load Pricing Rules", "Load current pricing rules and rates", "ENRICH_FACTS")
                .AddStep("CALCULATE_BASE_PRICE", "Calculate Base Price", "Calculate base prices", "LOAD_PRICING_RULES")
                .AddStep("APPLY_DISCOUNTS", "Apply Discounts", "Apply applicable discounts", "CALCULATE_BASE_PRICE")
                .AddStep("FINALIZE_PRICING", "Finalize Pricing", "Finalize and validate final prices", "APPLY_DISCOUNTS")
                .Build();
        }

        /// <summary>
        /// Demonstrate how to create a custom workflow programmatically
        /// </summary>
        public static GenericWorkflowStateMachine CreateCustomConfigurableWorkflow(List<(string id, string name, string[] prerequisites)> stepConfigs)
        {
            var builder = new WorkflowBuilder();

            foreach (var (id, name, prerequisites) in stepConfigs)
            {
                builder.AddStep(id, name, $"Auto-generated step: {name}", prerequisites);
            }

            return builder.Build();
        }

        /// <summary>
        /// Example showing how to create workflow from configuration
        /// </summary>
        public static async Task RunConfigurableWorkflowExample()
        {
            Console.WriteLine("\n=== Configurable Workflow Example ===");

            // Define workflow steps with their prerequisites
            var workflowConfig = new List<(string id, string name, string[] prerequisites)>
            {
                ("STEP_A", "Initialize System", new string[] { }),
                ("STEP_B", "Load Configuration", new[] { "STEP_A" }),
                ("STEP_C", "Validate Input", new[] { "STEP_B" }),
                ("STEP_D", "Process Data", new[] { "STEP_C" }),
                ("STEP_E", "Generate Output", new[] { "STEP_D" }),
                ("STEP_F", "Cleanup", new[] { "STEP_E" })
            };

            var workflow = CreateCustomConfigurableWorkflow(workflowConfig);

            Console.WriteLine("Created workflow from configuration:");
            workflow.DisplayCurrentState();

            // Execute all steps in order
            foreach (var (id, _, _) in workflowConfig)
            {
                await workflow.ExecuteStep(id);
            }

            workflow.DisplayCurrentState();
        }
    }
}
