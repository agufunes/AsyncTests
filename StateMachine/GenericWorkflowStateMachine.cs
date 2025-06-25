using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateMachine
{
    /// <summary>
    /// Generic workflow state machine that can handle any number of steps with configurable prerequisites
    /// </summary>
    public class GenericWorkflowStateMachine
    {
        private readonly Dictionary<string, GenericStep> _steps;
        private readonly Random _random;

        public GenericWorkflowStateMachine()
        {
            _steps = new Dictionary<string, GenericStep>();
            _random = new Random();
        }

        /// <summary>
        /// Add a step to the workflow
        /// </summary>
        public GenericWorkflowStateMachine AddStep(GenericStep step)
        {
            _steps[step.Id] = step;
            return this;
        }

        /// <summary>
        /// Add a step with basic configuration
        /// </summary>
        public GenericWorkflowStateMachine AddStep(string id, string name, string description = "", params string[] prerequisites)
        {
            var step = new GenericStep(id, name, description);
            step.AddPrerequisites(prerequisites);
            return AddStep(step);
        }

        /// <summary>
        /// Get a step by ID
        /// </summary>
        public GenericStep? GetStep(string stepId)
        {
            return _steps.TryGetValue(stepId, out var step) ? step : null;
        }

        /// <summary>
        /// Get all steps
        /// </summary>
        public IEnumerable<GenericStep> GetAllSteps()
        {
            return _steps.Values;
        }

        /// <summary>
        /// Check if a step can be executed based on its prerequisites
        /// </summary>
        public bool CanExecuteStep(string stepId)
        {
            if (!_steps.TryGetValue(stepId, out var step))
                return false;

            // Step is already completed or in progress
            if (step.Status == StepStatus.OK || step.Status == StepStatus.InProgress)
                return false;

            // Check all prerequisites
            foreach (var prerequisiteId in step.Prerequisites)
            {
                if (!_steps.TryGetValue(prerequisiteId, out var prerequisite))
                {
                    Console.WriteLine($"❌ Warning: Prerequisite step '{prerequisiteId}' not found for step '{stepId}'");
                    return false;
                }

                if (prerequisite.Status != StepStatus.OK)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get a list of steps that can currently be executed
        /// </summary>
        public List<GenericStep> GetExecutableSteps()
        {
            return _steps.Values
                .Where(step => CanExecuteStep(step.Id))
                .OrderBy(step => step.Id)
                .ToList();
        }

        /// <summary>
        /// Get steps that are blocked and why
        /// </summary>
        public Dictionary<GenericStep, List<string>> GetBlockedSteps()
        {
            var blocked = new Dictionary<GenericStep, List<string>>();

            foreach (var step in _steps.Values)
            {
                if (step.Status == StepStatus.OK || step.Status == StepStatus.InProgress)
                    continue;

                var reasons = new List<string>();

                foreach (var prerequisiteId in step.Prerequisites)
                {
                    if (!_steps.TryGetValue(prerequisiteId, out var prerequisite))
                    {
                        reasons.Add($"Prerequisite '{prerequisiteId}' not found");
                        continue;
                    }

                    if (prerequisite.Status != StepStatus.OK)
                    {
                        reasons.Add($"Prerequisite '{prerequisite.Name}' is {prerequisite.Status}");
                    }
                }

                if (reasons.Any())
                {
                    blocked[step] = reasons;
                }
            }

            return blocked;
        }

        /// <summary>
        /// Execute a step by ID
        /// </summary>
        public async Task<bool> ExecuteStep(string stepId, bool simulateFailure = false)
        {
            if (!_steps.TryGetValue(stepId, out var step))
            {
                throw new ArgumentException($"Step '{stepId}' not found");
            }

            Console.WriteLine($"🚀 Starting Step {step.Id}: {step.Name}...");

            // Validate dependencies
            if (!CanExecuteStep(stepId))
            {
                var blockedReasons = GetBlockedSteps();
                if (blockedReasons.TryGetValue(step, out var reasons))
                {
                    var errorMsg = $"Cannot execute step '{step.Name}': {string.Join(", ", reasons)}";
                    Console.WriteLine($"❌ {errorMsg}");
                    throw new InvalidStateTransitionException(errorMsg);
                }
            }

            try
            {
                step.Status = StepStatus.InProgress;
                step.ErrorMessage = null;

                Console.WriteLine($"⚙️ Processing {step.Name}...");
                if (!string.IsNullOrEmpty(step.Description))
                {
                    Console.WriteLine($"   📝 {step.Description}");
                }

                bool success;

                // Use custom execution action if provided, otherwise use default simulation
                if (step.ExecuteAction != null)
                {
                    success = await step.ExecuteAction(step);
                }
                else
                {
                    // Default simulation
                    await Task.Delay(_random.Next(1000, 3000));
                    success = !simulateFailure;
                }

                if (simulateFailure || !success)
                {
                    step.Status = StepStatus.Failed;
                    step.ErrorMessage = $"Step {step.Name} failed during execution";
                    Console.WriteLine($"❌ Step {step.Id} FAILED: {step.ErrorMessage}");
                    return false;
                }

                step.Status = StepStatus.OK;
                step.CompletedAt = DateTime.Now;
                Console.WriteLine($"✅ Step {step.Id} COMPLETED: {step.Name} processed successfully");
                return true;
            }
            catch (Exception ex)
            {
                step.Status = StepStatus.Failed;
                step.ErrorMessage = ex.Message;
                Console.WriteLine($"❌ Step {step.Id} FAILED: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Reset all steps to initial state
        /// </summary>
        public void Reset()
        {
            foreach (var step in _steps.Values)
            {
                step.Reset();
            }
            Console.WriteLine("🔄 Workflow state machine reset to initial state");
        }

        /// <summary>
        /// Check if the entire workflow is completed
        /// </summary>
        public bool IsCompleted()
        {
            return _steps.Values.All(step => step.Status == StepStatus.OK);
        }

        /// <summary>
        /// Check if the workflow has any failures
        /// </summary>
        public bool HasFailures()
        {
            return _steps.Values.Any(step => step.Status == StepStatus.Failed);
        }

        /// <summary>
        /// Display the current state of all steps
        /// </summary>
        public void DisplayCurrentState()
        {
            Console.WriteLine("📊 Current Workflow State:");

            foreach (var step in _steps.Values.OrderBy(s => s.Id))
            {
                var icon = step.Status switch
                {
                    StepStatus.NotStarted => "⏸️",
                    StepStatus.InProgress => "⚙️",
                    StepStatus.OK => "✅",
                    StepStatus.Failed => "❌",
                    _ => "❓"
                };

                Console.WriteLine($"   {icon} {step.Id}: {step.Name} ({step.Status})");

                if (!string.IsNullOrEmpty(step.ErrorMessage))
                {
                    Console.WriteLine($"      🚨 Error: {step.ErrorMessage}");
                }

                if (step.CompletedAt.HasValue)
                {
                    Console.WriteLine($"      ⏰ Completed: {step.CompletedAt:HH:mm:ss}");
                }

                if (step.Prerequisites.Any())
                {
                    Console.WriteLine($"      📋 Prerequisites: {string.Join(", ", step.Prerequisites)}");
                }
            }

            // Show executable steps
            var executable = GetExecutableSteps();
            if (executable.Any())
            {
                Console.WriteLine($"\n🟢 Ready to execute: {string.Join(", ", executable.Select(s => s.Id))}");
            }

            // Show blocked steps
            var blocked = GetBlockedSteps();
            if (blocked.Any())
            {
                Console.WriteLine("\n🔴 Blocked steps:");
                foreach (var kvp in blocked)
                {
                    Console.WriteLine($"   {kvp.Key.Id}: {string.Join(", ", kvp.Value)}");
                }
            }

            if (IsCompleted())
            {
                Console.WriteLine("\n🎉 Workflow completed successfully!");
            }
            else if (HasFailures())
            {
                Console.WriteLine("\n💥 Workflow has failures!");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Run an interactive demo
        /// </summary>
        public async Task RunInteractiveDemo()
        {
            Console.WriteLine("🎮 Interactive Generic Workflow Demo");
            Console.WriteLine("You can execute steps and see how prerequisites control the flow");

            while (!IsCompleted() && !HasFailures())
            {
                DisplayCurrentState();

                var executableSteps = GetExecutableSteps();

                if (executableSteps.Count == 0)
                {
                    Console.WriteLine("❌ No more steps can be executed. Workflow failed or completed.");
                    break;
                }

                Console.WriteLine("Available steps to execute:");
                for (int i = 0; i < executableSteps.Count; i++)
                {
                    var step = executableSteps[i];
                    Console.WriteLine($"  {i + 1} - {step.Name} ({step.Id})");
                }
                Console.WriteLine("  R - Reset workflow");
                Console.WriteLine("  S - Show step details");
                Console.WriteLine("  Q - Quit");

                Console.Write("Choose an option: ");
                var choice = Console.ReadLine()?.ToUpper();

                try
                {
                    if (int.TryParse(choice, out int stepIndex) && stepIndex >= 1 && stepIndex <= executableSteps.Count)
                    {
                        var selectedStep = executableSteps[stepIndex - 1];
                        await ExecuteStep(selectedStep.Id);
                    }
                    else
                    {
                        switch (choice)
                        {
                            case "R":
                                Reset();
                                break;
                            case "S":
                                ShowStepDetails();
                                break;
                            case "Q":
                                return;
                            default:
                                Console.WriteLine("❌ Invalid choice. Please try again.");
                                continue;
                        }
                    }
                }
                catch (InvalidStateTransitionException ex)
                {
                    Console.WriteLine($"❌ State Transition Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Unexpected Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }

            DisplayCurrentState();
        }

        /// <summary>
        /// Show detailed information about all steps
        /// </summary>
        private void ShowStepDetails()
        {
            Console.WriteLine("\n📋 Step Details:");
            foreach (var step in _steps.Values.OrderBy(s => s.Id))
            {
                Console.WriteLine($"\n🔹 Step {step.Id}: {step.Name}");
                Console.WriteLine($"   Status: {step.Status}");
                Console.WriteLine($"   Description: {step.Description}");
                Console.WriteLine($"   Prerequisites: {(step.Prerequisites.Any() ? string.Join(", ", step.Prerequisites) : "None")}");

                if (!string.IsNullOrEmpty(step.ErrorMessage))
                    Console.WriteLine($"   Error: {step.ErrorMessage}");

                if (step.CompletedAt.HasValue)
                    Console.WriteLine($"   Completed: {step.CompletedAt}");
            }
            Console.WriteLine();
        }
    }
}
