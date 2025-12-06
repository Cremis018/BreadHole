public static class StringUtil
{
    public static int NthIndexOfIndexOf(this string s, char ch, int n, int startIndex = 0)
    {
        if (string.IsNullOrEmpty(s) || n < 1) return -1;
        int index = -1;
        for (int i = 0; i < n; i++)
        {
            index = s.IndexOf(ch, index + 1);
            if (index == -1) return -1;
        }
        return index;
    }
}