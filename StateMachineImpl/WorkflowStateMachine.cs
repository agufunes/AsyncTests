using CommonsLibrary.Dtos.FilingsService;
using StateMachine;

namespace StateMachineImpl
{
    public class FilingProcessingWorkflow
    {
        private readonly Dictionary<string, Step> _steps = [];

        public FilingProcessingWorkflow(
            FilingStatus fileStatus,
            FilingStatus factStatus,
            FilingStatus priceStatus)
        {
            // Initialize the workflow with default steps
            AddStep(new Step("FILE", "Import XBRL Filing Files", fileStatus));
            AddStep(new Step("FACT", "Import Facts from XBRL Filing", factStatus)
                .AddPrerequisite(_steps["FILE"]));
            AddStep(new Step("PRICE", "Calculate Prices from Facts", priceStatus)
                .AddPrerequisites(_steps["FILE"], _steps["FACT"]));
        }

        /// <summary>
        /// Add a step to the workflow
        /// </summary>
        private FilingProcessingWorkflow AddStep(Step step)
        {
            if (_steps.ContainsKey(step.Id))
                throw new InvalidOperationException($"Step with ID '{step.Id}' already exists.");

            _steps[step.Id] = step;
            return this;
        }

        public (bool, string) CanExecuteStep(string stepId, bool replace)
        {
            // Get the step
            if (!_steps.TryGetValue(stepId, out var step))
                return (false, $"No step found with ID '{stepId}'.");
            return step.CanExecute(replace);
        }
    }
}