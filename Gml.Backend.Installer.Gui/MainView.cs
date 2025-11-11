using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace Gml.Backend.Installer.Gui;

public sealed partial class MainView
{
    
    private ObservableCollection<Language> _languages =
    [
        new(new CultureInfo("ru")),
        new(new CultureInfo("en"))
    ];
    
    private string[] _cultureInfoNameSource;
    private string[] _products = new[] { "Gml.Full", "Gml.Api" };
    private CultureInfo[] _cultureInfoSource;
    public CultureInfo CurrentCulture { get; private set; } = new("ru");

    private Label _currentLanguageLabel;
    private Wizard _wizard;
    private ListView _languageListView;

    public MainView()
    {
        InitializeComponent();

        _cultureInfoSource = _languages.Select(c => new CultureInfo(c.Code)).ToArray();
        
        // Build culture sources based on Application.SupportedCultures + Invariant
        _cultureInfoNameSource = _languages
            .Select(c => c.Name)
            .ToArray();

        CreateSteps();
    }

    private void CreateSteps()
    {
        _wizard = new Wizard();

        CreateStepSelectLanguage();
        CreateLicenceAgreement();
        ChoiceComponents();

        
        _wizard.Finished += (sender, args) =>
        {
            MessageBox.Query("Wizard", $"Finished. The Name entered is ''", "Ok");
            Application.RequestStop();
        };
        
        _wizard.GoToStep(_wizard.GetFirstStep());
    }

    private void ChoiceComponents()
    {
        var selectLanguageStep = new WizardStep();
        _wizard.AddStep(selectLanguageStep);
        selectLanguageStep.HelpText = Lang.ChoiceComponents;
        selectLanguageStep.NextButtonText = Lang.Install;
        selectLanguageStep.BackButtonText = Lang.Back;
        
        var items = new ObservableCollection<string>(_products.ToList());
        var products = new ListView
        {
            Source = new ListWrapper<string> (items),
            Height = Dim.Fill (2),
            Width = Dim.Percent (40),
        };
        
        selectLanguageStep.Add(products);
    }

    private void CreateLicenceAgreement()
    {
        // Add 2st step
        var agreementsStep = new WizardStep();
        _wizard.AddStep(agreementsStep);
        agreementsStep.NextButtonText = Lang.Agree;
        agreementsStep.BackButtonText = Lang.Back;
        agreementsStep.HelpText = Lang.NeedAgreeLicenceAgreement;

        // Use a read-only TextView which supports scrolling for long text
        var licenseTextView = new TextView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ReadOnly = true,
            WordWrap = true,
            Text = Lang.Licence
        };

        agreementsStep.Add(licenseTextView);

        Add(_wizard);
    }

    private void CreateStepSelectLanguage()
    {
        var selectLanguageStep = new WizardStep();
        _wizard.AddStep(selectLanguageStep);
        selectLanguageStep.HelpText = Lang.WelcomeText;
        selectLanguageStep.NextButtonText = Lang.Continue;
        selectLanguageStep.BackButtonText = Lang.Back;

        _currentLanguageLabel = new Label { Text = Lang.CurrentLanguage };
        
        var items = new ObservableCollection<string>(_cultureInfoNameSource.ToList());
        _languageListView = new ListView
        {
            Source = new ListWrapper<string> (items),
            X = 0,
            Y = Pos.Bottom (_currentLanguageLabel) + 2,
            Height = Dim.Fill (2),
            Width = Dim.Percent (40),
        };
        
        _languageListView.SelectedItemChanged += (s, e) =>
        {
            if (e.Item >= 0 && e.Item < _cultureInfoSource.Length)
            {
                SetCulture(_cultureInfoSource[e.Item]);
            }
        };
        
        _languageListView.SelectedItem = items.IndexOf(CurrentCulture.NativeName);
        
        selectLanguageStep.Add(_currentLanguageLabel, _languageListView);
    }

    private void SetCulture(CultureInfo culture)
    {
        if (Equals(CurrentCulture, culture))
        {
            return;
        }

        CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        Lang.Culture = culture;

        Remove(_wizard);
        _wizard.Dispose();
        CreateSteps();
    }

}