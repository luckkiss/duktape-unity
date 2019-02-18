using System;
using System.Collections.Generic;

namespace Duktape
{
    using UnityEngine;
    using UnityEditor;

    public enum NewLineStyle
    {
        AUTO,
        CR,
        LF,
        CRLF,
    }

    // duktape 配置 (editor only)
    public class Prefs
    {
        private static Prefs _prefs;
        public const string PATH = "ProjectSettings/duktape.json";

        public string logPath = "Temp/duktape.log";

        private bool _dirty;

        // 静态绑定代码的生成目录
        public string outDir = "Assets/Duktape/Generated";

        public NewLineStyle newLineStyle;

        public string newline
        {
            get
            {
                switch (Prefs.GetPrefs().newLineStyle)
                {
                    case NewLineStyle.CR: return "\r"; 
                    case NewLineStyle.LF: return "\n"; 
                    case NewLineStyle.CRLF: return "\r\n"; 
                    default: return Environment.NewLine; 
                }
            }
        }

        public string tab = "    ";

        // 生成的绑定类所在命名空间
        public string ns = "DuktapeJS";

        // 默认不导出任何类型, 需要指定导出类型列表
        public List<string> explicitAssemblies = new List<string>(new string[]
        {
            "Assembly-CSharp-firstpass",
            "Assembly-CSharp",
        });

        // 默认导出所有类型, 过滤黑名单
        public List<string> implicitAssemblies = new List<string>(new string[]
        {
            "UnityEngine",
            "UnityEngine.CoreModule",
            "UnityEngine.UIModule",
            "UnityEngine.TextRenderingModule",
            "UnityEngine.TextRenderingModule",
            "UnityEngine.UnityWebRequestWWWModule",
            "UnityEngine.Physics2DModule",
            "UnityEngine.AnimationModule",
            "UnityEngine.TextRenderingModule",
            "UnityEngine.IMGUIModule",
            "UnityEngine.UnityWebRequestModule",
            "UnityEngine.PhysicsModule",
            "UnityEngine.UI",
        });

        // type.FullName 前缀满足以下任意一条时不会被导出
        public List<string> typePrefixBlacklist = new List<string>(new string[]
        {
            "JetBrains.", 
            "Unity.Collections.",
            "Unity.Jobs.",
            "Unity.Profiling.",
            "UnityEditor.",
            "UnityEditorInternal.",
            "UnityEngineInternal.",
            "UnityEditor.Experimental.",
            "Unity.IO.LowLevel.",
            "Unity.Burst.",
        });

        public static Prefs GetPrefs()
        {
            if (_prefs == null)
            {
                try
                {
                    if (System.IO.File.Exists(PATH))
                    {
                        var json = System.IO.File.ReadAllText(PATH);
                        _prefs = JsonUtility.FromJson<Prefs>(json);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogWarning(exception);
                }
                if (_prefs == null)
                {
                    _prefs = new Prefs();
                    _prefs.MarkAsDirty();
                }
            }
            return _prefs;
        }

        public void MarkAsDirty()
        {
            if (!_dirty)
            {
                _dirty = true;
                EditorApplication.delayCall += Save;
            }
        }

        public void Save()
        {
            if (_dirty)
            {
                _dirty = false;
                try
                {
                    var json = JsonUtility.ToJson(this, true);
                    System.IO.File.WriteAllText(PATH, json);
                }
                catch (Exception exception)
                {
                    Debug.LogWarning(exception);
                }
            }
        }
    }
}