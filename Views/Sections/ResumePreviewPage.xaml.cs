using CVPilot.Models;
using CVPilot.Services;
using CVPilot.Services.Pdf;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.Resume;

[QueryProperty(nameof(ResumeId), "ResumeId")]
public partial class ResumePreviewPage : ContentPage
{
    private readonly PdfService _pdfService = new();

    private readonly ResumeDataService _resumeService = 
    IPlatformApplication.Current!
    .Services
    .GetRequiredService<ResumeDataService>();

    //private readonly PdfService _pdfService =
    //IPlatformApplication.Current!
    //.Services
    //.GetRequiredService<PdfService>();

    public int ResumeId { get; set; }
    private double _zoom = 1.0;

    public ResumePreviewPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadResume();

        DeviceDisplay.Current.MainDisplayInfoChanged += MainDisplayInfoChanged;

        UpdateScale();
    }

    private async Task LoadResume()
    {
        ResumeLayout.Children.Clear();

        var resume =
            await _resumeService.GetCompleteResumeAsync(ResumeId);

        if (resume == null)
            return;

        RenderHeader(resume);

        RenderProfessionalSummary(resume);

        RenderTechnicalSkills(resume);

        RenderExperience(resume);

        RenderProjects(resume);

        RenderEducation(resume);

        RenderCertifications(resume);

        RenderAchievements(resume);

        RenderLanguages(resume);

        RenderReferences(resume);
    }

    private void RenderHeader(CompleteResume resume)
    {
        if (resume.PersonalDetails == null)
            return;

        var person = resume.PersonalDetails;

        // Name

        ResumeLayout.Children.Add(new Label
        {
            Text = person.FullName,
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Start,
            TextColor = Colors.Black,
            Margin = new Thickness(0, 0, 0, 2)
        });

        // Professional Title

        if (!string.IsNullOrWhiteSpace(person.ProfessionalTitle))
        {
            ResumeLayout.Children.Add(new Label
            {
                Text = person.ProfessionalTitle,
                FontSize = 15,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Colors.Black,
                Margin = new Thickness(0, 0, 0, 8)
            });
        }

        // Contact Line

        List<string> contact = new();

        string location = "";

        if (!string.IsNullOrWhiteSpace(person.City))
            location += person.City;

        if (!string.IsNullOrWhiteSpace(person.State))
        {
            if (location != "")
                location += ", ";

            location += person.State;
        }

        if (!string.IsNullOrWhiteSpace(location))
            contact.Add(location);

        if (!string.IsNullOrWhiteSpace(person.PhoneNumber))
            contact.Add("+91 " + person.PhoneNumber);

        if (!string.IsNullOrWhiteSpace(person.Email))
            contact.Add("Email: " + person.Email);

        ResumeLayout.Children.Add(new Label
        {
            Text = string.Join(" | ", contact),
            FontSize = 12,
            HorizontalTextAlignment = TextAlignment.Start,
            TextColor = Colors.Black
        });

        // LinkedIn

        if (!string.IsNullOrWhiteSpace(person.LinkedInUrl))
        {
            //ResumeLayout.Children.Add(new Label
            //{
            //    Text = "LinkedIn: " + person.LinkedInUrl,
            //    FontSize = 11,
            //    HorizontalTextAlignment = TextAlignment.Start,
            //    TextColor = Colors.Blue
            //});
            var linkedInLabel = new Label
            {
                FontSize = 11,
                TextColor = Colors.Black
            };

            linkedInLabel.FormattedText = new FormattedString();

            linkedInLabel.FormattedText.Spans.Add(new Span
            {
                Text = "LinkedIn: ",
                TextColor = Colors.Black
            });

            linkedInLabel.FormattedText.Spans.Add(new Span
            {
                Text = person.LinkedInUrl,
                TextColor = Colors.Blue,
                TextDecorations = TextDecorations.Underline
            });

            var tapGesture = new TapGestureRecognizer();

            tapGesture.Tapped += async (s, e) =>
            {
                await Launcher.Default.OpenAsync(person.LinkedInUrl);
            };

            linkedInLabel.GestureRecognizers.Add(tapGesture);

            ResumeLayout.Children.Add(linkedInLabel);
        }

        // GitHub

        if (!string.IsNullOrWhiteSpace(person.GitHubUrl))
        {
            //ResumeLayout.Children.Add(new Label
            //{
            //    Text = "GitHub: " + person.GitHubUrl,
            //    FontSize = 11,
            //    HorizontalTextAlignment = TextAlignment.Start,
            //    TextColor = Colors.Blue
            //});
            var GitHubLabel = new Label
            {
                FontSize = 11,
                TextColor = Colors.Black
            };

            GitHubLabel.FormattedText = new FormattedString();

            GitHubLabel.FormattedText.Spans.Add(new Span
            {
                Text = "GitHub: ",
                TextColor = Colors.Black
            });

            GitHubLabel.FormattedText.Spans.Add(new Span
            {
                Text = person.GitHubUrl,
                TextColor = Colors.Blue,
                TextDecorations = TextDecorations.Underline
            });

            var tapGesture = new TapGestureRecognizer();

            tapGesture.Tapped += async (s, e) =>
            {
                await Launcher.Default.OpenAsync(person.GitHubUrl);
            };

            GitHubLabel.GestureRecognizers.Add(tapGesture);

            ResumeLayout.Children.Add(GitHubLabel);
        }

        // Portfolio

        if (!string.IsNullOrWhiteSpace(person.PortfolioUrl))
        {
            //ResumeLayout.Children.Add(new Label
            //{
            //    Text = "Portfolio: " + person.PortfolioUrl,
            //    FontSize = 11,
            //    HorizontalTextAlignment = TextAlignment.Start,
            //    TextColor = Colors.Blue
            //});
            var PortfolioLabel = new Label
            {
                FontSize = 11,
                TextColor = Colors.Black
            };

            PortfolioLabel.FormattedText = new FormattedString();

            PortfolioLabel.FormattedText.Spans.Add(new Span
            {
                Text = "Portfolio: ",
                TextColor = Colors.Black
            });

            PortfolioLabel.FormattedText.Spans.Add(new Span
            {
                Text = person.PortfolioUrl,
                TextColor = Colors.Blue,
                TextDecorations = TextDecorations.Underline
            });

            var tapGesture = new TapGestureRecognizer();

            tapGesture.Tapped += async (s, e) =>
            {
                await Launcher.Default.OpenAsync(person.PortfolioUrl);
            };

            PortfolioLabel.GestureRecognizers.Add(tapGesture);

            ResumeLayout.Children.Add(PortfolioLabel);
        }

        AddDivider();
    }

    private void RenderProfessionalSummary(CompleteResume resume)
    {
        if (resume.Objective == null)
            return;

        if (string.IsNullOrWhiteSpace(resume.Objective.ObjectiveText))
            return;

        AddSectionHeader("PROFESSIONAL SUMMARY");

        ResumeLayout.Children.Add(new Label
        {
            Text = resume.Objective.ObjectiveText,
            FontSize = 14,
            LineBreakMode = LineBreakMode.WordWrap,
            TextColor = Colors.Black,
            Margin = new Thickness(0, 5, 0, 10)
        });

        AddDivider();
    }

    private void RenderTechnicalSkills(CompleteResume resume)
    {
        if (!resume.Skills.Any())
            return;

        AddSectionHeader("TECHNICAL SKILLS");

        var groupedSkills = resume.Skills
            .GroupBy(s => string.IsNullOrWhiteSpace(s.SkillCategory)
                            ? "Other"
                            : s.SkillCategory);

        foreach (var group in groupedSkills)
        {
            string skills = string.Join(" | ",
                group.Select(x => x.SkillName));

            ResumeLayout.Children.Add(new Label
            {
                FormattedText = new FormattedString
                {
                    Spans =
            {
                new Span
                {
                    Text = group.Key + " : ",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 13,
                    TextColor = Colors.Black
                },

                new Span
                {
                    Text = skills,
                    FontSize = 13,
                    TextColor = Colors.Black
                }
            }
                },

                LineBreakMode = LineBreakMode.WordWrap,
                Margin = new Thickness(0, 2)
            });
        }

        AddDivider();
    }

    private void RenderExperience(CompleteResume resume)
    {
        if (!resume.Experiences.Any())
            return;

        AddSectionHeader("EXPERIENCE");

        foreach (var experience in resume.Experiences)
        {
            AddJobBlock(
                title: experience.JobTitle,
                organization: experience.CompanyName,
                duration: experience.Duration,
                location: experience.Location,
                description: experience.Description);
        }

        AddDivider();
    }

    private void RenderProjects(CompleteResume resume)
    {
        if (!resume.Projects.Any())
            return;

        AddSectionHeader("PROJECTS");

        foreach (var project in resume.Projects)
        {
            AddJobBlock(
                title: project.ProjectName,
                organization: project.Technologies,
                duration: project.Duration,
                location: "",
                description: project.Description);
        }

        AddDivider();
    }


    private void AddJobBlock(
        string title,
        string organization,
        string? duration,
        string? location,
        string? description)
    {
        //-------------------------------------------------
        // First Row
        //-------------------------------------------------

        var header = new Grid
        {
            ColumnDefinitions =
    {
        new ColumnDefinition(GridLength.Star),
        new ColumnDefinition(GridLength.Auto)
    },

            Margin = new Thickness(0, 6, 0, 2)
        };

        string heading = title;

        if (!string.IsNullOrWhiteSpace(organization))
            heading += " | " + organization;

        header.Add(new Label
        {
            Text = heading,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.Black,
            LineBreakMode = LineBreakMode.WordWrap
        }, 0, 0);

        if (!string.IsNullOrWhiteSpace(duration))
        {
            header.Add(new Label
            {
                Text = duration,
                FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Colors.Black
            }, 1, 0);
        }

        ResumeLayout.Children.Add(header);

        //-------------------------------------------------
        // Location
        //-------------------------------------------------

        if (!string.IsNullOrWhiteSpace(location))
        {
            ResumeLayout.Children.Add(new Label
            {
                Text = location,
                FontSize = 12,
                FontAttributes = FontAttributes.Italic,
                TextColor = Colors.Gray,
                Margin = new Thickness(0, 0, 0, 2)
            });
        }

        //-------------------------------------------------
        // Description
        //-------------------------------------------------

        if (!string.IsNullOrWhiteSpace(description))
        {
            var lines = description.Split(
                '\n',
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string bullet = line
                    .Trim()
                    .TrimStart('•', '-', '*')
                    .Trim();

                if (string.IsNullOrWhiteSpace(bullet))
                    continue;

                ResumeLayout.Children.Add(new Label
                {
                    Text = "• " + bullet,
                    FontSize = 12,
                    TextColor = Colors.Black,
                    Margin = new Thickness(15, 1, 0, 1),
                    LineBreakMode = LineBreakMode.WordWrap
                });
            }
        }

        //ResumeLayout.Children.Add(new BoxView
        //{
        //    HeightRequest = 1,
        //    BackgroundColor = Color.FromArgb("#EFEFEF"),
        //    Margin = new Thickness(0, 8)
        //});
    }


    private void RenderEducation(CompleteResume resume)
    {
        if (!resume.Educations.Any())
            return;

        AddSectionHeader("EDUCATION");

        foreach (var education in resume.Educations)
        {
            var details = new List<string>();

            if (!string.IsNullOrWhiteSpace(education.Institute))
                details.Add(education.Institute);

            if (!string.IsNullOrWhiteSpace(education.University))
                details.Add(education.University);

            if (!string.IsNullOrWhiteSpace(education.Grade))
                details.Add("Grade : " + education.Grade);

            string institute = string.Join(Environment.NewLine, details);

            AddJobBlock(
                title: education.Degree,
                organization: institute,
                duration: education.Duration,
                location: education.FieldOfStudy,
                description: education.Description);
        }

        AddDivider();
    }

    private void RenderCertifications(CompleteResume resume)
    {
        if (!resume.Certifications.Any())
            return;

        AddSectionHeader("CERTIFICATIONS");

        foreach (var certification in resume.Certifications)
        {
            AddJobBlock(
                title: certification.CertificationName,
                organization: certification.IssuingOrganization,
                duration: certification.ExpiryText,
                location: "",
                description: certification.CredentialId);
        }

        AddDivider();
    }

    private void RenderAchievements(CompleteResume resume)
    {
        if (!resume.Achievements.Any())
            return;

        AddSectionHeader("ACHIEVEMENTS");

        foreach (var achievement in resume.Achievements)
        {
            ResumeLayout.Children.Add(new Label
            {
                Text = "• " + achievement.Title,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });

            if (!string.IsNullOrWhiteSpace(achievement.Description))
            {
                ResumeLayout.Children.Add(new Label
                {
                    Text = achievement.Description,
                    FontSize = 13,
                    Margin = new Thickness(18, 0, 0, 8),
                    LineBreakMode = LineBreakMode.WordWrap,
                    TextColor = Colors.Black
                });
            }
        }

        AddDivider();
    }

    private void RenderLanguages(CompleteResume resume)
    {
        if (!resume.Languages.Any())
            return;

        AddSectionHeader("LANGUAGES");

        foreach (var language in resume.Languages)
        {
            ResumeLayout.Children.Add(new Label
            {
                Text = $"• {language.LanguageName} ({language.Proficiency})",
                FontSize = 14,
                TextColor = Colors.Black
            });
        }

        AddDivider();
    }

    private void RenderReferences(CompleteResume resume)
    {
        if (!resume.References.Any())
            return;

        AddSectionHeader("REFERENCES");

        foreach (var reference in resume.References)
        {
            ResumeLayout.Children.Add(new Label
            {
                Text = reference.FullName,
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black
            });

            ResumeLayout.Children.Add(new Label
            {
                Text = reference.CompanyName,
                FontSize = 13,
                TextColor = Colors.DarkSlateGray
            });

            ResumeLayout.Children.Add(new Label
            {
                Text = $"{reference.Email} | {reference.PhoneNumber}",
                FontSize = 13,
                TextColor = Colors.Black,
                Margin = new Thickness(0, 0, 0, 10)
            });
        }
    }

    private void AddSectionHeader(string title)
    {
        ResumeLayout.Children.Add(new Label
        {
            Text = title,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.Black,
            Margin = new Thickness(0, 12, 0, 2)
        });

        ResumeLayout.Children.Add(new BoxView
        {
            HeightRequest = 2,
            BackgroundColor = Colors.Black,
            Margin = new Thickness(0, 0, 0, 8),
            HorizontalOptions = LayoutOptions.Fill
        });
    }

    private void AddDivider()
    {
        ResumeLayout.Children.Add(new BoxView
        {
            HeightRequest = 1,
            BackgroundColor = Color.FromArgb("#D6D6D6"),
            //Margin = new Thickness(0, 8, 0, 12),
            Margin = new Thickness(0, 6),
            HorizontalOptions = LayoutOptions.Fill
        });
    }

    private void ZoomIn_Clicked(object sender, EventArgs e)
    {
        if (_zoom >= 2.0)
            return;

        _zoom += 0.10;

        ResumeBorder.Scale = _zoom;

        ZoomLabel.Text = $"{(int)(_zoom * 100)}%";
    }

    private void ZoomOut_Clicked(object sender, EventArgs e)
    {
        if (_zoom <= 0.50)
            return;

        _zoom -= 0.10;

        ResumeBorder.Scale = _zoom;

        ZoomLabel.Text = $"{(int)(_zoom * 100)}%";
    }

    private void MainDisplayInfoChanged(object? sender,
    DisplayInfoChangedEventArgs e)
    {
        UpdateScale();
    }

    private void UpdateScale()
    {
        double screenWidth =
            DeviceDisplay.Current.MainDisplayInfo.Width /
            DeviceDisplay.Current.MainDisplayInfo.Density;

        if (screenWidth < 500)
        {
            _zoom = screenWidth / 794.0;
        }
        else
        {
            _zoom = 1.0;
        }

        ResumeBorder.Scale = _zoom;

        ZoomLabel.Text = $"{(int)(_zoom * 100)}%";
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        DeviceDisplay.Current.MainDisplayInfoChanged -= MainDisplayInfoChanged;
    }

    private async void ExportPdf_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (ResumeId <= 0)
            {
                await DisplayAlertAsync(
                    "Error",
                    "Resume could not be identified.",
                    "OK");

                return;
            }

            var resume =
                await _resumeService.GetCompleteResumeAsync(ResumeId);

            if (resume == null)
            {
                await DisplayAlertAsync(
                    "Error",
                    "Resume data could not be loaded.",
                    "OK");

                return;
            }

            string savedLocation =
                await _pdfService.ExportResumeAsync(resume);

            // Empty result means the user cancelled
            // the Android save dialog.
            if (string.IsNullOrWhiteSpace(savedLocation))
            {
                return;
            }

            await DisplayAlertAsync(
                "PDF Saved",
                "Your resume PDF has been saved successfully.",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Export Error",
                ex.Message,
                "OK");
        }
    }
}
