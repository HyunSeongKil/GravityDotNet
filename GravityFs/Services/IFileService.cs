using GravityFs.Models;

namespace GravityFs.Services;

public interface IFileService
{

  string GetExtension(string contentType);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="bizKey"></param>
  /// <param name="formFile"></param>
  /// <returns>{SaveSubPath:string, SaveFilename:string}</returns>
  IDictionary<string, string> SaveFile(string bizKey, IFormFile formFile, string atchmnflGroupId, string atchmnflId);

  void DeleteFile(Atchmnfl atchmnfl);
  void DeleteFile(string fullPath);

  void DeleteFile(string saveSubPath, string saveFilename);


  /// <summary>
  /// 삭제 표시해 놓음. 나중에 스케줄러가 삭제해야 함. 삭제 표시된 파일명: 원래파일명.deleted
  /// </summary>
  /// <param name="saveSubPath"></param>
  /// <param name="saveFilename"></param>
  void MarkDeletedFile(string saveSubPath, string saveFilename);
  void RestoreDeletedFile(string saveSubPath, string saveFilename);

  FileInfo GetFile(string saveSubPath, string saveFilename);

  FileInfo CreateZipFile(IList<Atchmnfl> entites);

}