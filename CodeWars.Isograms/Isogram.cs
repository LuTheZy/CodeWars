using System.Linq;

public class Isogram
{
    public static bool IsIsogram(string str)
    {
        if (str == string.Empty)
        {
            return true;
        }
        return str.Where(char.IsLetter)
              .GroupBy(char.ToLower)
              .All(g => g.Count() == 1);
    }
}