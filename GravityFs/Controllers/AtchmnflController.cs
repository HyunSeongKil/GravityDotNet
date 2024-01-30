using System.IO.Compression;
using System.Web;
using GravityCmmn.Misc;
using GravityFs.Domains;
using GravityFs.Models;
using GravityFs.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLog;

namespace GravityFs.Controllers;

[ApiController]
[Route("gravity-fs/[controller]")]
public class AtchmnflController(GravityfsdbContext dbContext, IFileService fileService, IAtchmnflGroupService atchmnflGroupService, IAtchmnflService atchmnflService) : ControllerBase
{
    readonly Logger logger = LogManager.GetCurrentClassLogger();


    /// <summary>
    /// atchmnflGroupId가 없는 경우에는 새로운 atchmnflGroupId를 생성하여 등록한다.
    /// </summary>
    /// <param name="bizKey"></param>
    /// <param name="mfiles"></param>
    /// <returns></returns>
    [HttpPost]
    public GcResultMap Regist([FromQuery] string bizKey, List<IFormFile> mfiles)
    {
        string atchmnflGroupId = IAtchmnflGroupService.CreateId();
        //
        atchmnflGroupId = atchmnflGroupService.Regist(dbContext, new()
        {
            AtchmnflGroupId = atchmnflGroupId,
            BizKey = bizKey,
            RegisterId = "TODO",
        });


        try
        {
            IEnumerable<IDictionary<string, string>> data = mfiles.Select(mfile =>
            {
                string atchmnflId = RegistFileAndData(bizKey, mfile, atchmnflGroupId);

                return new Dictionary<string, string>()
                {
                    ["atchmnflGroupId"] = atchmnflGroupId,
                    ["atchmnflId"] = atchmnflId,
                };
            });

            dbContext.SaveChanges();

            return GcResultMap.WithData(data);
        }
        catch (Exception ex)
        {
            var entity = dbContext.AtchmnflGroups.Find(atchmnflGroupId);
            if (entity != null)
            {
                dbContext.Atchmnfls.Where(x => x.AtchmnflGroupId.Equals(atchmnflGroupId)).ToList().ForEach(entity => fileService.DeleteFile(entity.SaveSubPath, entity.SaveFilename));
                dbContext.Atchmnfls.RemoveRange(dbContext.Atchmnfls.Where(entity => entity.AtchmnflGroupId.Equals(atchmnflGroupId)));
                dbContext.AtchmnflGroups.Remove(entity);
                dbContext.SaveChanges();
            }

            logger.Error("{}", ex);
            return GcResultMap.WithCode(ex.Message, ex.Message);
        }
    }

    /// <summary>
    /// atchmnflGroupId가 있는 경우에는 해당 atchmnflGroupId에 파일 목록만 등록한다.
    /// </summary>
    /// <param name="bizKey"></param>
    /// <param name="atchmnflGroupId"></param>
    /// <param name="mfiles"></param>
    /// <returns></returns>
    [HttpPut]
    public GcResultMap Update([FromQuery] string bizKey, [FromQuery] string atchmnflGroupId, List<IFormFile> mfiles)
    {
        try
        {
            IEnumerable<IDictionary<string, string>> data = mfiles.Select(mfile =>
            {
                string atchmnflId = RegistFileAndData(bizKey, mfile, atchmnflGroupId);

                return new Dictionary<string, string>()
                {
                    ["atchmnflGroupId"] = atchmnflGroupId,
                    ["atchmnflId"] = atchmnflId,
                };
            });

            dbContext.SaveChanges();

            return GcResultMap.WithData(data);
        }
        catch (Exception ex)
        {
            logger.Error("{}", ex);
            return GcResultMap.WithCode(ex.Message, ex.Message);
        }
    }


    private string RegistFileAndData(string bizKey, IFormFile formFile, string atchmnflGroupId)
    {
        string atchmnflId = IAtchmnflService.CreateId();

        //
        IDictionary<string, string> dic = fileService.SaveFile(bizKey, formFile, atchmnflGroupId, atchmnflId);

        //
        atchmnflId = atchmnflService.Regist(dbContext, new AtchmnflDto()
        {
            AtchmnflId = atchmnflId,
            AtchmnflGroupId = atchmnflGroupId,
            ContentType = formFile.ContentType,
            FileSize = (int)formFile.Length,
            OriginalFilename = formFile.FileName,
            SaveFilename = dic["SaveFilename"],
            SaveSubPath = dic["SaveSubPath"],
            RegisterId = "TODO",
        });

        return atchmnflId;
    }

    [HttpDelete]
    [Route("/{atchmnflId}")]
    public GcResultMap DeleteById(string atchmnflId)
    {
        var entity = dbContext.Atchmnfls.Find(atchmnflId);
        if (entity != null)
        {
            dbContext.Atchmnfls.Remove(entity);
            dbContext.SaveChanges();
        }

        return GcResultMap.Empty();
    }

