using System.IO;

namespace Standard
{
    public class File
    {
        string path;
        string drive;

        public File(string filePath)
        {
            path = filePath;
            drive = path.Split(':')[0].ToLower();
        }

        public string Read()
        {
            if (drive!="q")
            {
                Console.WriteLine("You do not have rights outside of Q:");
                return "";
            }
            using (StreamReader sr=new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }
        public void Write(string text)
        {
            if (drive != "q")
            {
                Console.WriteLine("You do not have rights outside of Q:");
                return;
            }
            using (StreamWriter sw=new StreamWriter(path))
            {
                sw.Write(text);
            }
        }
        public void WriteLine(string text)
        {
            if (drive != "q")
            {
                Console.WriteLine("You do not have rights outside of Q:");
                return;
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(text);
            }
        }
        public void Delete()
        {
            if (drive != "q")
            {
                Console.WriteLine("You do not have rights outside of Q:");
                return;
            }
            FileInfo fi = new FileInfo(path);
            fi.Delete();
        }
    }
}
