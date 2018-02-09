using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Nop.Web.Framework.UI
{
    public static class ResourceVersionManager
    {
        private static ConcurrentDictionary<string, string> resourceDic = new ConcurrentDictionary<string, string>();

        public static ConcurrentDictionary<string,string> ResourceDic
        {
            get { return resourceDic; }
        }

        public static void AddResource(string absoluteFilePath) {
            absoluteFilePath = absoluteFilePath.ToUpper();
            if (!resourceDic.Keys.Contains(absoluteFilePath))
            { 
                if (File.Exists(absoluteFilePath))
                {
                    //var second = (File.GetLastWriteTime(absoluteFilePath) - new DateTime(1970, 1, 1)).Seconds;
                    var second = (File.GetLastWriteTime(absoluteFilePath)).ToString("yyyyMMddHHmmss"); 
                    resourceDic.TryAdd(absoluteFilePath, second.ToString());
                }
            }
        }

    }
}