    [HttpDelete]
    [Route("/parent/{atchmnflGroupId}")]
    public GcResultMap DeletesByAtchmnflGroupId(string atchmnflGroupId)
    {

        var entity = dbContext.AtchmnflGroups.Find(atchmnflGroupId);
        if (entity != null)
        {
            // ! 디비 처리 오류 발생을 대비해 삭제 표시만 함.             
            dbContext.Atchmnfls.Where(x => x.AtchmnflGroupId.Equals(atchmnflGroupId)).ToList().ForEach(entity => fileService.MarkDeletedFile(entity.SaveSubPath!, entity.SaveFilename!));
            dbContext.Atchmnfls.RemoveRange(dbContext.Atchmnfls.Where(entity => entity.AtchmnflGroupId.Equals(atchmnflGroupId)));
            dbContext.AtchmnflGroups.Remove(entity);
            dbContext.SaveChanges();
        }

        return GcResultMap.Empty();
    }

    [HttpGet]
    [Route("/{atchmnflId}")]
    public GcResultMap GetById(string atchmnflId)
    {
        return GcResultMap.WithData(dbContext.Atchmnfls.Find(atchmnflId));
    }

    [HttpGet]
    [Route("/parent/{atchmnflGroupId}")]
    public GcResultMap GetsByAtchmnflGroupId(string atchmnflGroupId)
    {
        return GcResultMap.WithData(dbContext.Atchmnfls.Where(entity => entity.AtchmnflGroupId.Equals(atchmnflGroupId)).ToList());
    }

    [HttpGet]
    [Route("/parent/{atchmnflGroupId}/first")]
    public GcResultMap GetFirstByAtchmnflGroupId(string atchmnflGroupId)
    {
        var entities = dbContext.Atchmnfls.Where(entity => entity.AtchmnflGroupId.Equals(atchmnflGroupId)).ToList();

        return entities.Count > 0 ? GcResultMap.WithData(entities[0]) : GcResultMap.Empty();
    }

    [HttpGet]
    [Route("/parent/{atchmnflGroupId}/files")]
    public IActionResult GetFilesByAtchmnflGroupId(string atchmnflGroupId)
    {
        DeleteOldZipFileAsync();

        //
        var entities = dbContext.Atchmnfls.Where(entity => entity.AtchmnflGroupId.Equals(atchmnflGroupId)).ToList();
        if (entities.Count == 0)
        {
            throw new Exception("entities is empty");
        }

        var zipFile = fileService.CreateZipFile(entities);

        //
        logger.Debug("zipFile: {}", zipFile);
        return new FileStreamResult(zipFile.OpenRead(), "application/zip")
        {
            FileDownloadName = HttpUtility.UrlEncode(zipFile.Name, System.Text.Encoding.UTF8),
        };
    }

    private void DeleteOldZipFileAsync()
    {
        Task.Run(() =>
        {
            string path = Path.Combine(Path.GetTempPath(), "download");
            Directory.GetFiles(path, "*.zip", SearchOption.AllDirectories).ToList().ForEach(f =>
            {
                // 24시간전 파일이면 삭제
                if (System.IO.File.GetLastWriteTime(f) < DateTime.Now.AddDays(-1))
                {
                    System.IO.File.Delete(f);
                }
            });
        });

    }


    [HttpGet]
    [Route("/parent/{atchmnflGroupId}/first/file")]
    public IActionResult GetFirstFileByAtchmnflGroupId(string atchmnflGroupId)
    {
        var entities = dbContext.Atchmnfls.Where(entity => entity.AtchmnflGroupId.Equals(atchmnflGroupId)).ToList();
        if (entities.Count == 0)
        {
            throw new Exception("존재하지 않는 파일입니다.");
        }

        FileInfo fileInfo = fileService.GetFile(entities[0].SaveSubPath!, entities[0].SaveFilename!);

        return new FileStreamResult(fileInfo.OpenRead(), entities[0].ContentType ?? "application/octet-stream")
        {
            FileDownloadName = HttpUtility.UrlEncode(entities[0].OriginalFilename, System.Text.Encoding.UTF8),
        };
    }

    [HttpGet]
    [Route("/{atchmnflId}/file")]
    public IActionResult GetFile(string atchmnflId)
    {
        var entity = dbContext.Atchmnfls.Find(atchmnflId) ?? throw new Exception("존재하지 않는 파일입니다.");

        FileInfo fileInfo = fileService.GetFile(entity.SaveSubPath!, entity.SaveFilename!);

        return new FileStreamResult(fileInfo.OpenRead(), entity.ContentType ?? "application/octet-stream")
        {
            FileDownloadName = HttpUtility.UrlEncode(entity.OriginalFilename, System.Text.Encoding.UTF8),
        };

    }
}
