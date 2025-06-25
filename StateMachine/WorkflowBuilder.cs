using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StateMachine
{
    /// <summary>
    /// Builder class to easily configure workflows
    /// </summary>
    public class WorkflowBuilder
    {
        private readonly GenericWorkflowStateMachine _workflow;

        public WorkflowBuilder()
        {
            _workflow = new GenericWorkflowStateMachine();
        }

        /// <summary>
        /// Add a step with prerequisites
        /// </summary>
        public WorkflowBuilder AddStep(string id, string name, string description = "", params string[] prerequisites)
        {
            _workflow.AddStep(id, name, description, prerequisites);
            return this;
        }

        /// <summary>
        /// Add a step with custom execution logic
        /// </summary>
        public WorkflowBuilder AddStep(string id, string name, string description, Func<GenericStep, Task<bool>> executeAction, params string[] prerequisites)
        {
            var step = new GenericStep(id, name, description)
                .AddPrerequisites(prerequisites)
                .SetExecuteAction(executeAction);

            _workflow.AddStep(step);
            return this;
        }

        /// <summary>
        /// Build the workflow
        /// </summary>
        public GenericWorkflowStateMachine Build()
        {
            return _workflow;
        }

        /// <summary>
        /// Create a workflow for the original File -> Fact -> Price scenario
        /// </summary>
        public static GenericWorkflowStateMachine CreateFileFactPriceWorkflow()
        {
            return new WorkflowBuilder()
                .AddStep("FILE", "File Processing", "Process and validate input files")
                .AddStep("FACT", "Fact Processing", "Extract and validate facts from file", "FILE")
                .AddStep("PRICE", "Price Calculation", "Calculate prices based on facts", "FILE", "FACT")
                .Build();
        }

        /// <summary>
        /// Create a more complex workflow example
        /// </summary>
        public static GenericWorkflowStateMachine CreateComplexWorkflow()
        {
            return new WorkflowBuilder()
                // Initial setup steps (no prerequisites)
                .AddStep("INIT", "Initialization", "Initialize system and load configuration")
                .AddStep("AUTH", "Authentication", "Authenticate user and validate permissions")

                // Data loading steps (require initialization)
                .AddStep("LOAD_USER", "Load User Data", "Load user profile and preferences", "INIT", "AUTH")
                .AddStep("LOAD_PRODUCT", "Load Product Data", "Load product catalog and inventory", "INIT")

                // Processing steps (require data loading)
                .AddStep("VALIDATE", "Data Validation", "Validate loaded data for consistency", "LOAD_USER", "LOAD_PRODUCT")
                .AddStep("CALCULATE", "Price Calculation", "Calculate prices and discounts", "VALIDATE")
                .AddStep("APPLY_RULES", "Apply Business Rules", "Apply business rules and policies", "VALIDATE")

                // Final steps (require all processing)
                .AddStep("GENERATE_REPORT", "Generate Report", "Generate final report", "CALCULATE", "APPLY_RULES")
                .AddStep("SEND_NOTIFICATION", "Send Notification", "Send notifications to stakeholders", "GENERATE_REPORT")
                .Build();
        }

        /// <summary>
        /// Create a parallel processing workflow example
        /// </summary>
        public static GenericWorkflowStateMachine CreateParallelWorkflow()
        {
            return new WorkflowBuilder()
                .AddStep("START", "Start Process", "Initialize the parallel processing workflow")

                // Three parallel branches
                .AddStep("BRANCH_A", "Process Branch A", "Process data stream A", "START")
                .AddStep("BRANCH_B", "Process Branch B", "Process data stream B", "START")
                .AddStep("BRANCH_C", "Process Branch C", "Process data stream C", "START")

                // Merge point - requires all branches
                .AddStep("MERGE", "Merge Results", "Combine results from all branches", "BRANCH_A", "BRANCH_B", "BRANCH_C")
                .AddStep("FINALIZE", "Finalize", "Final processing and cleanup", "MERGE")
                .Build();
        }

        /// <summary>
        /// Create a workflow with custom execution logic
        /// </summary>
        public static GenericWorkflowStateMachine CreateCustomWorkflow()
        {
            return new WorkflowBuilder()
                .AddStep("DOWNLOAD", "Download File", "Download file from remote server",
                    async (step) =>
                    {
                        Console.WriteLine("   🌐 Connecting to server...");
                        await Task.Delay(1000);
                        Console.WriteLine("   📥 Downloading file...");
                        await Task.Delay(2000);
                        step.Data["file_size"] = "2.5 MB";
                        step.Data["download_speed"] = "1.2 MB/s";
                        return true;
                    })

                .AddStep("EXTRACT", "Extract Archive", "Extract downloaded archive",
                    async (step) =>
                    {
                        var workflow = step.Data.ContainsKey("file_size") ? "with downloaded file" : "with default file";
                        Console.WriteLine($"   📦 Extracting archive {workflow}...");
                        await Task.Delay(1500);
                        step.Data["extracted_files"] = 42;
                        return true;
                    }, "DOWNLOAD")

                .AddStep("PROCESS", "Process Files", "Process extracted files",
                    async (step) =>
                    {
                        var fileCount = step.Data.ContainsKey("extracted_files") ? step.Data["extracted_files"] : 0;
                        Console.WriteLine($"   ⚙️ Processing {fileCount} files...");
                        await Task.Delay(3000);
                        step.Data["processed_records"] = 1250;
                        return true;
                    }, "EXTRACT")

                .Build();
        }
    }
}
