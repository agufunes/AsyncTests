using CommonsLibrary.Dtos.FilingsService;
using StateMachine;
using StateMachineImpl;

// Step stepFile = new("FILE", "Import XBRL Filing Files", FilingStatus.OK);

// Step stepFact = new Step("FACT", "Import Facts from XBRL Filing", FilingStatus.PENDING)
// .AddPrerequisite(stepFile);

// Step stepPrice = new Step("PRICE", "Calculate Prices from Facts", FilingStatus.PENDING)
// .AddPrerequisites(stepFile, stepFact);

var workflow = new FilingProcessingWorkflow(FilingStatus.OK, FilingStatus.ERROR, FilingStatus.PENDING);

Console.WriteLine(workflow.CanExecuteStep("FILE", false));
Console.WriteLine(workflow.CanExecuteStep("FACT", false));
Console.WriteLine(workflow.CanExecuteStep("PRICE", false));


Console.WriteLine(workflow.CanExecuteStep("FILE", true));
Console.WriteLine(workflow.CanExecuteStep("FACT", true));
Console.WriteLine(workflow.CanExecuteStep("PRICE", true));