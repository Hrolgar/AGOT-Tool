using AGOT.Base;
namespace AGOT.GenerateFiles;
public class GenerateDefinition : GenerateClasses
{
    public GenerateDefinition(List<Empire> empires) : base(empires) => Empires = empires;
    
    
    public void Generate (string generatedFile)
    {
        var writer = new StreamWriter(generatedFile, false, Encoding.Default);
        var csv = "0;0;0;0;x;x;\n";
        foreach (var eKingdom in Empires.SelectMany(empire => empire.Kingdoms))
        {
            foreach (var barony in eKingdom.Duchies.SelectMany(duchy => duchy.Counties.SelectMany(county => county.Baronies)))
            {
                csv += string.Join(";", barony.Id, barony.ColorRgbCsv, barony.Name?.ToUpper(), "x;\n");
            }
        }
        writer.WriteLine(csv);
        writer.Flush();
        writer.Close();
    }
}
