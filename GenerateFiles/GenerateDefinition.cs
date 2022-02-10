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
        // foreach (var eKingdom in Empires.SelectMany(empire => empire.Kingdoms))
        // {
        //     foreach (var barony in eKingdom.Duchies.SelectMany(duchy => duchy.Counties.SelectMany(county => county.Baronies)))
        //     {
        //         csv += string.Join(";", barony.ProvinceId, barony.ColorRgbCsv, barony.Name?.ToUpper(), "x;\n");
        //     }
        // }
        var baronies = Empires.SelectMany(e => e.Kingdoms).SelectMany(k => k.Duchies).SelectMany(d => d.Counties)
            .SelectMany(c => c.Baronies).ToList();
        if (baronies.Count <= 0)
            return false;
        writer.WriteLine(csv);
        writer.Flush();
        writer.Close();
        return true;
    }
}
