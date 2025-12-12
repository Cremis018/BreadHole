#nullable enable
using System;
using System.IO;
using System.Linq;
using Godot;

/// <summary>
/// 文本文件工具类：路径解析、存在性检查、读取与写入（自动创建目录）。
/// 所有方法均基于 Parse 将 Godot 虚拟路径或本地路径转换为系统绝对路径。
/// </summary>
public static class TextFileUtil
{
    /// <summary>
    /// 将 Godot 虚拟路径（如 res://、user://）或本地路径转换为系统绝对路径。
    /// </summary>
    /// <param name="path">Godot 路径或本地路径</param>
    /// <returns>系统绝对路径；若转换失败返回 null</returns>
    public static string? Parse(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        // 已经是绝对路径（Windows 风格也支持），直接返回
        if (Path.IsPathRooted(path))
            return Path.GetFullPath(path);

        // 尝试按 Godot 虚拟路径解析
        string global = ProjectSettings.GlobalizePath(path);
        if (!string.IsNullOrEmpty(global))
            return Path.GetFullPath(global);

        // 兜底：当作相对路径基于当前工作目录
        try
        {
            return Path.GetFullPath(path);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 检查指定路径是否存在 的文件。
    /// </summary>
    /// <param name="path">目录路径（Godot 虚拟路径或本地路径）</param>
    /// <returns>是否存在文件</returns>
    public static bool Exist(string path)
    {
        string? absDir = Parse(path);
        return absDir != null && File.Exists(absDir);
    }

    /// <summary>
    /// 读取目标文件全部文本内容（UTF-8）。
    /// </summary>
    /// <param name="path">文件路径（Godot 虚拟路径或本地路径）</param>
    /// <returns>文件内容；若失败返回 null</returns>
    public static string? Read(string path)
    {
        string? absPath = Parse(path);
        if (absPath == null)
            return null;

        try
        {
            return File.ReadAllText(absPath, System.Text.Encoding.UTF8);
        }
        catch
        {
            return null;
        }
    }

    public static string[] ReadLines(string path)
    {
        string? absPath = Parse(path);
        if (absPath == null)
            return [];

        try
        {
            return File.ReadAllLines(absPath, System.Text.Encoding.UTF8);
        }
        catch
        {
            return [];
        }      
    }

    /// <summary>
    /// 将文本内容写入目标文件（UTF-8），若目录不存在则自动创建；已存在则覆盖。
    /// </summary>
    /// <param name="path">目标文件路径（Godot 虚拟路径或本地路径）</param>
    /// <param name="content">要写入的文本内容</param>
    /// <returns>是否写入成功</returns>
    public static bool Write(string path, string? content)
    {
        string? absPath = Parse(path);
        if (absPath == null)
            return false;

        try
        {
            string? dir = Path.GetDirectoryName(absPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(absPath, content ?? string.Empty, System.Text.Encoding.UTF8);
            return true;
        }
        catch
        {
            return false;
        }
    }
}