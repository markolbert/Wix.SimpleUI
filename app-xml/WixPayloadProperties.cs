namespace Olbert.Wix
{
    public class WixPayloadProperties
    {
        public string Payload { get; set; }
        public string Package { get; set; }
        public string Container { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string DownloadUrl { get; set; }
        public bool LayoutOnly { get; set; }
    }
}