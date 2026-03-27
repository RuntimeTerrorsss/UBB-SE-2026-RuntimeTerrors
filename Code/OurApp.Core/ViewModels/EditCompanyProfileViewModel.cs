using CommunityToolkit.Mvvm.ComponentModel;
using OurApp.Core.Models;
using OurApp.Core.Services;
using OurApp.Core.Validators;
using System;

namespace OurApp.Core.ViewModels;

/// <summary>
/// Form state for the company edit-profile page. Saves through <see cref="ICompanyService.UpdateCompany"/>.
/// </summary>
public partial class EditCompanyProfileViewModel : ObservableObject
{
    private readonly ICompanyService _companyService;
    private readonly CompanyValidator _validator = new();

    [ObservableProperty]
    private int _companyId;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private string _aboutUs = "";

    [ObservableProperty]
    public string profilePicturePath = "";

    [ObservableProperty]
    public string companyLogoPath = "";

    [ObservableProperty]
    private string _location = "";

    [ObservableProperty]
    private string _email = "";

    [ObservableProperty]
    private string _buddyName = "";

    [ObservableProperty]
    private string _finalQuote = "";

    [ObservableProperty]
    private string _scenario1Text = "";

    [ObservableProperty]
    private string _scenario1Answer1 = "";

    [ObservableProperty]
    private string _scenario1Answer2 = "";

    [ObservableProperty]
    private string _scenario1Answer3 = "";

    [ObservableProperty]
    private string _scenario1Reaction1 = "";

    [ObservableProperty]
    private string _scenario1Reaction2 = "";

    [ObservableProperty]
    private string _scenario1Reaction3 = "";

    [ObservableProperty]
    private string _scenario2Text = "";

    [ObservableProperty]
    private string _scenario2Answer1 = "";

    [ObservableProperty]
    private string _scenario2Answer2 = "";

    [ObservableProperty]
    private string _scenario2Answer3 = "";

    [ObservableProperty]
    private string _scenario2Reaction1 = "";

    [ObservableProperty]
    private string _scenario2Reaction2 = "";

    [ObservableProperty]
    private string _scenario2Reaction3 = "";

    [ObservableProperty]
    private string _statusMessage = "";

    public EditCompanyProfileViewModel(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public void Load(int companyId)
    {
        CompanyId = companyId;
        StatusMessage = "";
        var c = _companyService.GetCompanyById(companyId);
        if (c is null)
        {
            StatusMessage = "Company not found.";
            return;
        }

        Name = c.Name;
        AboutUs = c.AboutUs;
        ProfilePicturePath = c.ProfilePicturePath;
        CompanyLogoPath = c.CompanyLogoPath;
        Location = c.Location;
        Email = c.Email;
    }

    private Company ToCompany(int postedJobs, int collaborators)
    {
        return new Company(
            name: Name,
            aboutus: AboutUs,
            pfpUrl: ProfilePicturePath,
            logoUrl: CompanyLogoPath,
            location: Location,
            email: Email,
            companyId: CompanyId,
            postedJobsCount: postedJobs,
            collaboratorsCount: collaborators);
    }

    /// <summary>Validates and persists. Returns null on success, or an error message.</summary>
    public string? TrySave()
    {
        StatusMessage = "";
        var existing = _companyService.GetCompanyById(CompanyId);
        var posted = existing?.PostedJobsCount ?? 0;
        var collab = existing?.CollaboratorsCount ?? 0;
        var copy = existing?.Collaborators ?? new System.Collections.Generic.List<string>();

        try
        {
            _validator.NameValidator(Name);
            _validator.AboutUsValidator(AboutUs);
            _validator.PfpValidator(ProfilePicturePath);
            _validator.LogoValidator(CompanyLogoPath);
            _validator.LocationValidator(Location);
            _validator.EmailValidator(Email);

            var updated = ToCompany(posted, collab);
            updated.Collaborators = copy;
            _companyService.UpdateCompany(updated);
            return null;
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            return ex.Message;
        }
    }
}
