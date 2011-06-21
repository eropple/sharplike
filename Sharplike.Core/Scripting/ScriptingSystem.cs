///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 2010 Ed Ropple.          ///
///                                                                         ///
/// This code is part of the Sharplike Roguelike library, and is licensed   ///
/// under the Common Public Attribution License (CPAL), version 1.0. Use of ///
/// this code is purusant to this license. The CPAL grants you certain      ///
/// permissions and requirements and should be read carefully before using  ///
/// this library.                                                           ///
///                                                                         ///
/// A copy of this license can be found in the Sharplike root directory,    ///
/// and must be included with all projects released using this library.     ///
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mono.Addins;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Sharplike.Core.Scripting
{
    public sealed class ScriptingSystem
    {
        internal ScriptingSystem()
        {
            //AddinManager.AddExtensionNodeHandler("/Sharplike/Scripting", ScriptsChanged);
            foreach (TypeExtensionNode node in AddinManager.GetExtensionNodes("/Sharplike/Scripting"))
            {
                IScriptingEngine eng = (IScriptingEngine)node.CreateInstance();
                engines.Add(node.Id, eng.Engine);
            }
        }

        void ScriptsChanged(object s, ExtensionNodeEventArgs args)
        {
            IScriptingEngine eng = (IScriptingEngine)args.ExtensionObject;

            switch (args.Change)
            {
                case ExtensionChange.Add:
                    engines.Add(args.ExtensionNode.Id, eng.Engine);
                    break;
                case ExtensionChange.Remove:
                    engines.Remove(args.ExtensionNode.Id);
                    break;
            }
        }

        public void Run(String file)
        {
            String ext = Path.GetExtension(file);
            engines[ext].ExecuteFile(file);
        }

        private Dictionary<String, ScriptEngine> engines = new Dictionary<string, ScriptEngine>();
    }
}
