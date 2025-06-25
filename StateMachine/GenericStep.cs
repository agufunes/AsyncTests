using System;
using System.Collections.Generic;

namespace StateMachine
{
    /// <summary>
    /// Represents a generic step in a process workflow
    /// </summary>
    public class GenericStep
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StepStatus Status { get; set; } = StepStatus.NotStarted;
        public string? ErrorMessage { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<string> Prerequisites { get; set; } = new List<string>();
        public Func<GenericStep, Task<bool>>? ExecuteAction { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        public GenericStep(string id, string name, string description = "")
        {
            Id = id;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Add a prerequisite step by ID
        /// </summary>
        public GenericStep AddPrerequisite(string stepId)
        {
            if (!Prerequisites.Contains(stepId))
            {
                Prerequisites.Add(stepId);
            }
            return this;
        }

        /// <summary>
        /// Add multiple prerequisite steps by ID
        /// </summary>
        public GenericStep AddPrerequisites(params string[] stepIds)
        {
            foreach (var stepId in stepIds)
            {
                AddPrerequisite(stepId);
            }
            return this;
        }

        /// <summary>
        /// Set the execution action for this step
        /// </summary>
        public GenericStep SetExecuteAction(Func<GenericStep, Task<bool>> action)
        {
            ExecuteAction = action;
            return this;
        }

        /// <summary>
        /// Reset the step to initial state
        /// </summary>
        public void Reset()
        {
            Status = StepStatus.NotStarted;
            ErrorMessage = null;
            CompletedAt = null;
            Data.Clear();
        }

        public override string ToString()
        {
            return $"{Id}: {Name} ({Status})";
        }
    }
}
