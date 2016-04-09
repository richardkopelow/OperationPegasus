using System.IO;

namespace Standard
{
    public class File
    {
        string path;

        public File(string filePath)
        {
            path = filePath;
        }

        public string Read()
        {
            using (StreamReader sr=new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }
        public void Write(string text)
        {
            using (StreamWriter sw=new StreamWriter(path))
            {
                sw.Write(text);
            }
        }
        public void WriteLine(string text)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(text);
            }
        }
        public void Delete()
        {
            FileInfo fi = new FileInfo(path);
            fi.Delete();
        }
    }
}
