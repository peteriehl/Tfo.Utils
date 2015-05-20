using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tfo.Utils.IO
{
    public class StreamReaderExtension
    {
        public IEnumerable<string> GetLines(string filePath)
        {
            string line;
            using (var reader = File.OpenText(filePath))
            {
                while((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
