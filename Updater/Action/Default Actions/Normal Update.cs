/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 6/26/2012
 * Time: 3:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;


namespace Updater
{
    /// <summary>
    /// Description of Normal_Update.
    /// </summary>
    public class Normal_Update : IAction
    {
        public override string download {
            get {
                return Program.updateserver + "/DLL/Core.dll";
            }
        }
        public override string saveas {
            get {
                return "MCForge.dll";
            }
        }
        public override string action {
            get {
                return "Downloading Core Updates..";
            }
        }
        public override void Action()
        {
            //Do nothing
        }
    }
}
