using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2Ex.Models
{
    internal class EncodeOptions
    {
        public EncodeOptions(Quality quality, FileSize fileSize, Codec codec)
        {
            this.quality = quality;
            this.fileSize = fileSize;
            this.codec = codec;
        }

        public Quality quality { get; }
        public FileSize fileSize { get; }
        public Codec codec { get; }

        public enum Quality
        {
            Low,
            Medium,
            High,
            Lossless
        }

        public enum FileSize
        {
            Low,
            Medium,
            High
        }

        public enum Codec
        {
            H264,
            H265,
            ProRes,
        }
    }
}
