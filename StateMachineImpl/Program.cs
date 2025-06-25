using CommonsLibrary.Dtos.FilingsService;
using StateMachine;

GenericStep stepFile = new GenericStep("FILE", "Import XBRL Filing Files");

GenericStep stepFact = new GenericStep("FACT", "Import Facts from XBRL Filing")
.AddPrerequisite("FILE");

GenericStep stepPrice = new GenericStep("PRICE", "Calculate Prices from Facts")
.AddPrerequisites("FILE", "FACT");



stepFile.SetStatus(FilingStatus.OK);
stepFact.SetStatus(FilingStatus.INIT);
stepPrice.SetStatus(FilingStatus.PENDING);