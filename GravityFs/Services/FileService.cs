using System.IO.Compression;
using GravityFs.Models;
using NLog;

namespace GravityFs.Services;

public class FileService(IConfiguration config) : IFileService
{
  readonly Logger logger = LogManager.GetCurrentClassLogger();
  readonly string? savePath = config.GetValue<string>("App:SavePath");


  readonly IDictionary<string, string> dic = new Dictionary<string, string>(){
    {"image/jpeg", ".jpeg"},
    {"image/jpg", ".jpeg"},
  };

  public FileInfo CreateZipFile(IList<Atchmnfl> entities)
  {
    if (entities.Count == 0)
    {
      throw new Exception("entites is empty");
    }

    //
    string ymdhms = DateTime.Now.ToString("yyyyMMddHHmmss");
    string path = Path.Combine(Path.GetTempPath(), "download", ymdhms);
    Directory.CreateDirectory(path);

    //
    foreach (var entity in entities)
    {
      string destFileName = Path.Combine(path, entity.SaveFilename! + "" + Path.GetExtension(entity.OriginalFilename));
      GetFile(entity.SaveSubPath!, entity.SaveFilename!).CopyTo(destFileName);

      logger.Debug("{}", destFileName);
    }


    //
    string zipFileName = $"{ymdhms}_{entities.Count}ea.zip";
    ZipFile.CreateFromDirectory(path, Path.Combine(path, "..", zipFileName));

    // 복사된 파일&디렉터리 삭제
    Directory.Delete(path, true);

    //
    return new FileInfo(Path.Combine(path, "..", zipFileName));
  }

  public void DeleteFile(string fullPath)
  {
    File.Delete(fullPath);
  }

  public void DeleteFile(Atchmnfl atchmnfl)
  {
    if (savePath == null || atchmnfl == null)
    {
      return;
    }

    DeleteFile(Path.Combine(savePath, atchmnfl.SaveSubPath!, atchmnfl.SaveFilename!));
  }

  public void DeleteFile(string? saveSubPath, string? saveFilename)
  {
    if (savePath == null || saveSubPath == null || saveFilename == null)
    {
      return;
    }

    DeleteFile(Path.Combine(savePath!, saveSubPath, saveFilename));
  }

  public string GetExtension(string contentType)
  {
    return dic.ContainsKey(contentType) ? dic[contentType] : string.Empty;
  }

  public FileInfo GetFile(string saveSubPath, string saveFilename)
  {
    return new FileInfo(Path.Combine(savePath!, saveSubPath, saveFilename));
  }

  public void MarkDeletedFile(string saveSubPath, string saveFilename)
  {
    if (savePath == null)
    {
      throw new Exception("savePath is null");
    }

    string sourceFileName = Path.Combine(savePath, saveSubPath, saveFilename);
    string destFileName = Path.Combine(savePath, saveSubPath, saveFilename + ".deleted");

    File.Move(sourceFileName, destFileName);

  }

  public void RestoreDeletedFile(string saveSubPath, string saveFilename)
  {
    if (savePath == null)
    {
      throw new Exception("savePath is null");
    }

    string sourceFileName = Path.Combine(savePath, saveSubPath, saveFilename + ".deleted");
    string destFileName = Path.Combine(savePath, saveSubPath, saveFilename);

    File.Move(sourceFileName, destFileName);
  }

  public IDictionary<string, string> SaveFile(string bizKey, IFormFile formFile, string atchmnflGroupId, string atchmnflId)
  {
    if (savePath == null)
    {
      throw new Exception("savePath is null");
    }

    string saveSubPath = Path.Combine(bizKey, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));
    string saveFilename = $"{atchmnflGroupId}_{atchmnflId}." + formFile.ContentType.Replace("/", "_");
    Directory.CreateDirectory(Path.Combine(savePath, saveSubPath));

    string path = Path.Combine(savePath!, saveSubPath, saveFilename);

    using var stream = new FileStream(path, FileMode.Create);
    formFile.CopyTo(stream);



    return new Dictionary<string, string>()
    {
      {"SaveSubPath", saveSubPath},
      {"SaveFilename", saveFilename}
    };
  }
}