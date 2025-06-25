using StateMachine;

public class GenericWorkflowStateMachine
{
    private readonly Dictionary<string, GenericStep> _steps = new();
    private readonly ProcessState _processState = new();

    /// <summary>
    /// Add a step to the workflow
    /// </summary>
    public void AddStep(GenericStep step)
    {
        if (_steps.ContainsKey(step.Id))
            throw new InvalidOperationException($"Step with ID '{step.Id}' already exists.");

        _steps[step.Id] = step;
    }

    /// <summary>
    /// Execute a step by its ID
    /// </summary>
    public async Task ExecuteStepAsync(string stepId)
    {
        if (!_steps.TryGetValue(stepId, out var step))
            throw new KeyNotFoundException($"No step found with ID '{stepId}'.");

        if (!CanExecuteStep(step))
            throw new InvalidStateTransitionException($"Cannot execute step '{stepId}' due to unmet prerequisites.");

        _processState.FileStatus = StepStatus.InProgress; // Example of updating state
        await step.ExecuteAsync();
        _processState.FileStatus = StepStatus.OK; // Update status after execution
    }

    private bool CanExecuteStep(GenericStep step)
    {
        return step.Prerequisites.All(p => _steps.TryGetValue(p, out var prereq) && prereq.Status == StepStatus.OK);
    }
}