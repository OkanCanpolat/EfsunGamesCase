using System.Collections.Generic;

public static class ListExtension
{
    public static bool TryGetFactoryData(this List<FactoryData> source, string id, out FactoryData result)
    {
        foreach(FactoryData item in source)
        {
            if (item.Id == id)
            {
                result = item;
                return true;
            }
        }
        result = null;
        return false;
    }
}
