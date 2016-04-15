using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;

namespace Modified.CSharp
{
    public class UnityCSharpCodeProvider:CSharpCodeProvider
    {
        public UnityCSharpCodeProvider(Dictionary<string,string> providerOptions):base(providerOptions)
        {
            
        }
        public override ICodeCompiler CreateCompiler()
        {
            return new UnityCSharpCodeCompiler();//base.CreateCompiler();
        }
    }
}
