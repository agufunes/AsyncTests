using StateMachine;

Console.WriteLine("=== Generic State Machine Examples ===\n");

Console.WriteLine("Choose which workflow example to run:");
Console.WriteLine("1. Original File -> Fact -> Price Workflow");
Console.WriteLine("2. Complex Multi-Step Workflow");
Console.WriteLine("3. Parallel Processing Workflow");
Console.WriteLine("4. Custom Execution Logic Workflow");
Console.WriteLine("5. Interactive Demo");
Console.Write("Enter your choice (1-5): ");

var choice = Console.ReadLine();

switch (choice)
{
    case "1":
        await RunOriginalWorkflow();
        break;
    case "2":
        await RunComplexWorkflow();
        break;
    case "3":
        await RunParallelWorkflow();
        break;
    case "4":
        await RunCustomWorkflow();
        break;
    case "5":
        await RunInteractiveDemo();
        break;
    default:
        Console.WriteLine("Invalid choice. Running original workflow...");
        await RunOriginalWorkflow();
        break;
}

// Example 1: Original File -> Fact -> Price workflow
static async Task RunOriginalWorkflow()
{
    Console.WriteLine("\n=== File -> Fact -> Price Workflow ===");

    var workflow = WorkflowBuilder.CreateFileFactPriceWorkflow();

    // Show initial state
    workflow.DisplayCurrentState();

    // Try to execute Fact before File (should fail)
    try
    {
        await workflow.ExecuteStep("FACT");
    }
    catch (InvalidStateTransitionException ex)
    {
        Console.WriteLine($"Expected error: {ex.Message}\n");
    }

    // Execute in correct order
    await workflow.ExecuteStep("FILE");
    await workflow.ExecuteStep("FACT");
    await workflow.ExecuteStep("PRICE");

    workflow.DisplayCurrentState();
}

// Example 2: Complex workflow with multiple dependencies
static async Task RunComplexWorkflow()
{
    Console.WriteLine("\n=== Complex Multi-Step Workflow ===");

    var workflow = WorkflowBuilder.CreateComplexWorkflow();
    workflow.DisplayCurrentState();

    // Execute steps in dependency order
    await workflow.ExecuteStep("INIT");
    await workflow.ExecuteStep("AUTH");

    workflow.DisplayCurrentState();

    // Now we can run parallel data loading
    await workflow.ExecuteStep("LOAD_USER");
    await workflow.ExecuteStep("LOAD_PRODUCT");

    // Continue with processing
    await workflow.ExecuteStep("VALIDATE");
    await workflow.ExecuteStep("CALCULATE");
    await workflow.ExecuteStep("APPLY_RULES");

    // Final steps
    await workflow.ExecuteStep("GENERATE_REPORT");
    await workflow.ExecuteStep("SEND_NOTIFICATION");

    workflow.DisplayCurrentState();
}

// Example 3: Parallel processing workflow
static async Task RunParallelWorkflow()
{
    Console.WriteLine("\n=== Parallel Processing Workflow ===");

    var workflow = WorkflowBuilder.CreateParallelWorkflow();
    workflow.DisplayCurrentState();

    await workflow.ExecuteStep("START");

    // Execute all branches in parallel
    var tasks = new[]
    {
        workflow.ExecuteStep("BRANCH_A"),
        workflow.ExecuteStep("BRANCH_B"),
        workflow.ExecuteStep("BRANCH_C")
    };

    await Task.WhenAll(tasks);

    // Now we can merge
    await workflow.ExecuteStep("MERGE");
    await workflow.ExecuteStep("FINALIZE");

    workflow.DisplayCurrentState();
}

// Example 4: Custom execution logic
static async Task RunCustomWorkflow()
{
    Console.WriteLine("\n=== Custom Execution Logic Workflow ===");

    var workflow = WorkflowBuilder.CreateCustomWorkflow();
    workflow.DisplayCurrentState();

    await workflow.ExecuteStep("DOWNLOAD");
    await workflow.ExecuteStep("EXTRACT");
    await workflow.ExecuteStep("PROCESS");

    workflow.DisplayCurrentState();

    // Show custom data that was stored
    var downloadStep = workflow.GetStep("DOWNLOAD");
    var extractStep = workflow.GetStep("EXTRACT");
    var processStep = workflow.GetStep("PROCESS");

    Console.WriteLine("\n📊 Custom Data Collected:");
    if (downloadStep?.Data.ContainsKey("file_size") == true)
        Console.WriteLine($"   📁 File Size: {downloadStep.Data["file_size"]}");
    if (extractStep?.Data.ContainsKey("extracted_files") == true)
        Console.WriteLine($"   📦 Extracted Files: {extractStep.Data["extracted_files"]}");
    if (processStep?.Data.ContainsKey("processed_records") == true)
        Console.WriteLine($"   ⚙️ Processed Records: {processStep.Data["processed_records"]}");
}

// Example 5: Interactive demo
static async Task RunInteractiveDemo()
{
    Console.WriteLine("\n=== Interactive Demo ===");
    Console.WriteLine("Choose a workflow to interact with:");
    Console.WriteLine("1. File -> Fact -> Price");
    Console.WriteLine("2. Complex Workflow");
    Console.WriteLine("3. Parallel Workflow");
    Console.Write("Choice: ");

    var workflowChoice = Console.ReadLine();
    GenericWorkflowStateMachine workflow = workflowChoice switch
    {
        "1" => WorkflowBuilder.CreateFileFactPriceWorkflow(),
        "2" => WorkflowBuilder.CreateComplexWorkflow(),
        "3" => WorkflowBuilder.CreateParallelWorkflow(),
        _ => WorkflowBuilder.CreateFileFactPriceWorkflow()
    };

    await workflow.RunInteractiveDemo();
}
