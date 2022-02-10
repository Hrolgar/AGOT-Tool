using AGOT.Base;
using AGOT.Extensions;
namespace AGOT.GenerateFiles;
public class GenerateProvinceTerrain : GenerateClasses
{
    public GenerateProvinceTerrain (List<Empire> empires) : base(empires) => Empires = empires;

    public bool Generate(string generatedFile)
    {
        var folderStruct = Directory.CreateDirectory(@$"{generatedFile}\common\province_terrain\");
        var fileName = @$"{folderStruct}\00_province_terrain.txt";
        var writer = new StreamWriter(fileName, false, Encoding.Default);
        var txt = "default=plains\n";
        
        // foreach (var eKingdom in Empires.SelectMany(empire => empire.Kingdoms))
        // {
        //     foreach (var barony in eKingdom.Duchies.SelectMany(duchy => duchy.Counties.SelectMany(county => county.Baronies)))
        //     {
        //         if (!barony.Terrain.IsEmptyOrNull())
        //         {
        //             txt += $"{barony.ProvinceId}={barony.Terrain}\n";
        //         }
        //     }
        // }
        
        var baronies = Empires.SelectMany(e => e.Kingdoms).SelectMany(k => k.Duchies).SelectMany(d => d.Counties)
            .SelectMany(c => c.Baronies).ToList();
        if (baronies.Count <= 0)
            return false;
        
        foreach (var barony in baronies)
        {
            if (!barony.Terrain.IsEmptyOrNull())
            {
                txt += $"{barony.ProvinceId}={barony.Terrain}\n";
            }
        }
        writer.WriteLine(txt);
        writer.Flush();
        writer.Close();
        return true;
    }
}
