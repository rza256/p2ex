using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2Ex.Models
{
    internal class Media
    {
        public string filePath { get; set; }
        public string[] leftAndRight { get; set; } = new string[2];
        public EncodeOptions encodeOptions { get; set; }

        public override string ToString()
        {
            string output = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            // Console.WriteLine(output);

            // for some reason panasonic doesn't use the 3rd or 4th audio channels,
            // which wastes a WHOLE bunch of space on the P2 card

            // according to VLC it is all pcm_s16le for .mxf audio files
            string outName = Path.ChangeExtension(Path.GetFileName(filePath), ".mp4");
            string preset = "medium";
            string codec = "libx265";

            switch(encodeOptions.quality)
            {
                case EncodeOptions.Quality.Lossless:
                    preset = "slower";
                    break;
                case EncodeOptions.Quality.High:
                    preset = "slow";
                    break;
                case EncodeOptions.Quality.Medium:
                    preset = "medium";
                    break;
                case EncodeOptions.Quality.Low:
                    preset = "fast";
                    break;
            }

            switch (encodeOptions.codec)
            {
                case EncodeOptions.Codec.H264:
                    codec = "libx264";
                    break;
                case EncodeOptions.Codec.H265:
                    codec = "libx265";
                    break;
            }

            string cli = $@"-i {"\"" + filePath + "\""} -i {"\"" + leftAndRight[0] + "\""} -i {"\"" + leftAndRight[1] + "\""}
                  -filter_complex ""[1:a][2:a]join=inputs=2:channel_layout=stereo[a]""
                  -map 0:v -map ""[a]""
                  -c:v {codec} -crf 26 -preset {preset} -pix_fmt yuv420p -tag:v hvc1
                  -c:a aac -b:a 256k
                  {"\"" + Path.Combine(output, outName) + "\""}";

            return cli.Replace("\r\n", " ");
        }

        /*
        ffmpeg -i video.mp4 -i left.wav -i right.wav \
  -filter_complex "[1:a][2:a]join=inputs=2:channel_layout=stereo[a]" \
  -map 0:v -map "[a]" \
  -c:v copy -c:a aac -b:a 256k \
  output.mp4
         */
    }
}
