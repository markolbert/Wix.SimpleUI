
// Copyright (c) 2017 Mark A. Olbert some rights reserved
//
// This software is licensed under the terms of the MIT License
// (https://opensource.org/licenses/MIT)

namespace Olbert.Wix.Deprecated
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