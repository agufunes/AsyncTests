using CommonsLibrary.Dtos.FilingsService;
using StateMachine;

public class GenericWorkflowStateMachine
{
    private readonly Dictionary<string, GenericStep> _steps = new();

    /// <summary>
    /// Add a step to the workflow
    /// </summary>
    public void AddStep(GenericStep step)
    {
        if (_steps.ContainsKey(step.Id))
            throw new InvalidOperationException($"Step with ID '{step.Id}' already exists.");

        _steps[step.Id] = step;
    }

    public (bool, string?) CanExecuteStep(string stepId)
    {
        // Get the step
        if (!_steps.TryGetValue(stepId, out var step))
            return (false, $"No step found with ID '{stepId}'.");
        // Check if the status is not PENDING
        if (step.Status != FilingStatus.PENDING)
            return (false, "Status is not PENDING");
        // Check if the pre requisites are all OK
        if (step.AreAllPrerequisitesOk())
            return (false, "Previous status are not OK");
        return (true,"");
    }
}