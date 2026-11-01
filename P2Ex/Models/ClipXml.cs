using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

// THIS IS BASED OFF A HVX-200P'S XML!!!
// NEWER DEVICES MIGHT NOT WORK WITH THIS CODE, AND MAY REQUIRE MODIFICATION

namespace P2Ex.Models
{
    internal static class P2
    {
        public const string Ns = "urn:schemas-Professional-Plug-in:P2:ClipMetadata:v3.0";
    }

    [XmlRoot("P2Main", Namespace = P2.Ns)]
    [XmlType(Namespace = P2.Ns)]
    public class P2Main
    {
        [XmlElement("ClipContent")]
        public ClipContent ClipContent { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class ClipContent
    {
        [XmlElement("ClipName")]
        public string ClipName { get; set; }

        [XmlElement("GlobalClipID")]
        public string GlobalClipID { get; set; }

        [XmlElement("Duration")]
        public long Duration { get; set; }

        // e.g. "1001/60000"
        [XmlElement("EditUnit")]
        public string EditUnit { get; set; }

        [XmlElement("EssenceList")]
        public EssenceList EssenceList { get; set; }

        [XmlElement("ClipMetadata")]
        public ClipMetadata ClipMetadata { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class EssenceList
    {
        [XmlElement("Video")]
        public Video Video { get; set; }

        [XmlElement("Audio")]
        public List<Audio> Audio { get; set; } = new List<Audio>();
    }

    [XmlType(Namespace = P2.Ns)]
    public class Video
    {
        [XmlAttribute("ValidAudioFlag")]
        public bool ValidAudioFlag { get; set; }

        [XmlElement("VideoFormat")]
        public string VideoFormat { get; set; }

        [XmlElement("Codec")]
        public string Codec { get; set; }

        [XmlElement("FrameRate")]
        public FrameRate FrameRate { get; set; }

        [XmlElement("StartTimecode")]
        public string StartTimecode { get; set; }

        [XmlElement("StartBinaryGroup")]
        public string StartBinaryGroup { get; set; }

        [XmlElement("AspectRatio")]
        public string AspectRatio { get; set; }

        [XmlElement("VideoIndex")]
        public EssenceIndex VideoIndex { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class FrameRate
    {
        [XmlAttribute("DropFrameFlag")]
        public bool DropFrameFlag { get; set; }

        // e.g. "59.94p"
        [XmlText]
        public string Value { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class Audio
    {
        [XmlElement("AudioFormat")]
        public string AudioFormat { get; set; }

        [XmlElement("SamplingRate")]
        public int SamplingRate { get; set; }

        [XmlElement("BitsPerSample")]
        public int BitsPerSample { get; set; }

        [XmlElement("AudioIndex")]
        public EssenceIndex AudioIndex { get; set; }
    }

    // Shared by VideoIndex and AudioIndex
    [XmlType(Namespace = P2.Ns)]
    public class EssenceIndex
    {
        [XmlElement("StartByteOffset")]
        public long StartByteOffset { get; set; }

        [XmlElement("DataSize")]
        public long DataSize { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class ClipMetadata
    {
        [XmlElement("UserClipName")]
        public string UserClipName { get; set; }

        [XmlElement("DataSource")]
        public string DataSource { get; set; }

        [XmlElement("Access")]
        public Access Access { get; set; }

        [XmlElement("Device")]
        public Device Device { get; set; }

        [XmlElement("Shoot")]
        public Shoot Shoot { get; set; }

        [XmlElement("Thumbnail")]
        public Thumbnail Thumbnail { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class Access
    {
        [XmlElement("CreationDate")]
        public DateTime CreationDate { get; set; }

        [XmlElement("LastUpdateDate")]
        public DateTime LastUpdateDate { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class Device
    {
        [XmlElement("Manufacturer")]
        public string Manufacturer { get; set; }

        // The element name really is "SerialNo."
        [XmlElement("SerialNo.")]
        public string SerialNo { get; set; }

        [XmlElement("ModelName")]
        public string ModelName { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class Shoot
    {
        [XmlElement("StartDate")]
        public DateTime StartDate { get; set; }

        [XmlElement("EndDate")]
        public DateTime EndDate { get; set; }
    }

    [XmlType(Namespace = P2.Ns)]
    public class Thumbnail
    {
        [XmlElement("FrameOffset")]
        public int FrameOffset { get; set; }

        [XmlElement("ThumbnailFormat")]
        public string ThumbnailFormat { get; set; }

        [XmlElement("Width")]
        public int Width { get; set; }

        [XmlElement("Height")]
        public int Height { get; set; }
    }
}