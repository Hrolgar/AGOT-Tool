namespace AGOT.Extensions;
public static class Helpers
{
    public static void GenerateMapZones (List<int> ids, string commentTxt, string insideTxt, ref string txt)
    {
        if (ids.Count <= 0)
        {
            txt += "";
            return;
        }
        txt += "\n#############\n" +
               $"# {commentTxt}\n" +
               "#############\n";
        var rangeList = ids.Count > 2 ? "RANGE" : "LIST";
        var notEqual = ids[^1] != ids[0] ? ids[^1].ToString() : "";
        
        txt += $"{insideTxt} = {rangeList} {{ {ids[0]} {notEqual} }}\n";
    }
}
