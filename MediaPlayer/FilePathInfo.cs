using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media
{
    class FilePathInfo
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory + @"\MediaPlayer";
        public static string Next = Path + @"\Icons\Next.png";
        public static string Prev = Path + @"\Icons\Previous.png";
        public static string Play = Path + @"\Icons\Play.png";
        public static string Pause = Path + @"\Icons\Pause.png";

        public static string AudioFile = Path + @"\Media Information\AudioInformation.txt";
        public static string VideoFile = Path + @"\Media Information\VideoInformation.txt";
    }
}
