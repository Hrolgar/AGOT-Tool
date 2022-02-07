﻿using AGOT.Base;
using AGOT.Extensions;
namespace AGOT.GenerateFiles;
public class GenerateProvinceTerrain : GenerateClasses
{
    public GenerateProvinceTerrain (List<Empire> empires) : base(empires) => Empires = empires;

    public void Generate(string generatedFile)
    {
        var writer = new StreamWriter(generatedFile, false, Encoding.Default);
        var txt = "default=plains\n";
        foreach (var eKingdom in Empires.SelectMany(empire => empire.Kingdoms))
        {
            foreach (var barony in eKingdom.Duchies.SelectMany(duchy => duchy.Counties.SelectMany(county => county.Baronies)))
            {
                if (!barony.Terrain.IsEmptyOrNull())
                {
                    txt += $"{barony.Id}={barony.Terrain}\n";
                }
            }
        }
        writer.WriteLine(txt);
        writer.Flush();
        writer.Close();
    }
}
