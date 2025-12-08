using System;
using System.Text;

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
    
    /// <summary>
    /// 获取被指定左右括号包围的第 N 组子串（不含括号）。
    /// </summary>
    /// <param name="str">源字符串</param>
    /// <param name="left">左括号字符</param>
    /// <param name="right">右括号字符</param>
    /// <param name="nth">第 N 组，从 1 开始</param>
    /// <returns>匹配到的子串；未找到返回 <see cref="string.Empty"/></returns>
    public static string SubstringWithBracket(this string str, char left, char right, int nth = 1)
    {
        if (string.IsNullOrEmpty(str) || nth < 1) return string.Empty;

        int count = 0;          // 括号深度计数
        int start = -1;         // 当前候选左括号位置
        int found = 0;          // 已找到的匹配组数

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == left)
            {
                if (count == 0) start = i; // 记录最外层左括号起点
                count++;
            }
            else if (str[i] == right)
            {
                if (count > 0) count--;
                if (count != 0 || start < 0) continue;
                found++;
                if (found == nth)
                {
                    // 返回不含括号的内容
                    return str.Substring(start + 1, i - start - 1);
                }
                start = -1; // 重置候选起点
            }
        }

        return string.Empty;
    }
    
    /// <summary>
    /// 使用字符串作为左右括号，获取第 N 组匹配的内容（不含括号）。
    /// </summary>
    public static string SubstringWithBracket(
        this string str,
        string left,
        string right,
        int nth = 1)
    {
        return SubstringWithBracket(str, left, right, nth, false);
    }

    /// <summary>
    /// 使用字符串作为左右括号，获取第 N 组匹配的内容，可配置是否包含括号与边界严格性。
    /// </summary>
    /// <param name="str">源字符串</param>
    /// <param name="left">左括号字符串</param>
    /// <param name="right">右括号字符串</param>
    /// <param name="nth">第 N 组（从 1 开始）</param>
    /// <param name="includeBrackets">是否包含左右括号</param>
    /// <param name="strict">是否严格匹配边界（左侧不能紧贴字母数字，右侧不能紧贴字母数字或下划线）</param>
    /// <param name="comparison">字符串比较规则</param>
    /// <returns>匹配到的子串；未找到返回 <see cref="string.Empty"/></returns>
    public static string SubstringWithBracket(
        this string str,
        string left,
        string right,
        int nth,
        bool includeBrackets,
        bool strict = false,
        StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right) || nth < 1)
            return string.Empty;

        int count = 0;
        int start = -1; // 候选左括号起始索引
        int found = 0;  // 已匹配组数

        for (int i = 0; i <= str.Length - left.Length; i++)
        {
            // 1) 尝试匹配左括号
            if (str.AsSpan(i, left.Length).Equals(left.AsSpan(), comparison))
            {
                if (count == 0)
                {
                    // 检查左侧边界：若严格模式，左侧不能是字母数字或下划线
                    if (strict && i > 0 && IsWordChar(str[i - 1]))
                    {
                        continue;
                    }
                    start = i;
                }
                count++;
                i += left.Length - 1;
                continue;
            }

            // 2) 尝试匹配右括号（只有当我们在括号内时才检查）
            if (count <= 0 || i > str.Length - right.Length ||
                !str.AsSpan(i, right.Length).Equals(right.AsSpan(), comparison)) continue;
            count--;
            if (count == 0 && start >= 0)
            {
                found++;
                if (found == nth)
                {
                    int contentStart = includeBrackets ? start : start + left.Length;
                    int contentEnd = includeBrackets ? i + right.Length : i;
                    // 检查右侧边界：若严格模式，右侧不能是字母数字或下划线
                    if (!strict || contentEnd >= str.Length || !IsWordChar(str[contentEnd]))
                        return str.Substring(contentStart, contentEnd - contentStart);
                    // 右侧紧贴词字符，视为不匹配，重置候选
                }
                start = -1; // 重置候选起点
            }
            i += right.Length - 1;
        }

        return string.Empty;
    }

    /// <summary>
    /// 判断字符是否为“单词字符”（字母、数字、下划线），用于严格边界判定。
    /// </summary>
    private static bool IsWordChar(char c) => char.IsLetterOrDigit(c) || c == '_';

    /// <summary>
    /// 返回一个新字符串，移除所有被指定单字符左右括号及其内部内容（不含括号本身）。
    /// 支持嵌套与并列，例如："(a(b)c)" -> "ac"。
    /// </summary>
    public static string WithoutBracketContent(this string str, char left, char right)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;

        var sb = new StringBuilder(str.Length);
        int depth = 0;
        foreach (char c in str)
        {
            if (c == left) { depth++; continue; }
            if (c == right) { if (depth > 0) depth--; continue; }
            if (depth == 0) sb.Append(c);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 返回一个新字符串，移除所有被指定字符串左右括号及其内部内容（不含括号本身）。
    /// 支持嵌套与并列，例如："【a【b】c】" -> "ac"。
    /// </summary>
    public static string WithoutBracketContent(this string str, string left, string right, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
            return str ?? string.Empty;

        var sb = new StringBuilder(str.Length);
        int depth = 0;
        int i = 0;
        while (i <= str.Length - left.Length)
        {
            if (str.AsSpan(i, left.Length).Equals(left.AsSpan(), comparison))
            {
                depth++;
                i += left.Length;
                continue;
            }

            if (depth > 0 && i <= str.Length - right.Length &&
                str.AsSpan(i, right.Length).Equals(right.AsSpan(), comparison))
            {
                depth--;
                i += right.Length;
                continue;
            }

            if (depth == 0) sb.Append(str[i]);
            i++;
        }

        // 剩余字符（右括号多、不成对的情况）直接跳过
        return sb.ToString();
    }
}