using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.IO;

public class ConsoleManager : MonoBehaviour
{
    public Text Output;
    public InputField Input;

    Process cmdProcess;
    Thread outputUpdater;
    string outputText="";

    // Use this for initialization
    void Start()
    {
        ProcessStartInfo cmdStartInfo = new ProcessStartInfo("cmd.exe","/K");
        cmdStartInfo.CreateNoWindow = true;
        cmdStartInfo.UseShellExecute = false;
        cmdStartInfo.RedirectStandardInput = true;
        cmdStartInfo.RedirectStandardOutput = true;
        cmdProcess = Process.Start(cmdStartInfo);
        cmdProcess.StandardInput.WriteLine();

        //Setup environment
        DirectoryInfo qDriveFolder = new DirectoryInfo(Application.dataPath + "/StreamingAssets/drives/q");
        if (!qDriveFolder.Exists)
        {
            qDriveFolder.Create();
        }
        cmdProcess.StandardInput.WriteLine("subst Q: " + qDriveFolder.FullName);
        cmdProcess.StandardInput.WriteLine("Q:");


        outputUpdater = new Thread(ReadConsole);
        outputUpdater.Start();
        Thread.Sleep(200);
        outputText = "";
        cmdProcess.StandardInput.WriteLine();
    }

    // Update is called once per frame
    void Update()
    {
        int lines = (int)(((RectTransform)Output.transform).rect.height / (Output.fontSize+5));
        string[] outputLines = outputText.Split('\n');
        if (outputLines.Length > lines)
        {
            outputText = "";

            for (int i = 1; i < outputLines.Length; i++)
            {
                outputText += outputLines[i]+"\n";
            }
            outputText = outputText.Substring(0, outputText.Length - 1);
        }
        Output.text = outputText;
    }
    void ReadConsole()
    {
        while (!cmdProcess.StandardOutput.EndOfStream)
        {
            char inputChar = (char)cmdProcess.StandardOutput.Read();
            outputText += inputChar;
        }
    }
    public void OnTextChanged(string text)
    {
        if (text.Contains("\n"))
        {
            cmdProcess.StandardInput.Write(text);
            Input.text = "";
        }
    }
    void OnApplicationQuit()
    {
        cmdProcess.StandardInput.WriteLine("subst Q: /d");
        cmdProcess.Kill();
    }
    public void Write(string text)
    {
        outputText += text;
    }
    public void WriteLine(string text)
    {
        Write(text + "\n");
    }
    public void NewPrompt()
    {
        cmdProcess.StandardInput.WriteLine();
    }
}
