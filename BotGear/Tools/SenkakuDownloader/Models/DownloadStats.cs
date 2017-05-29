using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneRandomImageDownloader.Sankaku.Models
{
    public class DownloadStats
    {
        public int PostsFound { get; set; } = 0;
        public int PostsDownloaded { get; set; } = 0;
        public bool WasCancelled { get; set; } = false;
    }
}
