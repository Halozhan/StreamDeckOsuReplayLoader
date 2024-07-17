using System;
using System.Diagnostics;
using System.IO;

namespace StreamDeckOsuReplayLoader
{
    internal class OsuReplays
    {
        public static void LoadRecentReplay(String replayPath)
        {
            // ref: http://www.gisdeveloper.co.kr/?p=2044
            DirectoryInfo di = new DirectoryInfo(replayPath);
            string LatestReplay = string.Empty;
            DateTime LatestReplayTime = new DateTime();

            foreach (FileInfo file in di.GetFiles())
            {
                if (file.Extension.ToLower().CompareTo(".osr") == 0)
                {
                    if (LatestReplayTime <= file.LastWriteTimeUtc)
                    {
                        LatestReplay = file.FullName;
                        LatestReplayTime = file.LastWriteTimeUtc;
                    }
                }
            }
            Process LatestOsr = new Process();
            LatestOsr.StartInfo.FileName = LatestReplay;
            LatestOsr.Start();
        }
    }
}
