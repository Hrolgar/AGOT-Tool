using AGOT.Base;
using AGOT.Extensions;

namespace AGOT.GenerateFiles;

public class GenerateHistoryProvinces : GenerateClasses
{
    public GenerateHistoryProvinces (List<Empire> empires) : base(empires) => Empires = empires;

    public bool Generate (string generatedFile)
    {
        var folderStruct = Directory.CreateDirectory(@$"{generatedFile}\history\provinces\");
        foreach (var eKingdom in Empires.SelectMany(empire => empire.Kingdoms))
        {
            var file = folderStruct.ToString();
            file += @$"00_agot_k_{eKingdom.Name.RemoveExtra().Replace(" ", "_")}_prov.txt";

            var txt = "";
            foreach (var eKingdomDuchy in eKingdom.Duchies)
            {
                txt += $"\n## d_{eKingdomDuchy.Name.RemoveExtra()} ###################################\n";
                foreach (var eCounty in eKingdomDuchy.Counties)
                {
                    txt += $"### c_{eCounty.Name.RemoveExtra()}\n";
                    foreach (var barony in eCounty.Baronies)
                    {
                        txt += $"{barony.ProvinceId} = {{ # {barony.Name}\n";
                        if (!barony.Culture.IsEmptyOrNull())
                        {
                            txt += $"\tculture = {barony.Culture}\n";
                        }
                        if (!barony.Religion.IsEmptyOrNull())
                        {
                            txt += $"\treligion = {barony.Religion}\n";
                        }
                        txt += barony.HoldingType.IsEmptyOrNull() ? "\tholding = none\n" : $"\tholding = {barony.HoldingType}\n";
                        if (!barony.ProvinceHistory.IsEmptyOrNull())
                        {
                            txt += "\n# History\n" +
                                   $"{barony.ProvinceHistory}\n";
                        }
                        txt += "}\n\n";
                    }
                }
            }
            var hWriter = new StreamWriter(file, false, Encoding.Default);
            hWriter.WriteLine(txt);
            hWriter.Flush();
            hWriter.Close();
        }
        return true;
    }
}
