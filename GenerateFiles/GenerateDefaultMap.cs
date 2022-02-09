using System.Windows.Navigation;
using AGOT.Extensions;
namespace AGOT.GenerateFiles;
public class GenerateDefaultMap
{
    public void Generate (string generatedFile, DataTable dataTable)
    {
        var dict = new Dictionary<int, string>()
        {
            { 0, "Comment" }, { 1, "Province ID" }, { 2, "Red" },{ 3, "Green" },{ 4, "Blue" }, { 5, "ProvinceName" },{ 6, "EmptyCol" },
        };
        var folderStruct = Directory.CreateDirectory(@$"{generatedFile}\map_data\");
        var fileName = @$"{folderStruct}\default.map";
        
        var txt = "#max_provinces = 1466\n" +
                  "definitions = \"definition.csv\"\n" +
                  "provinces = \"provinces.png\"\n" +
                  "#positions  = \"positions.txt\"\n" +
                  "rivers = \"rivers.png\"\n" +
                  "#terrain_definition  = \"terrain.txt\"\n" +
                  "topology = \"heightmap.heightmap\"\n" +
                  "#tree_definition  = \"trees.bmp\"\n" +
                  "continent = \"continent.txt\"\n" +
                  "adjacencies = \"adjacencies.csv\"\n" +
                  "#climate = \"climate.txt\"\n" +
                  "island_region = \"island_region.txt\"\n" +
                  "seasons = \"seasons.txt\"\n\n";

        var seaZones = new List<int>();
        var impassableSea = new List<int>();
        var riverProvinces = new List<int>();
        var lakes = new List<int>();
        var impassableMountains = new List<int>();
        
        foreach (DataRow row in dataTable.Rows)
        {
            var provId = 0;
            if (int.TryParse(row.ItemArray[1]?.ToString(), out var a))
            {
                provId = a;
            }
            else
            {
                File.WriteAllText(fileName, string.Empty);
                return;
            }

            var seaZone = row.ItemArray[6]?.ToString();

            switch (seaZone)
            {
                case "SZ":
                    seaZones.Add(provId);
                    break;
                case "ISZ":
                    impassableSea.Add(provId);
                    break;
                case "RP":
                    riverProvinces.Add(provId);
                    break;
                case "L":
                    lakes.Add(provId);
                    break;
                case "IM":
                    impassableMountains.Add(provId);
                    break;
            }

        }
        
        Helpers.GenerateMapZones(seaZones, "SEA ZONES", "sea_zones",ref txt);
        Helpers.GenerateMapZones(impassableSea, "IMPASSABLE SEAS", "impassable_seas",ref txt);
        Helpers.GenerateMapZones(riverProvinces, "MAJOR RIVERS", "river_provinces",ref txt);
        Helpers.GenerateMapZones(lakes, "LAKES", "lakes",ref txt);
        Helpers.GenerateMapZones(impassableMountains, "WASTELAND", "impassable_mountains",ref txt);

        var hWriter = new StreamWriter(fileName, false, Encoding.Default);
        hWriter.WriteLine(txt);
        hWriter.Flush();
        hWriter.Close();
    }
}
