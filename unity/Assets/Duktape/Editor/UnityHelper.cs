using System;
using System.Collections.Generic;

namespace Duktape
{
    using UnityEngine;
    using UnityEditor;

    public class UnityHelper
    {
        #region All Menu Items
        [MenuItem("Duktape/Generate Bindings")]
        public static void GenerateBindings()
        {
            var bm = new BindingManager();
            bm.Collect();
            // temp
            // bm.AddExport(typeof(GameObject));
            // bm.AddExport(typeof(Transform));
            bm.Generate();
            bm.Cleanup();
            AssetDatabase.Refresh();
        }
        
        [MenuItem("Duktape/Prefs ...")]
        public static void OpenPrefsEditor()
        {
            EditorWindow.GetWindow<PrefsEditor>().Show();
        }
        #endregion
    }
}