using System;
using System.Collections.Generic;
using CommonsLibrary.Dtos.FilingsService;

namespace StateMachine
{
    /// <summary>
    /// Represents a generic step in a process workflow
    /// </summary>
    public class Step
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public FilingStatus Status { get; private set; }
        public string? ErrorMessage { get; set; }
        public List<Step> Prerequisites { get; private set; } = [];

        public Step(string id, string name, FilingStatus status)
        {
            Id = id;
            Name = name;
            Status = status;
        }

        /// <summary>
        /// Add a prerequisite step by ID
        /// </summary>
        public Step AddPrerequisite(Step step)
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
        public Step AddPrerequisites(params Step[] steps)
        {
            foreach (var stepId in steps)
            {
                AddPrerequisite(stepId);
            }
            return this;
        }

        private bool AreAllPrerequisitesOk()
        {
            return Prerequisites.All(step => step.IsFinished());
        }

        public bool IsFinished()
        {
            // A step is finished if its status is OK or FAILED
            return Status == FilingStatus.OK || Status == FilingStatus.WARNING || Status == FilingStatus.NO_DATA;
        }

        /// <summary>
        /// Check if the step is ready to execute
        /// A step is ready if its status is not OK or if replace is true.
        /// </summary>
        /// <param name="replace"></param>
        /// <returns></returns>
        private bool IsReadyToExecute(bool replace)
        {
            return Status != FilingStatus.OK || replace;
        }


        public (bool, string) CanExecute(bool replace)
        {
            // Check if the status is ready to execute
            if (!IsReadyToExecute(replace))
                return (false, "Step is not ready to execute. Status is not OK or replace is false.");

            // Check if all prerequisites are OK
            if (!AreAllPrerequisitesOk())
                return (false, "Not all prerequisites are OK.");
            return (true, "Step is ready to execute.");
        }
    }
}
