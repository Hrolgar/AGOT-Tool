using AGOT.Base;
using AGOT.ExtensionsHelpers;
using AGOT.GenerateFiles;

namespace AGOT;
public partial class MainWindow
{
    private string _importXls = "";
    private string _exportDirectory = "";

    private List<Empire> _empires = new();
    private Workbook _workbook;
    private GenerateLandedTitle _generateLandedTitle;
    private GenerateHistoryProvinces _generateHistoryProvinces;
    private GenerateDefaultMap _generateDefaultMap = new();
    private GenerateDefinition _generateDefinition;
    private GenerateProvinceTerrain _generateProvinceTerrain;
    private static Dictionary<string, int> _newFileColumns = new();
    private static Dictionary<string, string> _comboBoxData = new();
    private void Init()
    {
        _generateLandedTitle = new GenerateLandedTitle(_empires);
        _generateHistoryProvinces = new GenerateHistoryProvinces(_empires);
        _generateDefinition = new GenerateDefinition(_empires);
        _generateProvinceTerrain = new GenerateProvinceTerrain(_empires);
    }

    public MainWindow()
    {
        Extensions.AllocConsole();
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
            ExcelFileTb.Text = openFileDialog.SafeFileName;
            
            _workbook = new Workbook();
            _workbook.LoadFromFile(_importXls);
            var listOfSheets = _workbook.Worksheets.ToArray().Select(d => d.Name).ToList();
            listOfSheets.Add("");
            
            // Sheets.Children.Add(Extensions.SheetDockElement(listOfSheets, "Sheet for generating Landed Title:", "land"));
            // Sheets.Children.Add(Extensions.SheetDockElement(listOfSheets, "Sheet for generating Province History:", "land"));
            // Sheets.Children.Add(Extensions.SheetDockElement(listOfSheets, "Sheet for generating Province Terrain:", "land"));
            // Sheets.Children.Add(Extensions.SheetDockElement(listOfSheets, "Sheet for generating Default Map:", "sea"));
            // Sheets.Children.Add(Extensions.SheetDockElement(listOfSheets, "Sheet for generating Definition:", "land"));
            
            var westerosLandWorksheet = _workbook.Worksheets[_workbook.Worksheets
                .First(w => w.Name.RemoveSpaceAndCaps() == "westerosland").Index].ExportDataTable();
            _newFileColumns = westerosLandWorksheet.Columns.Cast<DataColumn>().ToDictionary(column => column.ColumnName, column => column.Ordinal);

            Sheets.Children.Add(Extensions.SheetDockElement(_newFileColumns.Keys, "Dropdown", "Province ID", "Province ID"));
            Sheets.Children.Add(Extensions.SheetDockElement(_newFileColumns.Keys, "Dropdown", "Empire", "Empire"));

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
        
        var westerosLandWorksheet = _workbook.Worksheets[_workbook.Worksheets
            .First(w => w.Name.RemoveSpaceAndCaps() == "westerosland").Index];
        var westerosSeaProvincesWorksheet = _workbook.Worksheets[_workbook.Worksheets
            .First(w => w.Name.RemoveSpaceAndCaps() == "westerosseaprovinces").Index];
        var westerosLandDataTable = westerosLandWorksheet.ExportDataTable();
        var westerosSeaDataTable = westerosSeaProvincesWorksheet.ExportDataTable();

        var generatedLandedT = _generateLandedTitle.Generate(_exportDirectory, westerosLandDataTable);
        StatusPanel.Children.Add(Extensions.StatusTextBox(generatedLandedT, "Landed Title"));
        
        var generatedHistoryP = _generateHistoryProvinces.Generate(_exportDirectory);
        StatusPanel.Children.Add(Extensions.StatusTextBox(generatedHistoryP, "History Provinces"));

        var generatedDefinition = _generateDefinition.Generate(_exportDirectory);
        StatusPanel.Children.Add(Extensions.StatusTextBox(generatedDefinition, "Definition"));

        var generatedDefaultM = _generateDefaultMap.Generate(_exportDirectory, westerosSeaDataTable);
        StatusPanel.Children.Add(Extensions.StatusTextBox(generatedDefaultM, "Default Map", " Due to Excel sheet being empty."));

       var generatedProvinceT = _generateProvinceTerrain.Generate(_exportDirectory);
       StatusPanel.Children.Add(Extensions.StatusTextBox(generatedProvinceT, "Province Terrain"));
       
       _empires = new List<Empire>();
        Init();
    }
    
    public static void ComboBoxEvent(ComboBox cB)
    {
        _comboBoxData.Add(cB.Name, cB.Text);
    }
}

