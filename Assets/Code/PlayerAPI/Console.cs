using UnityEngine;

namespace Standard
{
    public class Console
    {
        public static void Write(string text)
        {
            ConsoleManager console = GameObject.FindObjectOfType<ConsoleManager>();
            console.Write(text);
        }
        public static void WriteLine(string text)
        {
            Write(text + "\n");
        }
    }
}
