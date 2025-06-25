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
        public List<GenericStep> Prerequisites { get; set; } = [];

        public GenericStep(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Add a prerequisite step by ID
        /// </summary>
        public GenericStep AddPrerequisite(GenericStep step)
        {
            if (Prerequisites.All(s => s.Id != step.Id))
            {
                Prerequisites.Add(step);
            }
            return this;
        }

        /// <summary>
        /// Add multiple prerequisite steps by ID
        /// </summary>
        public GenericStep AddPrerequisites(params GenericStep[] steps)
        {
            foreach (var stepId in steps)
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

        private bool AreAllPrerequisitesOk()
        {
            return Prerequisites.All(s => s.Status == FilingStatus.OK);
        }

    }
}
