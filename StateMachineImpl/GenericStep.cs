using System;
using System.Collections.Generic;
using CommonsLibrary.Dtos.FilingsService;

namespace StateMachine
{
    /// <summary>
    /// Represents a generic step in a process workflow
    /// </summary>
    public class GenericStep
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public FilingStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<string> Prerequisites { get; set; } = new List<string>();
        public Func<GenericStep, Task<bool>>? ExecuteAction { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        public GenericStep(string id, string name)
        {
            Id = id;
            Name = name;
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
        /// Set the status of this step
        /// </summary>
        public void SetStatus(FilingStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Set the execution action for this step
        /// </summary>
        public GenericStep SetExecuteAction(Func<GenericStep, Task<bool>> action)
        {
            ExecuteAction = action;
            return this;
        }

    }
}
