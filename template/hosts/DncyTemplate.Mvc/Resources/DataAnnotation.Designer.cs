﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DncyTemplate.Mvc {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class DataAnnotation {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DataAnnotation() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DncyTemplate.Mvc.Resources.DataAnnotation", typeof(DataAnnotation).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 Password 的本地化字符串。
        /// </summary>
        public static string LoginPassword {
            get {
                return ResourceManager.GetString("LoginPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Account 的本地化字符串。
        /// </summary>
        public static string LoginUserName {
            get {
                return ResourceManager.GetString("LoginUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} is not an valid email address 的本地化字符串。
        /// </summary>
        public static string MustEmailAddress {
            get {
                return ResourceManager.GetString("MustEmailAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} value bust between {1} and {2}. 的本地化字符串。
        /// </summary>
        public static string PageSizeMessage {
            get {
                return ResourceManager.GetString("PageSizeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 password must 8 char 的本地化字符串。
        /// </summary>
        public static string PwdMust8length {
            get {
                return ResourceManager.GetString("PwdMust8length", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 user not exist 的本地化字符串。
        /// </summary>
        public static string UserNotExist {
            get {
                return ResourceManager.GetString("UserNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} is required 的本地化字符串。
        /// </summary>
        public static string ValueIsRequired {
            get {
                return ResourceManager.GetString("ValueIsRequired", resourceCulture);
            }
        }
    }
}
