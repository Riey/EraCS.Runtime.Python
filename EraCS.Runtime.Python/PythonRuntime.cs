using Microsoft.Scripting.Hosting;
using System.Collections.Generic;
using System.Reflection;

namespace EraCS.Runtime.Python
{
    public class PythonRuntime
    {
        private readonly ScriptEngine _engine;
        
        public ScriptScope Scope { get; }

        public PythonRuntime(IEnumerable<string> sources, IEnumerable<Assembly> assemblys, IDictionary<string, object> options)
        {
            _engine = IronPython.Hosting.Python.CreateEngine(options);

            foreach(Assembly asm in assemblys)
            {
                _engine.Runtime.LoadAssembly(asm);
            }

            Scope = _engine.CreateScope();

            foreach (string source in sources)
            {
                var script = _engine.CreateScriptSourceFromFile(source);
                var code = script.Compile();

                code.Execute(Scope);
            }
        }

        public void Init<TConsole, TVariable>(EraProgram<TConsole, TVariable> program)
            where TConsole : IEraConsole
        {
            Scope.GetVariable("ERA_INIT")(program);
        }

        public void Start()
        {
            Scope.GetVariable("ERA_START")();
        }
    }
}
