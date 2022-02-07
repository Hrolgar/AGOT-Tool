using AGOT.Extensions;
namespace AGOT.Base;
public class Base
{
    public string? Name { get; set; }
    protected string? Color { get; set; }
    protected string Color2 { get; set; }
    protected string? CanCreate;
    protected string? AiPri;
    protected bool? DefForm;
    protected Base (string? name, string? color)
    {
        Name = name;
        Color = color;
        Color2 = "255 255 255";
    }

    public void AddExtra (object[] extras)
    {
        CanCreate = extras[0].ToString();
        AiPri = extras[1].ToString();
        DefForm = (bool?)extras[2].GetType().GetProperty("defForm")?.GetValue(extras[2], null);
    }
}

public class Empire : Base
{
    public string Capital { get; set; } = null!;
    public readonly List<Kingdom> Kingdoms = new();

    public Empire (string? name, string? color) : base(name, color)
    {
        Name = name;
        Color = color;
    }

    public void AddKingdom (Kingdom kingdom) => Kingdoms.Add(kingdom);

    public string Print()
    {
        var txt =
            "@correct_culture_primary_score = 100\n" +
            "@better_than_the_alternatives_score = 50\n" +
            "@always_primary_score = 1000\n\n" +
            $"e_{Name.RemoveExtra()} = {{" +
            $"\n\tcolor = {{ {Color} }}\n" +
            $"\tcolor2 = {{ {Color2} }}\n";
        if (DefForm == true)
        {
            txt += "\n\tdefinite_form = yes\n";
        }
        txt += $"\n\tcapital = {Capital}\n";
        // if (!AiPri.IsEmptyOrNull())
        // {
        //     txt += $"\nai_primary_priority = {{\n" +
        //            $"{AiPri}\n }}";
        // }
        txt = Kingdoms.Aggregate(txt, (current, kingdom) => current + kingdom.Print());
        txt += "}\n";
        return txt;
    }
}

public class Kingdom : Base
{
    public string Capital { get; set; } = null!;
    public readonly List<Duchy> Duchies = new();

    public Kingdom (string? name, string? color) : base(name, color)
    {
        Name = name;
        Color = color;
    }
    public void AddDuchy (Duchy newDuchy)
    {
        Duchies.Add(newDuchy);
    }

    public string Print()
    {
        var txt =
            $"\tk_{Name.RemoveExtra()} = {{" +
            $"\n\t\tcolor = {{ {Color} }}\n" +
            $"\t\tcolor2 = {{ {Color2} }}\n";
        if (DefForm == true)
        {
            txt += "\n\t\tdefinite_form = yes\n";
        }
        txt += $"\n\t\tcapital = {Capital}\n";
        txt = Duchies.Aggregate(txt, (current, duchy) => current + duchy.Print());
        txt += "\t}\n";
        return txt;
    }
}

public class Duchy : Base
{
    private static int _dId;
    public int Id { get; set; }

    public string Capital { get; set; } = null!;
    public readonly List<County> Counties = new();

    public Duchy (string? name, string? color, bool resetId = false) : base(name, color)
    {
        if (resetId) _dId = 0;

        Id = ++_dId;
        Name = name;
        Color = color;
    }
    public void AddCounty (County newCounty) => Counties.Add(newCounty);

    public string Print()
    {
        var txt =
            $"\t\td_{Name.RemoveExtra()}_d_{Id} = {{" +
            $"\n\t\t\tcolor = {{ {Color} }}\n" +
            $"\t\t\tcolor2 = {{ {Color2} }}\n";
        if (DefForm == true)
        {
            txt += "\n\t\t\tdefinite_form = yes\n";
        }
        txt += $"\n\t\t\tcapital = {Capital}\n";
        txt = Counties.Aggregate(txt, (current, county) => current + county.Print(Id));
        txt += "\t\t}\n";
        return txt;
    }
}

public class County : Base
{
    private static int _cId;
    public int Id { get; set; }

    public readonly List<Barony> Baronies = new();
    public County (string? name, string? color, bool resetId = false) : base(name, color)
    {
        if (resetId) _cId = 0;
        Id = ++_cId;
        Name = name;
        Color = color;
    }

    public void AddBarony (Barony newBarony) => Baronies.Add(newBarony);

    public string Print (int duchyId)
    {
        var txt =
            $"\t\t\tc_{Name.RemoveExtra()}_d_{duchyId}_c_{Id} = {{" +
            $"\n\t\t\t\tcolor = {{ {Color} }}\n" +
            $"\t\t\t\tcolor2 = {{ {Color2} }}\n";
        if (DefForm == true)
        {
            txt += "\n\t\t\tdefinite_form = yes\n";
        }
        txt = Baronies.Aggregate(txt, (current, barony) => current + barony.Print());
        txt += "\n\t\t\t}\n";
        return txt;
    }
}

public class Barony : Base
{
    private static int _bId;
    public int Id { get; set; }
    public int ProvinceId { get; set; }
    
    public string Culture { get; set; }
    public string Religion { get; set; }
    public string HoldingType { get; set; }
    public string ProvinceHistory { get; set; }
    public string Terrain { get; set; }
    public string? ColorRgbCsv => Color?.Replace(" ", ";");

    public Barony (string? name, string? color, int provinceId, string culture, string religion, string holdingType, string provinceHistory, string terrain) : base(name, color)
    {
        Id = ++_bId;
        Name = name;
        Color = color;
        ProvinceId = provinceId;
        Culture = culture;
        Religion = religion;
        HoldingType = holdingType;
        ProvinceHistory = provinceHistory;
        Terrain = terrain;
    }

    public string Print()
    {
        var txt = $"\n\t\t\t\tb_{Name.RemoveExtra()}_{Id} = {{" +
                  $"\n\t\t\t\t\tprovince = {ProvinceId}\n" +
                  $"\t\t\t\t\tcolor = {{ {Color} }}\n" +
                  $"\t\t\t\t\tcolor2 = {{ {Color2} }}\n";
        if (DefForm == true)
        {
            txt += "\n\t\t\t\tdefinite_form = yes\n";
        }
        txt += $"\t\t\t\t}}";
        return txt;
    }
}
