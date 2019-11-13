using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace Twitch_API
{
    class Downloader
    {
        private string _videoURL;
        private Process _proc;


        public Downloader(string url)
        {
            _videoURL = url;
            var thread = new Thread(StartProc);
            thread.Start();
        }

        private void StartProc()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            _proc = new Process
            {
                StartInfo =
                {
                    FileName = "ffmpeg.exe",
                    Arguments = "-i \"" + _videoURL + "\" -acodec copy -vcodec copy -absf aac_adtstoasc \"" +
                                localFolder.Path + "\\myfuckingvideo.mp4",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true
                },
                EnableRaisingEvents = true,

            };

            _proc.Start();
            using (var reader = _proc.StandardError)
            {
                var thisline = reader.ReadLine();
                while (true)
                {
                    Debug.WriteLine(thisline);
                    thisline = reader.ReadLine();
                }
            }
        }
        public void Stop()
        {
            var buffer = _proc.StandardInput;
            buffer.WriteLineAsync("q");
        }
    }
}

