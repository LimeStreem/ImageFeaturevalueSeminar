using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeatureValueExperiment;
using Microsoft.CSharp;
using ScriptBase;

namespace ScriptBase
{
    public class TransformStep:StepBase
    {

        private readonly FeatureValueContext _context;
        private readonly IExperimentStep _fore;
        CompilerResults compilerResults = null;

        private ScriptingControl scriptForm;

        public TransformStep(FeatureValueContext context,IExperimentStep fore)
        {
            _context = context;
            _fore = fore;
            SetPreview(fore);
            scriptForm=new ScriptingControl();
            scriptForm.Show();
            scriptForm.updateButton.Click+= UpdateButtonOnClick;
        }

        private void UpdateButtonOnClick(object sender, EventArgs eventArgs)
        {
            UpdateStep();
        }

        public override int StepNumber { get; set; }
        public override IExperimentStep NextStep { get; set; }
        public override IExperimentStep ForeStep { get; set; }

        public override void UpdateStep()
        {
            if (ForeStep != null)
            {
                if (Compile(scriptForm.scriptingBody.Text, new string[] { "SlimDX.dll", "FeatureValueExperimentBase.dll","System.dll" }))
                {
                    Type[] types = compilerResults.CompiledAssembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.IsSubclassOf(typeof (TransformerScriptBase)))
                        {
                            object instance=compilerResults.CompiledAssembly.CreateInstance(type.FullName);
                            TransformerScriptBase transformed = instance as TransformerScriptBase;
                            if(transformed==null)break;
                            CurrentImageContent = transformed.Transform(ForeStep.CurrentImageContent);
                            break;
                        }
                    }
                    if(NextStep!=null)NextStep.UpdateStep();
                }
            }
        }


   public bool Compile(string script, string[] assemblyNames)
   {
       // コンパイル時のオプション設定
       CompilerParameters param = new CompilerParameters(assemblyNames);  
       param.GenerateInMemory = true;          // exe を作らない
       param.IncludeDebugInformation = false;  // デバッグ情報を付加しない

       // コンパイルする
       CSharpCodeProvider codeProvider= new CSharpCodeProvider();
       compilerResults = codeProvider.CompileAssemblyFromSource(param, script);

       // エラーメッセージが無ければ成功
       return compilerResults.Errors.Count == 0;
   }



        public override IImageContent CurrentImageContent { get; set; }
    }
}
