using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
public class JobPosting
{
    // Identity
    public int JobId { get; set; }
    public int CompanyId { get; set; }

    // Optional media
    public string? Photo { get; set; }

    // Core Job Info
    public string JobTitle { get; set; }
    public string IndustryField { get; set; }
    public string JobType { get; set; }
    public string ExperienceLevel { get; set; }

    public string JobDescription { get; set; }
    public string JobLocation { get; set; }
    public int AvailablePositions { get; set; }

    // Dates
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? PostedAt { get; set; }
    public DateTime? Deadline { get; set; }

    // Financials
    public int? Salary { get; set; }
    public int? AmountPayed { get; set; }

    // Scheduling
    public DateTime? ScheduledAt { get; set; }

    // Skills
    public List<SkillRequirement> RequiredSkills { get; set; } = new();

    public string StartDateDisplay => StartDate.HasValue ? $"Start: {StartDate.Value:dd/MM/yyyy}" : null;
    public string EndDateDisplay => EndDate.HasValue ? $"End: {EndDate.Value:dd/MM/yyyy}" : null;
    public string DeadlineDisplay => Deadline.HasValue ? $"Deadline: {Deadline.Value:dd/MM/yyyy}" : null;
    public string PostedAtDisplay => PostedAt.HasValue ? $"Posted: {PostedAt.Value:g}" : null;

    public string SalaryDisplay => Salary.HasValue ? $"Salary: {Salary.Value}" : null;
    public string AmountPayedDisplay => AmountPayed.HasValue ? $"Paid: {AmountPayed.Value}" : null;

    public string DescriptionDisplay => !string.IsNullOrWhiteSpace(JobDescription)
        ? $"Description: {JobDescription}"
        : null;

    public string JobTitleDisplay => $"Title: {JobTitle}";
    public string IndustryFieldDisplay => $"Field: {IndustryField}";
    public string JobTypeDisplay => $"Type: {JobType}";
    public string ExperienceLevelDisplay => $"Experience: {ExperienceLevel}";
    public string JobLocationDisplay => $"Location: {JobLocation}";
    public string PositionsDisplay => $"Positions: {AvailablePositions}";
    public string SkillsDisplay => RequiredSkills.Count > 0
        ? string.Join(", ", RequiredSkills.Select(s => $"{s.SkillName} ({s.Percentage}%)"))
        : "No skills";
}