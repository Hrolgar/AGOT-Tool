using System.Runtime.InteropServices;
using AGOT.Base;
using AGOT.ExtensionsHelpers;
using AGOT.GenerateFiles;

namespace AGOT;
public partial class MainWindow
{
    private string _importXls = "";
    private string _exportDirectory = "";

    private List<Empire> _empires = new();
    
    private GenerateLandedTitle _generateLandedTitle;
    private GenerateHistoryProvinces _generateHistoryProvinces;
    private GenerateDefaultMap _generateDefaultMap = new();
    private GenerateDefinition _generateDefinition;
    private GenerateProvinceTerrain _generateProvinceTerrain;

    private void Init()
    {
        _generateLandedTitle = new GenerateLandedTitle(_empires);
        _generateHistoryProvinces = new GenerateHistoryProvinces(_empires);
        _generateDefinition = new GenerateDefinition(_empires);
        _generateProvinceTerrain = new GenerateProvinceTerrain(_empires);
    }

    public MainWindow()
    {
        InitializeComponent();
        Init();
        GenerateButton.IsEnabled = false;
        NewFilesTb.IsEnabled = false;
        ExcelFileTb.IsEnabled = false;
    }
    
    private void ExcelFile_OnClick (object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            DefaultExt = ".xls",
            Filter = "Excel Files|*.xls;",
            InitialDirectory = @"C:\Documents\",
        };
        var result = openFileDialog.ShowDialog();
        if (result == true)
        {
            _importXls = openFileDialog.FileName;
            ExcelFileTb.Text = openFileDialog.FileName;
        }
        if (_importXls.IsEmptyOrNull() || _exportDirectory.IsEmptyOrNull()) return;
        GenerateButton.IsEnabled = true;
    }
    
    private void NewFiles_OnClick (object sender, RoutedEventArgs e)
    {
        var openFolderDialog = new CommonOpenFileDialog()
        {
            IsFolderPicker = true,
            InitialDirectory = @"C:\Documents\"
        };
    
        if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            _exportDirectory = openFolderDialog.FileName;
            NewFilesTb.Text = openFolderDialog.FileName;
        }
        if (_importXls.IsEmptyOrNull() || _exportDirectory.IsEmptyOrNull()) return;
        GenerateButton.IsEnabled = true;
    }


    private void GenerateFiles_OnClick (object sender, RoutedEventArgs e)
    {
        StatusPanel.Children.Clear();
        if (_importXls.IsEmptyOrNull() || _exportDirectory.IsEmptyOrNull()) return;
        
        var workbook = new Workbook();
        workbook.LoadFromFile(_importXls);
        // foreach (var worksheet in workbook.Worksheets)
        // {
        //     Extensions.AllocConsole();
        //     Console.WriteLine(worksheet.Name);
        // }
        var westerosLandWorksheet = workbook.Worksheets[workbook.Worksheets
            .First(w => w.Name.RemoveSpaceAndCaps() == "westerosland").Index];
        var westerosSeaProvincesWorksheet = workbook.Worksheets[workbook.Worksheets
            .First(w => w.Name.RemoveSpaceAndCaps() == "westerosseaprovinces").Index];
        var westerosLandDataTable = westerosLandWorksheet.ExportDataTable();
        var westerosSeaDataTable = westerosSeaProvincesWorksheet.ExportDataTable();

        var generatedLandedT = _generateLandedTitle.Generate(_exportDirectory, westerosLandDataTable);
        StatusPanel.Children.Add(Extensions.StatusTextBox(generatedLandedT, "Landed Title"));
        
        var generatedHistoryP = _generateHistoryProvinces.Generate(_exportDirectory);
        StatusPanel.Children.Add(Extensions.StatusTextBox(generatedHistoryP, "Default Map"));

        var generatedDefinition = _generateDefinition.Generate(_exportDirectory);
        StatusPanel.Children.Add(Extensions.StatusTextBox(generatedDefinition, "Default Map"));

        var generatedDefaultM = _generateDefaultMap.Generate(_exportDirectory, westerosSeaDataTable);
        StatusPanel.Children.Add(Extensions.StatusTextBox(generatedDefaultM, "Default Map", " Due to Excel sheet being empty."));

       var generatedProvinceT = _generateProvinceTerrain.Generate(_exportDirectory);
       StatusPanel.Children.Add(Extensions.StatusTextBox(generatedProvinceT, "Province Terrain"));
       
       _empires = new List<Empire>();
        Init();
    }
}
