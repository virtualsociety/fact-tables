using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using Xunit;

namespace Vs.FactTables.CodeGenerator.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var reader = new FactTableReader();
            var source = reader.Read();
            File.WriteAllText(@"../../../../Vs.FactTables.Entities/FactTables.cs", source);
            //FileInfo fi = new FileInfo(@"../../../Vs.FactTables.Entities/Db.cs");

            //CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            //ICodeCompiler icc = codeProvider.CreateCompiler();

            //CompilerParameters parameters = new CompilerParameters();
            //parameters.GenerateExecutable = true;
            //parameters.OutputAssembly = "./Vs.FactTables.dll";
            //CompilerResults results = icc.CompileAssemblyFromSource(parameters, source);
        }
    }
}
