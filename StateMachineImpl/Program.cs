using CommonsLibrary.Dtos.FilingsService;
using StateMachine;

GenericStep stepFile = new GenericStep("FILE", "Import XBRL Filing Files");

GenericStep stepFact = new GenericStep("FACT", "Import Facts from XBRL Filing")
.AddPrerequisite(stepFile);
GenericStep stepPrice = new GenericStep("PRICE", "Calculate Prices from Facts")
.AddPrerequisites(stepFile, stepFact);


stepFile.SetStatus(FilingStatus.OK);
stepFact.SetStatus(FilingStatus.PENDING);
stepPrice.SetStatus(FilingStatus.PENDING);

GenericWorkflowStateMachine workflow = new();

workflow.AddStep(stepFile);
workflow.AddStep(stepFact);
workflow.AddStep(stepPrice);

Console.WriteLine(workflow.CanExecuteStep("FILE"));
Console.WriteLine(workflow.CanExecuteStep("FACT"));
Console.WriteLine(workflow.CanExecuteStep("PRICE"));