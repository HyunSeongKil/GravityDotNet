using NLog;
using Quartz;

namespace GravityFs.Services;

public class DeleteOldDeletedFilesJob(IConfiguration config) : IJob
{
    readonly Logger logger = LogManager.GetCurrentClassLogger();
    readonly string? savePath = config.GetValue<string>("App:SavePath");

    public Task Execute(IJobExecutionContext context)
    {
        logger.Info(">> path: {}", savePath);

        if (savePath == null)
        {
            logger.Warn("<< savePath is null");
            return Task.CompletedTask;
        }

        int cnt = 0;
        Directory.GetFiles(savePath, "*.deleted", SearchOption.AllDirectories).ToList().ForEach(f =>
        {
            // 24시간전 파일이면 삭제
            if (File.GetLastWriteTime(f) < DateTime.Now.AddDays(-1))
            {
                File.Delete(f);
                cnt++;
                logger.Info("deleted {}", f);
            }
        });

        logger.Info("<< {} deleted", cnt);

        return Task.CompletedTask;
    }

}