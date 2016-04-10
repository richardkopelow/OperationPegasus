using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections.Specialized;

public class CodeEditorManager : MonoBehaviour
{
    public GameObject FileDialogue;
    public InputField DialogueText;
    public InputField EditorText;
    public ConsoleManager Console;

    string openFilePath="";
    bool saveAsDialogue;

    StringCollection referencedAssemblies;

    // Use this for initialization
    void Start()
    {
        referencedAssemblies = new StringCollection();

        referencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

        AssemblyName[] assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        foreach (AssemblyName assem in assemblies)
        {
            referencedAssemblies.Add(assem.CodeBase);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSaveClicked()
    {
        using (StreamWriter sw=new StreamWriter(openFilePath))
        {
            sw.Write(EditorText.text);
        }
    }
    public void OnSaveAsClicked()
    {
        saveAsDialogue = true;
        showDialogue(true);
    }
    public void OnLoadClicked()
    {
        saveAsDialogue = false;
        showDialogue(false);
    }
    public void OnTestClicked()
    {
        Assembly result = Compile();
        if (result != null)
        {
            TestBed tester = (TestBed)result.CreateInstance("Test");
            Console.Write("\n");
            tester.Run();
        }
        Console.NewPrompt();
    }
    public void OnSubmitClicked()
    {
        Assembly result = Compile();

        if (result!=null)
        {
            MissionManager.Instance.CheckSolution(result);
        }
        Console.NewPrompt();
    }
    public void OnDialogueOKClicked()
    {
        openFilePath = DialogueText.text;
        if (saveAsDialogue)
        {
            using (StreamWriter sw = new StreamWriter(openFilePath))
            {
                sw.Write(EditorText.text);
            }
        }
        else
        {
            using (StreamReader sr=new StreamReader(openFilePath))
            {
                EditorText.text = sr.ReadToEnd();
            }
        }
        FileDialogue.SetActive(false);
    }
    public void OnDialogueCancelClicked()
    {
        FileDialogue.SetActive(false);
    }
    public void OnDialogueTextChanged(string text)
    {
        if (text.Contains("\n"))
        {
            DialogueText.text = DialogueText.text.Replace("\n","");
            OnDialogueOKClicked();
        }
    }
    void showDialogue(bool saveAs)
    {
        saveAsDialogue = saveAs;
        DialogueText.text = openFilePath;
        FileDialogue.SetActive(true);
        DialogueText.Select();
    }
    Assembly Compile()
    {
        CSharpCodeProvider provider = new CSharpCodeProvider();

        CompilerParameters compilerParams = new CompilerParameters();
        compilerParams.GenerateInMemory = true;
        compilerParams.GenerateExecutable = false;
        foreach (string assemblyPath in referencedAssemblies)
        {
            compilerParams.ReferencedAssemblies.Add(assemblyPath);
        }

        CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, EditorText.text);

        if (results.Errors.Count > 0)
        {
            Console.Write("\n");
            foreach (CompilerError error in results.Errors)
            {
                Console.WriteLine(string.Format("Line {0} - {1}",error.Line,error.ErrorText));
            }
        }

        return results.CompiledAssembly;
    }
}
