/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 6/26/2012
 * Time: 3:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Reflection;
using MCForge.Interface;

namespace Updater
{
    /// <summary>
    /// Description of Download_Action.
    /// </summary>
    public class Download_Action : IAction
    {
        string _download;
        public Download_Action(string url) : base()
        {
            _download = url;
        }
        public override string download {
            get {
                return _download;
            }
        }
        public override string action {
            get {
                return "Downloading Patch";
            }
        }
        public override string saveas {
            get {
                return "temp.dll";
            }
        }
        public override void Action()
        {
            object instance = null;
            Assembly a = LoadAllDlls.LoadFile("temp.dll");
            foreach (Type ClassType in a.GetTypes()) {
                if (ClassType.IsPublic) {
                    if (ClassType.BaseType == typeof(IAction)) {
                        instance = Activator.CreateInstance(ClassType);
                        break;
                    }
                }
            }
            if (instance == null)
                return;
            IAction.actions.Add((IAction)instance);
            File.Delete("temp.dll");
        }
    }
}
