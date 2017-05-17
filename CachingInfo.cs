namespace Olbert.Wix.Deprecated
{
    public class CachingInfo
    {
        public string PackageOrContainerID { get; set; }
        public string PayloadID { get; set; }
        public int CachingPercent { get; set; }
        public int OverallPercent { get; set; }
        public long BytesCached { get; set; }
        public long BytesToCache { get; set; }
    }
}