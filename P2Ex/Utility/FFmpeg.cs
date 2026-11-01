using P2Ex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace P2Ex.Utility
{
    internal class FFmpeg
    {
        private string ffmpegPath;
        public FFmpeg(string ffmpegPath) 
        { 
            this.ffmpegPath = ffmpegPath;
        }

        public static void downloadFFmpeg(string path)
        {
            //
            string zip = Path.Combine(path, "ffmpeg.zip");
            string ffmpeg = Path.Combine(path, "ffmpeg");
            Directory.CreateDirectory(path);

            // download
            using (var client = new WebClient())
            {
                Console.WriteLine("downloading ffmpeg... https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip");
                client.DownloadFile("https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip", zip);

            }

            // extract
            Console.WriteLine("extracting ffmpeg...");

            if (Directory.Exists(ffmpeg))
                Directory.Delete(ffmpeg, true);
            System.IO.Compression.ZipFile.ExtractToDirectory(zip, ffmpeg);
            Console.WriteLine("extracted ffmpeg!");
        }

        public void ProcessClip(Media media, EncodeOptions encodeOptions)
        {
            Console.Write(media.filePath);

            // var process = Process.Start(ffmpegPath, "-h");
            //process.WaitForExit();
            LoadingDot loadingDot = new LoadingDot();

            var psi = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = media.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            using (var process = new Process { StartInfo = psi })
            {
                // process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                // process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }

            loadingDot.Stop();
        }
    }
}
