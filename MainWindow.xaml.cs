using System.Diagnostics;
using AGOT;
using AGOT.Base;
using AGOT.GenerateFiles;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AGOT;
public partial class MainWindow
{
    public const string ImportXls = @"C:\Users\Helgi\Desktop\Development\CKIII - Excel Exporter\AGOT-Tool\TestFile\provinceDef.xls";
    public const string ExportLandedTitles = @"C:\Users\Helgi\Desktop\AgotTestFolder\common\landed_titles\";
    public const string ExportHistory = @"C:\Users\Helgi\Desktop\AgotTestFolder\history\provinces\";
    public const string ExportDefaultMap = @"C:\Users\Helgi\Desktop\AgotTestFolder\map_data\default.map";
    public const string ExportDefinition = @"C:\Users\Helgi\Desktop\AgotTestFolder\definition.csv";
    public const string ExportProvinceTerrain = @"C:\Users\Helgi\Desktop\AgotTestFolder\common\province_terrain\province_terrain.txt";
    
    private List<Empire> _empires = new();
    
    private GenerateLandedTitle _generateLandedTitle;
    private GenerateHistoryProvinces _generateHistoryProvinces;
    private GenerateDefaultMap _generateDefaultMap = new();
    private GenerateDefinition _generateDefinition;
    private GenerateProvinceTerrain _generateProvinceTerrain;

    private void Init()
    {
        var workbook = new Workbook();
        workbook.LoadFromFile(ImportXls);
        var westerosLandWorksheet = workbook.Worksheets[1]; //WESTEROS LAND Sheet
        var westerosSeaProvincesWorksheet = workbook.Worksheets[2]; //WESTEROS Sea Provinces Sheet
        var westerosLandDataTable = westerosLandWorksheet.ExportDataTable();
        var westerosSeaDataTable = westerosSeaProvincesWorksheet.ExportDataTable();
    
        _generateLandedTitle = new GenerateLandedTitle(_empires);
        // _generateLandedTitle.Generate(ExportLandedTitles, westerosLandDataTable);
    
        _generateHistoryProvinces = new GenerateHistoryProvinces(_empires);
        // _generateHistoryProvinces.Generate(ExportHistory);
    
        // _generateDefaultMap.Generate(ExportDefaultMap, westerosSeaDataTable);
    
        _generateDefinition = new GenerateDefinition(_empires);
        // _generateDefinition.Generate(ExportDefinition);
    
        _generateProvinceTerrain = new GenerateProvinceTerrain(_empires);
        // _generateProvinceTerrain.Generate(ExportProvinceTerrain);
    }
    
    
    public MainWindow()
    {

        InitializeComponent();
        Init();

    }
    private void LTButton_OnClick (object sender, RoutedEventArgs e)
    {
        var openFolderDialog = new CommonOpenFileDialog()
        {
            IsFolderPicker = true,
            InitialDirectory = $@"{ExportLandedTitles}",
        };

        if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            FileNameLtTextBox.Text = openFolderDialog.FileName;
        }
    }
    private void HButton_OnClick (object sender, RoutedEventArgs e)
    {
        var openFolderDialog = new CommonOpenFileDialog()
        {
            IsFolderPicker = true,
            InitialDirectory = @"C:\Documents\",
        };

        if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            FileNameHTextBox.Text = openFolderDialog.FileName;
        }
    }
    private void ExcelFile_OnClick (object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            DefaultExt = ".xls",
            Filter = "Excel Files|*.xls;*.xlsx;*.xlsm",
            InitialDirectory = @"C:\Documents\",
        };
        var result = openFileDialog.ShowDialog();
        if (result == true)
        {
            excelFileTextBlock.Text = openFileDialog.FileName;
        }
    }
}
