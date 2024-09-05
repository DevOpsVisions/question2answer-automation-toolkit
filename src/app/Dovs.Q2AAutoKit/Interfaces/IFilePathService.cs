using System;

namespace Dovs.Q2AAutoKit.Interfaces
{
    public interface IFilePathService
    {
        string GetBasePath(int levelsToTraverse);
        string GetFilePath(string defaultFilePath);
        string[] GetExcelFiles(string basePath);
    }
}
