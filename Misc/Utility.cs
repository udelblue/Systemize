namespace Systemize.Misc;

public static class Utility
{
    public static string GetLastChars(byte[] token)
    {
        return token[7].ToString();
    }
    public static List<string> SplitStringBySemicolon(string input)
    {
        return input.Split(';').ToList();
    }


}
