using AGOT.Base;
namespace AGOT.GenerateFiles;
public class GenerateDefinition : GenerateClasses
{
    public GenerateDefinition(List<Empire> empires) : base(empires) => Empires = empires;
    
    
    public bool Generate (string generatedFile)
    {
        var folderStruct = Directory.CreateDirectory(@$"{generatedFile}\map_data\");
        var fileName = $@"{folderStruct}\definition.csv";
        var writer = new StreamWriter(fileName, false, Encoding.Default);
        var csv = "0;0;0;0;x;x;\n";

        var baronies = Empires.SelectMany(e => e.Kingdoms).SelectMany(k => k.Duchies).SelectMany(d => d.Counties)
            .SelectMany(c => c.Baronies).ToList();
        foreach (var barony in baronies)
        {
            csv += string.Join(";", barony.ProvinceId, barony.ColorRgbCsv, barony.Name?.ToUpper(), "x;\n");
        }
        if (baronies.Count <= 0)
            return false;
        writer.WriteLine(csv);
        writer.Flush();
        writer.Close();
        return true;
    }
}
