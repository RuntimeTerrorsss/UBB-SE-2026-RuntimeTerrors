using iss_project.UI.ViewModels.Jobs;
using System;
using System.Collections.Generic;
using iss_project.Code.OurApp.Core.ViewModels.Jobs;

namespace iss_project.UI.Validators
{
    public class JobValidator
    {
        public List<string> Validate(CreateJobViewModel model)
        {
            var errors = new List<string>();

            // Required text fields
            if (string.IsNullOrWhiteSpace(model.JobTitle))
                errors.Add("Job title is required.");

            if (string.IsNullOrWhiteSpace(model.IndustryField))
                errors.Add("Industry field is required.");

            if (string.IsNullOrWhiteSpace(model.JobType))
                errors.Add("Job type is required.");

            if (string.IsNullOrWhiteSpace(model.ExperienceLevel))
                errors.Add("Experience level is required.");

            if (string.IsNullOrWhiteSpace(model.JobDescription))
                errors.Add("Job description is required.");

            if (string.IsNullOrWhiteSpace(model.JobLocation))
                errors.Add("Job location is required.");

            // Numeric validation
            if (model.AvailablePositions <= 0)
                errors.Add("Available positions must be greater than 0.");

            // Dropdown validation (ensure selected value is valid)
            if (!model.JobTypes.Contains(model.JobType))
                errors.Add("Invalid job type selected.");

            if (!model.ExperienceLevels.Contains(model.ExperienceLevel))
                errors.Add("Invalid experience level selected.");

            // Date validation
            if (model.StartDate.HasValue && model.EndDate.HasValue)
            {
                if (model.EndDate <= model.StartDate)
                    errors.Add("End date must be after start date.");
            }

            if (model.Deadline.HasValue)
            {
                if (model.Deadline < DateTimeOffset.Now)
                    errors.Add("Deadline cannot be in the past.");
            }

            if (model.StartDate.HasValue)
            {
                if (model.StartDate < DateTimeOffset.Now)
                    errors.Add("Start date must be in the future.");
            }

            // Financial validation
            if (model.Salary.HasValue && model.Salary < 0)
                errors.Add("Salary cannot be negative.");

            if (model.AmountPayed.HasValue && model.AmountPayed < 0)
                errors.Add("Amount paid cannot be negative.");

            if (model.Salary.HasValue && model.AmountPayed.HasValue)
            {
                if (model.AmountPayed > model.Salary)
                    errors.Add("Amount paid cannot exceed salary.");
            }

            if (model.UseAutomaticExpiration)
            {
                if (!model.ExpirationDays.HasValue || model.ExpirationDays <= 0)
                    errors.Add("Expiration days must be greater than 0 when automatic expiration is enabled.");
            }

            return errors;
        }

        public List<string> Validate(EditJobViewModel model)
        {
            var errors = new List<string>();

            // Required text fields
            if (string.IsNullOrWhiteSpace(model.Job.JobTitle))
                errors.Add("Job title is required.");

            if (string.IsNullOrWhiteSpace(model.Job.IndustryField))
                errors.Add("Industry field is required.");

            if (string.IsNullOrWhiteSpace(model.JobType))
                errors.Add("Job type is required.");

            if (string.IsNullOrWhiteSpace(model.ExperienceLevel))
                errors.Add("Experience level is required.");

            if (string.IsNullOrWhiteSpace(model.Job.JobDescription))
                errors.Add("Job description is required.");

            if (string.IsNullOrWhiteSpace(model.Job.JobLocation))
                errors.Add("Job location is required.");

            // Numeric validation
            if (model.Job.AvailablePositions <= 0)
                errors.Add("Available positions must be greater than 0.");

            // Dropdown validation (ensure selected value is valid)
            if (!model.JobTypes.Contains(model.JobType))
                errors.Add("Invalid job type selected.");

            if (!model.ExperienceLevels.Contains(model.ExperienceLevel))
                errors.Add("Invalid experience level selected.");

            // Date validation
            if (model.Job.StartDate.HasValue && model.Job.EndDate.HasValue)
            {
                if (model.Job.EndDate <= model.Job.StartDate)
                    errors.Add("End date must be after start date.");
            }

            if (model.Job.Deadline.HasValue)
            {
                if (model.Job.Deadline < DateTimeOffset.Now)
                    errors.Add("Deadline cannot be in the past.");
            }

            if (model.Job.StartDate.HasValue)
            {
                if (model.Job.StartDate < DateTimeOffset.Now)
                    errors.Add("Start date must be in the future.");
            }

            // Financial validation
            if (model.Job.Salary.HasValue && model.Job.Salary < 0)
                errors.Add("Salary cannot be negative.");

            if (model.Job.AmountPayed.HasValue && model.Job.AmountPayed < 0)
                errors.Add("Amount paid cannot be negative.");

            if (model.Job.Salary.HasValue && model.Job.AmountPayed.HasValue)
            {
                if (model.Job.AmountPayed > model.Job.Salary)
                    errors.Add("Amount paid cannot exceed salary.");
            }

            //if (model.UseAutomaticExpiration)
            //{
            //    if (!model.ExpirationDays.HasValue || model.ExpirationDays <= 0)
            //        errors.Add("Expiration days must be greater than 0 when automatic expiration is enabled.");
            //}

            return errors;
        }
    }
}