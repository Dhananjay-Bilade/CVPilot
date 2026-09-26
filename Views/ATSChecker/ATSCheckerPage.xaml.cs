using CVPilot.Models;
using CVPilot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CVPilot.Views.ATSChecker;

public partial class ATSCheckerPage : ContentPage
{
    private readonly ATSAnalyzerService _analyzerService;

    private readonly JobDescriptionMatcherService
        _jobMatcherService;

    private FileResult? _selectedFile;

    private string _resumeText = string.Empty;

    public ATSCheckerPage()
    {
        InitializeComponent();

        _analyzerService =
            IPlatformApplication.Current!
                .Services
                .GetRequiredService<ATSAnalyzerService>();

        _jobMatcherService =
            IPlatformApplication.Current!
                .Services
                .GetRequiredService<JobDescriptionMatcherService>();
    }

    private async void ChooseResume_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            var result =
                await FilePicker.Default.PickAsync(
                    new PickOptions
                    {
                        PickerTitle =
                            "Select your resume",

                        FileTypes =
                            new FilePickerFileType(
                                new Dictionary<
                                    DevicePlatform,
                                    IEnumerable<string>>
                                {
                                    {
                                        DevicePlatform.Android,
                                        new[]
                                        {
                                            "application/pdf",
                                            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                                        }
                                    },

                                    {
                                        DevicePlatform.WinUI,
                                        new[]
                                        {
                                            ".pdf",
                                            ".docx"
                                        }
                                    }
                                })
                    });

            if (result == null)
                return;

            _selectedFile = result;

            SelectedFileLabel.Text =
                result.FileName;

            AnalyzeButton.IsEnabled = true;

            ResultCard.IsVisible = false;

            JobMatchCard.IsVisible = false;

            MatchButton.IsEnabled = false;

