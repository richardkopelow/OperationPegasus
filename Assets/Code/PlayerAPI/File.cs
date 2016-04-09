using System.IO;

namespace Standard
{
    public class File
    {
        string path;
        public string Read()
        {
            using (StreamReader sr=new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
