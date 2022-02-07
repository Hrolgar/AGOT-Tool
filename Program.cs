// using AGOT.Base;
// using AGOT.GenerateFiles;
// namespace AGOT;
//
// public static class Program
// {
//     public const string ImportXls = "C:\\Users\\Helgi\\Desktop\\Development\\CKIII - Excel Exporter\\AGOT Excel Exporter\\TestFile\\provinceDef.xls";
//     public const string ExportLandedTitles = @"C:\Users\Helgi\Desktop\AgotTestFolder\common\landed_titles\01_agot_landed_titles.txt";
//     public const string ExportHistory = @"C:\Users\Helgi\Desktop\AgotTestFolder\history\provinces\";
//     public const string ExportDefaultMap = @"C:\Users\Helgi\Desktop\AgotTestFolder\map_data\default.map";
//     public const string ExportDefinition = @"C:\Users\Helgi\Desktop\AgotTestFolder\definition.csv";
//     public const string ExportProvinceTerrain = @"C:\Users\Helgi\Desktop\AgotTestFolder\common\province_terrain\province_terrain.txt";
//
//     private static List<Empire> _empires = new();
//     
//     private static GenerateLandedTitle _generateLandedTitle;
//     private static GenerateHistoryProvinces _generateHistoryProvinces;
//     private static GenerateDefaultMap _generateDefaultMap = new();
//     private static GenerateDefinition _generateDefinition;
//     private static GenerateProvinceTerrain _generateProvinceTerrain;
//
//     private static void Main (string[] args)
//     {
//         var application = new Application()
//         {
//             StartupUri = new Uri("MainWindow.xaml", UriKind.Relative)
//         };
//         
//         application.Run();
//         var workbook = new Workbook();
//         workbook.LoadFromFile(ImportXls);
//         var westerosLandWorksheet = workbook.Worksheets[1]; //WESTEROS LAND Sheet
//         var westerosSeaProvincesWorksheet = workbook.Worksheets[2]; //WESTEROS Sea Provinces Sheet
//         var westerosLandDataTable = westerosLandWorksheet.ExportDataTable();
//         var westerosSeaDataTable = westerosSeaProvincesWorksheet.ExportDataTable();
//         
//         _generateLandedTitle = new GenerateLandedTitle(_empires);
//         _generateLandedTitle.Generate(ExportLandedTitles, westerosLandDataTable);
//         
//         _generateHistoryProvinces = new GenerateHistoryProvinces(_empires);
//         _generateHistoryProvinces.Generate(ExportHistory);
//         
//         _generateDefaultMap.Generate(ExportDefaultMap, westerosSeaDataTable);
//         
//         _generateDefinition = new GenerateDefinition(_empires);
//         _generateDefinition.Generate(ExportDefinition);
//
//         _generateProvinceTerrain = new GenerateProvinceTerrain(_empires);
//         _generateProvinceTerrain.Generate(ExportProvinceTerrain);
//     }
// }
