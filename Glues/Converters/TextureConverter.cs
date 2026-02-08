using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

internal class TextureConverter
{
    #region read
    public Texture2D PathToSingleTexture(string path)
    {
        if (!AccessUtil.FileExists(path,out var absPath,GameSettingsStorage.UserLevelRootPath))
            throw new Exception($"文件 {absPath} 未找到");
        var texture = ResourceLoader.Load<Texture2D>(absPath);
        return texture ?? throw new Exception($"文件 {absPath} 已损坏，无法加载");
    }

    public Dictionary<string, Texture2D> FolderToTextureMap(string path)
    {
        Dictionary<string,Texture2D> map = [];
        if (!AccessUtil.FolderExists(path,out var absPath,GameSettingsStorage.UserLevelRootPath))
            throw new Exception($"文件夹 {absPath} 未找到");

        var folder = DirAccess.Open(absPath);
        if (folder is null)
            throw new Exception($"文件夹 {absPath} 已损坏，无法加载");

        var files = folder.GetFiles();
        foreach (var fileName in files)
        {
            if (string.IsNullOrWhiteSpace(fileName)) continue;
            var ext = fileName.GetExtension().ToLower();
            if (!Constants.SUPPORTED_IMAGE_EXTENSIONS.Contains(ext)) continue;
            var fullPath = absPath.PathJoin(fileName);
            try
            {
                var texture = PathToSingleTexture(fullPath);
                var baseName = fileName.GetBaseName();
                map[baseName] = texture;
            }
            catch
            {
                // ignored
            }
        }
        return map;
    }
    #endregion

    #region write
    
    #endregion
}