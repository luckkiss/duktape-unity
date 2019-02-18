using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Duktape
{
    using UnityEngine;
    using UnityEditor;

    public partial class CodeGenerator
    {
        public BindingManager bindingManager;
        public TextGenerator csharp;
        public TextGenerator typescript;

        public CodeGenerator(BindingManager bindingManager)
        {
            this.bindingManager = bindingManager;
            var tab = Prefs.GetPrefs().tab;
            var newline = Prefs.GetPrefs().newline;
            csharp = new TextGenerator(newline, tab);
            typescript = new TextGenerator(newline, tab);
        }

        public void Clear()
        {
            csharp.Clear();
            typescript.Clear();
        }

        public void Generate(Type type)
        {
            Clear();
            using (new PlatformCodeGen(this))
            {
                using (new TopLevelCodeGen(this, type))
                {
                    using (new NamespaceCodeGen(this, Prefs.GetPrefs().ns, type))
                    {
                        if (type.IsEnum)
                        {
                            using (new EnumCodeGen(this, type))
                            {
                            }
                        }
                        else
                        {
                            using (var ccg = new ClassCodeGen(this, type))
                            {
                            }
                        }
                    }
                }
            }
        }

        public void WriteTo(string outDir, string filename, string tx)
        {
            try
            {
                var csName = filename + ".cs" + tx;
                var csPath = Path.Combine(outDir, csName);
                this.bindingManager.AddOutputFile(csPath);
                File.WriteAllText(csPath, this.csharp.ToString());
            }
            catch (Exception exception)
            {
                this.bindingManager.Error("write csharp file failed [{0}]: {1}", filename, exception.Message);
            }

            try
            {
                var tsName = filename + ".d.ts" + tx;
                var tsPath = Path.Combine(outDir, tsName);
                this.bindingManager.AddOutputFile(tsPath);
                File.WriteAllText(tsPath, this.typescript.ToString());
            }
            catch (Exception exception)
            {
                this.bindingManager.Error("write typescript file failed [{0}]: {1}", filename, exception.Message);
            }
        }
    }
}