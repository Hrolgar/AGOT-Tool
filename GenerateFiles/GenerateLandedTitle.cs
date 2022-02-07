using AGOT.Base;
using AGOT.Extensions;
namespace AGOT.GenerateFiles;
public class GenerateLandedTitle : GenerateClasses
{
    public GenerateLandedTitle(List<Empire> empires) : base(empires)=>  Empires = empires;
    
    public void Generate (string generatedFilePath, DataTable dataTable)
    {
        var folderStruct = Directory.CreateDirectory(@$"{generatedFilePath}\common\landed_titles\");
        var fileName = @$"{folderStruct}\01_agot_landed_titles.txt";
        var streamWriter = new StreamWriter(fileName, false, Encoding.Default);

        var dict = new Dictionary<int, string>()
        {
            { 0, "Comment" }, { 1, "Province ID" }, { 2, "Province Color" },
            { 3, "Province name" }, { 4, "(O)cean, (R)ivers, (L)akes, (W)astelands" }, { 5, "Renamings" }, { 6, "Empire" },
            { 7, "Kingdom" }, { 8, "Duchy" }, { 9, "County" }, { 10, "Culture" }, { 11, "Religion" },
            { 12, "Terrain" }, { 13, "Climate" }, { 14, "Holdyng Type" }, { 15, "County Title Color" }, { 16, "Duchy Title Color" },
            { 17, "Kingdom Title Color" },
            { 18, "Empire Title Color" }, { 19, "definite_form" }, { 20, "ai_primary_priority" }, { 21, "can_create" }, { 22, "can_create_on_partition" },
            { 23, "can_be_named_after_dynasty" }, { 24, "Province History" }
        };

        Empire empire = null!;
        Kingdom kingdom = null!;
        Duchy duchy = null!;
        County county = null!;

        foreach (DataRow row in dataTable.Rows)
        {
            var provinceColors = row.ItemArray[2]?.ToString().HexToRgb();
            var countyColors = row.ItemArray[15]?.ToString();
            var duchyColors = row.ItemArray[16]?.ToString();
            var kingdomColors = row.ItemArray[17]?.ToString();
            var empireColors = row.ItemArray[18]?.ToString();

            var empireName = row.ItemArray[6]?.ToString();
            var kingdomName = row.ItemArray[7]?.ToString();
            var duchyName = row.ItemArray[8]?.ToString();
            var countyName = row.ItemArray[9]?.ToString();

            var baronyName = row.ItemArray[3]?.ToString();

            var provId = row.ItemArray[1].ToInt();
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

                if (!defForm.IsEmptyOrNull() && defForm.Contains("e-"))
                {
                    var e = defForm.Replace("e-", "");
                    var s = e.Remove(3);
                    Console.WriteLine(s);
                }
                
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
                duchy = new Duchy(duchyName, duchyColors.IsEmptyOrNull() ? provinceColors : duchyColors.HexToRgb(), !kingdomName.IsEmptyOrNull());
                kingdom?.AddDuchy(duchy);
            }

            if (!countyName.IsEmptyOrNull())
            {
                county = new County(countyName, countyColors.IsEmptyOrNull() ? provinceColors : countyColors.HexToRgb(), !duchyName.IsEmptyOrNull());
                duchy.AddCounty(county);

                var newCapitalName = $"c_{county.Name.RemoveExtra()}_d_{duchy.Id}_c_{county.Id}\n";
                
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
                var barony = new Barony(baronyName, provinceColors, provId, culture, religion, holdingType, provinceHistory, terrain);
                
                county?.AddBarony(barony);
            }
        }
        var txt = "";
        Empires.ForEach(e => txt += e.Print());
        streamWriter.WriteLine(txt);
        streamWriter.Flush();
        streamWriter.Close();
    }
}
