using System;
using System.Collections.Generic;

namespace Duktape
{
    using UnityEngine;

    public interface IDuktapeListener
    {
        void OnTypesBinding(DuktapeVM vm);
        void OnBindingError(DuktapeVM vm, Type type);
        void OnBinded(DuktapeVM vm, int numRegs);
        void OnProgress(DuktapeVM vm, int step, int total);
        void OnLoaded(DuktapeVM vm);
    }

    // 编辑器运行时监听
    public interface IDuktapeEditorListener
    {
        // 源代码发生变更
        void OnSourceModified();
    }
}
