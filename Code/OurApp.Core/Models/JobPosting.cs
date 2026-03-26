using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iss_project.Code.OurApp.Core.Models
{
    public class JobPosting
    {
        // =======================
        // Identity
        // =======================
        public int JobId { get; set; }
        public int CompanyId { get; set; }

        // =======================
        // Optional Media
        // =======================
        public string? Photo { get; set; }

        // =======================
        // Core Job Info
        // =======================
        public string JobTitle { get; set; }
        public string IndustryField { get; set; }
        public string JobType { get; set; }
        public string ExperienceLevel { get; set; }

        public string JobDescription { get; set; }
        public string JobLocation { get; set; }

        public int AvailablePositions { get; set; }

        // =======================
        // Dates
        // =======================
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PostedAt { get; set; }
        public DateTime? Deadline { get; set; }

        // =======================
        // Financials
        // =======================
        public int? Salary { get; set; }
        public int? AmountPayed { get; set; }

        // =======================
        // Scheduling
        // =======================
        public DateTime? ScheduledAt { get; set; }

        // =======================
        // Skills
        // =======================
        public List<(string SkillName, int Percentage)> RequiredSkills { get; set; } = new();

        // =======================
        // Visibility Helpers
        // =======================
        public Visibility SalaryVisibility => Salary.HasValue ? Visibility.Visible : Visibility.Collapsed;
        public Visibility AmountPayedVisibility => AmountPayed.HasValue ? Visibility.Visible : Visibility.Collapsed;
        public Visibility StartDateVisibility => StartDate.HasValue ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EndDateVisibility => EndDate.HasValue ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DeadlineVisibility => Deadline.HasValue ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PostedAtVisibility => PostedAt.HasValue ? Visibility.Visible : Visibility.Collapsed;

        // =======================
        // Display (UI-friendly)
        // =======================
        public string JobTitleDisplay => $"Title: {JobTitle}";
        public string IndustryFieldDisplay => $"Field: {IndustryField}";
        public string JobTypeDisplay => $"Type: {JobType}";
        public string ExperienceLevelDisplay => $"Experience: {ExperienceLevel}";
        public string JobLocationDisplay => $"Location: {JobLocation}";
        public string PositionsDisplay => $"Positions: {AvailablePositions}";

        public string StartDateDisplay => StartDate.HasValue ? $"Start: {StartDate.Value:dd/MM/yyyy}" : "";
        public string EndDateDisplay => EndDate.HasValue ? $"End: {EndDate.Value:dd/MM/yyyy}" : "";
        public string DeadlineDisplay => Deadline.HasValue ? $"Deadline: {Deadline.Value:dd/MM/yyyy}" : "";
        public string PostedAtDisplay => PostedAt.HasValue ? $"Posted: {PostedAt.Value:g}" : "";

        public string SalaryDisplay => Salary.HasValue ? $"Salary: {Salary.Value}" : "";
        public string AmountPayedDisplay => AmountPayed.HasValue ? $"Paid: {AmountPayed.Value}" : "";
        public string DescriptionDisplay => $"Description: {JobDescription}";

        public string SkillsDisplay => RequiredSkills != null && RequiredSkills.Count > 0
            ? string.Join(", ", RequiredSkills.Select(s => $"{s.SkillName} ({s.Percentage}%)"))
            : "No skills";

        public Visibility SkillsVisibility => RequiredSkills != null && RequiredSkills.Count > 0
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}