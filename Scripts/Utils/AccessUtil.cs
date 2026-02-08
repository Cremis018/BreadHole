using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Godot;
using FileAccess = Godot.FileAccess;

internal static class AccessUtil
{ 
    public static bool IsVirtualPath(this string path)
    {
        // 空值检查
        if (string.IsNullOrWhiteSpace(path))
            return false;
        
        // 去除首尾空白字符
        path = path.Trim();
        
        // Godot 虚拟路径必须以 res:// 或 user:// 开头
        if (!path.StartsWith("res://") && !path.StartsWith("user://"))
            return false;
        
        // 基本格式检查：协议后应该至少有一个字符（不能是空的）
        if (path.Length <= 6) // "res://" 或 "user://" 长度是 6
            return false;
        
        // 使用正则表达式进行更严格的验证
        // 允许字母、数字、下划线、连字符、点、斜杠和空格（在文件名中）
        const string pattern = @"^(res|user)://([a-zA-Z0-9_\-\./\s]|[^\x00-\x1F\x7F])+$";
        
        try
        {
            return Regex.IsMatch(path, pattern);
        }
        catch (ArgumentException)
        {
            // 正则表达式无效时的备用方案
            return IsValidBasicGodotPath(path);
        }
    }
    
    private static bool IsValidBasicGodotPath(string path)
    {
        // 检查协议部分
        string protocol = path[..6].ToLower();
        if (protocol != "res://" && protocol != "user://")
            return false;
        
        // 获取路径部分（去掉协议）
        string pathPart = path[6..];
        
        // 路径不能为空
        if (string.IsNullOrEmpty(pathPart))
            return false;
        
        // 检查是否包含非法字符（基本检查）
        // Godot 通常不允许控制字符等
        if (pathPart.Any(c => char.IsControl(c) || c == '<' || c == '>' || c == '"' || c == '|' || c == '?' || c == '*'))
        {
            return false;
        }

        // 检查是否有连续的斜杠（除了协议后的双斜杠）
        return !path.Contains("//") || path.IndexOf("//", StringComparison.Ordinal) == 5; // 协议后的 // 在位置5
    }
    
    public static bool FileExists(string path, out string absPath, string rootPath = "")
    {
        absPath = path;
        if (FileAccess.FileExists(path)) return true;
        if (IsVirtualPath(path)) return false;
        absPath = Path.Combine(rootPath,path);
        return FileAccess.FileExists(absPath);
    }

    public static bool FolderExists(string path, out string absPath, string rootPath = "")
    {
        absPath = path;
        if (DirAccess.DirExistsAbsolute(path)) return true;
        if (IsVirtualPath(path)) return false; 
        absPath = Path.Combine(rootPath,path);
        return DirAccess.DirExistsAbsolute(absPath);
    }
}