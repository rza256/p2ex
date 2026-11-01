// This isn't the best code I will ever write

using P2Ex.Models;
using P2Ex.Utility;
using System.IO;
using System.Xml.Serialization;

namespace P2Ex
{
    class P2Ex
    {

        public static void Main(string[] args)
        {
            // temp
#if DEBUG
            string baseDir = "D:\\P2\\October 24 2026";
#endif

#if RELEASE
            string baseDir = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
#endif

            if (!Directory.Exists(baseDir))
            {
                return;
            }

            if (!File.Exists(Path.Combine(baseDir, "LASTCLIP.TXT")))
            {
                Console.WriteLine("LASTCLIP.TXT does not exist. Did you put the executable in the right directory?");
                return;
            }

            // Preliminary checks, does ffmpeg exist?
            string tempPath = Path.Combine(System.IO.Path.GetTempPath(), "p2ex");
            var allFiles = Directory.GetFiles(tempPath, "*.exe", SearchOption.AllDirectories);
            string ffmpegPath = "";

            foreach (string file in allFiles)
            {
                if (file.Contains("ffmpeg"))
                {
                    Console.WriteLine("found ffmpeg: " + file);
                    ffmpegPath = file;
                    break;
                }
            }

            if (ffmpegPath == "")
            {
                Console.WriteLine("ffmpeg not found. download ffmpeg @ " + tempPath + "? y/n");
                string input_ = Console.ReadLine();
                if (input_.ToLower() == "y")
                {
                    Console.WriteLine("downloading ffmpeg...");
                    Utility.FFmpeg.downloadFFmpeg(tempPath);
                    Console.WriteLine("ffmpeg downloaded");
                    ffmpegPath = Directory.GetFiles(tempPath, "*.exe", SearchOption.AllDirectories).FirstOrDefault(f => f.Contains("ffmpeg"));
                }
                else
                {
                    Console.WriteLine("exiting");
                }
            }

            FFmpeg ffmpeg = new FFmpeg(ffmpegPath);

            string[] paths = Directory.GetFiles(Path.Combine(baseDir, "CONTENTS/CLIP"), "*.*", SearchOption.AllDirectories);
            string[] validClips = { };
            string camera = "???";
            long timeTotal = 0;
            foreach (string path in paths)
            {
                // Console.WriteLine(path);
                string testData = File.ReadAllText(path);

                XmlSerializer serializer = new XmlSerializer(typeof(P2Main));
                using (TextReader reader = new StringReader(testData))
                {
                    P2Main result = (P2Main)serializer.Deserialize(reader);
                    // Console.WriteLine(result.ClipContent.ClipName + " : " + TimeSpan.FromMilliseconds(result.ClipContent.Duration).TotalSeconds + "s");
                    // Console.WriteLine("shot on " + result.ClipContent.ClipMetadata.Device.ModelName);

                    // for each correlating clip, eg. 00004NH.MXF
                    // - there should be: 00004NH00.MXF (L) 00004NH01.MXF (R)
                    // - im not sure what channels 02 and 03 are for, but they exist on the HVX-200P

                    if (File.Exists(Path.Combine(baseDir, "CONTENTS/AUDIO", result.ClipContent.ClipName + "00.MXF")) &&
                        File.Exists(Path.Combine(baseDir, "CONTENTS/AUDIO", result.ClipContent.ClipName + "01.MXF")))
                    {
                        // Console.WriteLine("found correlating L & R clip: " + result.ClipContent.ClipName);
                        validClips = validClips.Append(result.ClipContent.ClipName).ToArray();
                    }
                    else
                    {
                        Console.WriteLine("warn: clip " + result.ClipContent.ClipName + " is missing correlating MXF files");
                    };

                    camera = result.ClipContent.ClipMetadata.Device.ModelName;
                    timeTotal += result.ClipContent.Duration;
                }
            }

            Console.Write($"\nValid clips ({ validClips.Length }): ");
            foreach (string clip in validClips)
            {
                Console.Write(clip + " ");
            }

            Console.WriteLine();

            if (!Utility.Comaptibility.IsCameraSupported(camera))
            {
                Console.WriteLine("warn: camera " + camera + " has not been tested");
            }

            Console.WriteLine("Camera: Panasonic " + camera);
            Console.WriteLine("All clips: " + TimeSpan.FromMilliseconds(timeTotal).TotalSeconds + "s\n");

            Console.WriteLine("L: low quality M: medium quality (default) H: high quality L: lossless");
            Console.WriteLine("4: H.264 5: H.265 (default)");
            Console.Write("enter (e.g: L4): ");

            string input = Console.ReadLine();
            EncodeOptions.Quality quality = (input[0] == 'L') 
                ? EncodeOptions.Quality.Low : (input[0] == 'M') 
                ? EncodeOptions.Quality.Medium : (input[0] == 'H') 
                ? EncodeOptions.Quality.High : EncodeOptions.Quality.Lossless;

            // unused actually
            EncodeOptions.FileSize fileSize = EncodeOptions.FileSize.High;

            EncodeOptions.Codec codec = (input[1] == 4)
                ? EncodeOptions.Codec.H264 : (input[1] == 5)
                ? EncodeOptions.Codec.H265 : EncodeOptions.Codec.H265;

            EncodeOptions options = new EncodeOptions(quality, fileSize, codec);

            Console.WriteLine("\nUsing these settings:\nCodec: " + codec + "\nFilesize: " + fileSize + "\nQuality: " + quality + "\n");
        
            foreach(string file in validClips)
            {
                Media media = new Media
                {
                    filePath = Path.Combine(baseDir, "CONTENTS/VIDEO", file + ".MXF"),
                    leftAndRight = new string[2] { Path.Combine(baseDir, "CONTENTS/AUDIO", file + "00.MXF"), Path.Combine(baseDir, "CONTENTS/AUDIO", file + "01.MXF") },
                    encodeOptions = options
                };

                Console.Write("Processing " + file + ": ");
                ffmpeg.ProcessClip(media, options);
                Console.Write("\n");
            }
        }
    }
}