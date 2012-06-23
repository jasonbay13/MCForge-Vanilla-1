﻿/********************************************************
 * ADO.NET 2.0 Data Provider for SQLite Version 3.X
 * Written by Robert Simpson (robert@blackcastlesoft.com)
 * 
 * Released to the public domain, use at your own risk!
 ********************************************************/

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SQLite.Designer {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class VSPackage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal VSPackage() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SQLite.Designer.VSPackage", typeof(VSPackage).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RZE1PAAIMRZRE2CKM3CIRRIREIJQR1PTAEHTRHKTCPP1KKRRJQK9CTM9ZHMRETI9E9J8REKEA1MER9PDKQDIH8HMRRH2DIACHIP1KHK2IAZKM8R0EZRTKDHADII9ICCH.
        /// </summary>
        internal static string _400 {
            get {
                return ResourceManager.GetString("400", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The database and its metadata will be un-encrypted.  No password will be required to open the database and view its contents..
        /// </summary>
        internal static string Decrypt {
            get {
                return ResourceManager.GetString("Decrypt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The database and its metadata will be encrypted using the supplied password as a hash..
        /// </summary>
        internal static string Encrypt {
            get {
                return ResourceManager.GetString("Encrypt", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap info {
            get {
                object obj = ResourceManager.GetObject("info", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The database and its metadata will be re-encrypted using the supplied password as a hash..
        /// </summary>
        internal static string ReEncrypt {
            get {
                return ResourceManager.GetString("ReEncrypt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [SQLite]
        ///System.Data.SQLite.SQLiteConnection, System.Data.SQLite
        ///System.Data.SQLite.SQLiteDataAdapter, System.Data.SQLite
        ///System.Data.SQLite.SQLiteCommand, System.Data.SQLite
        ///.
        /// </summary>
        internal static string ToolboxItems {
            get {
                return ResourceManager.GetString("ToolboxItems", resourceCulture);
            }
        }
    }
}
