using AGOT.Base;
using AGOT.ExtensionsHelpers;
namespace AGOT.GenerateFiles;
public class GenerateLandedTitle : GenerateClasses
{
    public GenerateLandedTitle(List<Empire> empires) : base(empires)=>  Empires = empires;
    
    public bool Generate (string generatedFilePath, DataTable dataTable)
    {
        var folderStruct = Directory.CreateDirectory(@$"{generatedFilePath}\common\landed_titles\");
        var fileName = @$"{folderStruct}\01_landed_titles.txt";
        var streamWriter = new StreamWriter(fileName, false, Encoding.Default);
        
        var columnDictionary = dataTable.Columns.Cast<DataColumn>().ToDictionary(tableColumn => tableColumn.Ordinal, tableColumn => tableColumn.ColumnName);
        var asd = dataTable.Columns.Cast<DataColumn>().ToDictionary( tableColumn => tableColumn.ColumnName, tableColumn => tableColumn.Ordinal);

        Empire empire = null!;
        Kingdom kingdom = null!;
        Duchy duchy = null!;
        County county = null!;

        foreach (DataRow row in dataTable.Rows)
        {
            // var provinceId = row.ItemArray[columnDictionary.First(d => d.Value == "Province ID").Key]?.ToInt();

            var provinceId = row.ItemArray[asd.GetColumnNumber("Province ID")].ToInt();
            var provinceColors = row.ItemArray[columnDictionary.First(d => d.Value == "Province Colour").Key]?.ToString().HexToRgb();
            var countyColors = row.ItemArray[15]?.ToString();
            var duchyColors = row.ItemArray[16]?.ToString();
            var kingdomColors = row.ItemArray[17]?.ToString();
            var empireColors = row.ItemArray[18]?.ToString();

            var empireName = row.ItemArray[6]?.ToString();
            var kingdomName = row.ItemArray[7]?.ToString();
            var duchyName = row.ItemArray[8]?.ToString();
            var countyName = row.ItemArray[9]?.ToString();
            var baronyName = row.ItemArray[3]?.ToString();

            var culture = row.ItemArray[10]?.ToString();
            var religion = row.ItemArray[11]?.ToString();
            var holdingType = row.ItemArray[14]?.ToString();
            var provinceHistory = row.ItemArray[24]?.ToString();
            var terrain = row.ItemArray[12]?.ToString();
            
            // var defForm = !row.ItemArray[19]?.ToString().IsEmptyOrNull();
            var defForm = row.ItemArray[19]?.ToString();
            var aiPri = row.ItemArray[20]?.ToString();
            var canCreate = row.ItemArray[21]?.ToString();

            if (!empireName.IsEmptyOrNull())
            {
                empire = new Empire(empireName, empireColors.IsEmptyOrNull() ? provinceColors : empireColors.HexToRgb());

                // if (!defForm.IsEmptyOrNull() && defForm.Contains("e-"))
                // {
                //     var e = defForm.Replace("e-", "");
                //     var s = e.Remove(3);
                //     Console.WriteLine(s);
                // }
                
                // if (!defForm.ToString().IsEmptyOrNull())
                // {
                //     var obj = new object[] { new { canCreate }, new { aiPri }, new { defForm } };
                //
                //     empire.AddExtra(obj);
                // }
                Empires.Add(empire);
            }

            if (!kingdomName.IsEmptyOrNull())
            {
                kingdom = new Kingdom(kingdomName, kingdomColors.IsEmptyOrNull() ? provinceColors : kingdomColors.HexToRgb());
                empire?.AddKingdom(kingdom);
            }

            if (!duchyName.IsEmptyOrNull())
            {
                duchy = new Duchy(duchyName, duchyColors.IsEmptyOrNull() ? provinceColors : duchyColors.HexToRgb());
                kingdom?.AddDuchy(duchy);
            }

            if (!countyName.IsEmptyOrNull())
            {
                county = new County(countyName, countyColors.IsEmptyOrNull() ? provinceColors : countyColors.HexToRgb());
                duchy.AddCounty(county);

                var newCapitalName = $"c_{county.Name.RemoveExtra()}\n";
                
                if (!duchyName.IsEmptyOrNull())
                {
                    duchy.Capital = newCapitalName;
                }
                if (!kingdomName.IsEmptyOrNull() && kingdom is not null)
                {
                    kingdom.Capital = newCapitalName;
                }
                if (!empireName.IsEmptyOrNull() && empire is not null)
                {
                    empire.Capital = newCapitalName;
                }
            }

            if (!baronyName.IsEmptyOrNull())
            {
                var barony = new Barony(baronyName, provinceColors, provinceId, culture, religion, holdingType, provinceHistory, terrain);
                
                county?.AddBarony(barony);
            }
        }
        var txt = "@correct_culture_primary_score = 100\n" +
                  "@better_than_the_alternatives_score = 50\n" +
                  "@always_primary_score = 1000\n\n";
        Empires.ForEach(e => txt += e.Print());
        streamWriter.WriteLine(txt);
        streamWriter.Flush();
        streamWriter.Close();
        return true;
    }
}
