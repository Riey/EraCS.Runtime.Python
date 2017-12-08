using EraCS.UI.EraConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EraCS.Runtime.Python.Test
{
    public class TestVariableData
    {
        public int Time { get; set; }
    }

    public class TestProgram : EraProgram<EraConsole, TestVariableData>
    {
        public TestProgram() : base(new EraConsole(), new TestVariableData())
        {
        }

        protected override void RunScript()
        {
            try
            {
                var runtime = new PythonRuntime(
                    new[] { "test.py" },
                    new[] { Assembly.GetExecutingAssembly(), typeof(EraConsole).Assembly, typeof(EraProgram<EraConsole, TestVariableData>).Assembly },
                    new Dictionary<string, object>() { { "Debug", true } });

                runtime.Init(this);
                runtime.Start();
            }
            catch (Exception e)
            {
            }
        }
    }

    public class TestClass
    {
        [Fact]
        public void Test()
        {
            var program = new TestProgram();

            program.Start();

            while(program.Status == ProgramStatus.Running)
            {
                Task.Delay(100).Wait();
            }

            Assert.Equal(1000, program.VarData.Time);
        }
    }
}