            _resumeText = string.Empty;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                $"Unable to select the resume.\n\n{ex.Message}",
                "OK");
        }
    }

    private async void Analyze_Clicked(
        object sender,
        EventArgs e)
    {
        if (_selectedFile == null)
            return;

        try
        {
            AnalyzeButton.IsEnabled = false;

            AnalyzeButton.Text =
                "Analyzing...";

            ATSAnalysisResult result =
                await _analyzerService.AnalyzeAsync(
                    _selectedFile);

            ScoreLabel.Text =
                $"{result.OverallScore} / 100";

            ResultMessageLabel.Text =
                result.Message;

            ContactScoreLabel.Text =
                $"{result.ContactScore}/15";

            SectionScoreLabel.Text =
                $"{result.SectionScore}/20";

            SkillsScoreLabel.Text =
                $"{result.SkillsScore}/20";

            ExperienceScoreLabel.Text =
                $"{result.ExperienceScore}/15";

            EducationScoreLabel.Text =
                $"{result.EducationScore}/10";

            KeywordScoreLabel.Text =
                $"{result.KeywordScore}/10";

            FormattingScoreLabel.Text =
                $"{result.FormattingScore}/10";

            SkillsLabel.Text =
                result.FoundSkills.Count == 0
                    ? "No common skills detected."
                    : string.Join(
                        ", ",
                        result.FoundSkills);

            RecommendationsLabel.Text =
                result.Recommendations.Count == 0
                    ? "No major recommendations."
                    : "• " +
                      string.Join(
                          "\n• ",
                          result.Recommendations);

            ResultCard.IsVisible = true;

            // Extract the resume text again for
            // job description matching.
            await PrepareResumeTextAsync();

            MatchButton.IsEnabled =
                !string.IsNullOrWhiteSpace(
                    _resumeText) &&
                !string.IsNullOrWhiteSpace(
                    JobDescriptionEditor.Text);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Analysis Error",
                ex.Message,
                "OK");
        }
        finally
        {
            AnalyzeButton.IsEnabled = true;

            AnalyzeButton.Text =
                "Analyze Resume";
        }
    }

    private async Task PrepareResumeTextAsync()
    {
        if (_selectedFile == null)
            return;

        try
        {
            // The ATS analyzer already validates and
            // processes the selected file.
            //
            // For the first matching version,
            // create a lightweight text representation
            // from the file itself.

            await using Stream stream =
                await _selectedFile.OpenReadAsync();

            using MemoryStream memoryStream =
                new MemoryStream();

            await stream.CopyToAsync(
                memoryStream);

            byte[] data =
                memoryStream.ToArray();

            string fileName =
                _selectedFile.FileName
                    .ToLowerInvariant();

            if (fileName.EndsWith(".txt"))
            {
                _resumeText =
                    System.Text.Encoding.UTF8
                        .GetString(data);
            }
            else
            {
                // Use the ATS analyzer's internal
                // analysis through a temporary
                // keyword-friendly representation.
                _resumeText =
                    await ExtractTextForMatchingAsync(
                        data,
                        fileName);
            }
        }
        catch
        {
            _resumeText = string.Empty;
        }
    }

    private async Task<string>
        ExtractTextForMatchingAsync(
            byte[] data,
            string fileName)
    {
        // Re-open the selected file through
        // ATSAnalyzerService-compatible processing.
        //
        // This will be replaced by a shared
        // extraction service in the next cleanup sprint.

        try
        {
            using MemoryStream stream =
                new MemoryStream(data);

            if (fileName.EndsWith(".pdf"))
            {
                return await Task.Run(() =>
                {
                    using var document =
                        new Syncfusion.Pdf.Parsing
                            .PdfLoadedDocument(stream);

                    System.Text.StringBuilder text =
                        new System.Text.StringBuilder();

                    for (int i = 0; i < document.Pages.Count; i++)
                    {
                        Syncfusion.Pdf.PdfPageBase page =
                            document.Pages[i];

                        text.AppendLine(
                            page.ExtractText(true));
                    }

                    return text.ToString();
                });
            }

            if (fileName.EndsWith(".docx"))
            {
                using var archive =
                    new System.IO.Compression.ZipArchive(
                        stream,
                        System.IO.Compression.ZipArchiveMode.Read);

                var entry =
                    archive.GetEntry(
                        "word/document.xml");

                if (entry == null)
                    return string.Empty;

                using var entryStream =
                    entry.Open();

                using var reader =
                    new StreamReader(entryStream);

                string xml =
                    await reader.ReadToEndAsync();

                string text =
                    System.Text.RegularExpressions.Regex.Replace(
                        xml,
                        "<[^>]+>",
                        " ");

                return System.Net.WebUtility
                    .HtmlDecode(text);
            }
        }
        catch
        {
            return string.Empty;
        }

        return string.Empty;
    }

    private async void MatchJob_Clicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
                _resumeText))
        {
            await DisplayAlertAsync(
                "Resume Required",
                "Please analyze your resume first.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(
                JobDescriptionEditor.Text))
        {
            await DisplayAlertAsync(
                "Job Description Required",
                "Please paste the job description first.",
                "OK");

            return;
        }

        try
        {
            MatchButton.IsEnabled = false;

            MatchButton.Text =
                "Matching...";

            JobMatchResult result =
                _jobMatcherService.Analyze(
                    _resumeText,
                    JobDescriptionEditor.Text);

            MatchPercentageLabel.Text =
                $"{result.MatchPercentage}%";

            MatchedKeywordsLabel.Text =
                result.MatchedKeywords.Count == 0
                    ? "No matching keywords found."
                    : "✓ " +
                      string.Join(
                          "\n✓ ",
                          result.MatchedKeywords);

            MissingKeywordsLabel.Text =
                result.MissingKeywords.Count == 0
                    ? "No important keywords missing."
                    : "✗ " +
                      string.Join(
                          "\n✗ ",
                          result.MissingKeywords);

            JobRecommendationsLabel.Text =
                result.Recommendations.Count == 0
                    ? "No major recommendations."
                    : "• " +
                      string.Join(
                          "\n• ",
                          result.Recommendations);

            JobMatchCard.IsVisible = true;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Match Error",
                ex.Message,
                "OK");
        }
        finally
        {
            MatchButton.IsEnabled = true;

            MatchButton.Text =
                "Match Job";
        }
    }
}