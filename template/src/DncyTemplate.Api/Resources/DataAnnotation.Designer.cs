﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DncyTemplate.Api {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DncyTemplate.Api.Resources.DataAnnotation", typeof(DataAnnotation).Assembly);
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
        ///   查找类似 address 的本地化字符串。
        /// </summary>
        public static string DemoModel_Address {
            get {
                return ResourceManager.GetString("DemoModel.Address", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 name 的本地化字符串。
        /// </summary>
        public static string DemoModel_Name {
            get {
                return ResourceManager.GetString("DemoModel.Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} value bust between {1} and {2}. 的本地化字符串。
        /// </summary>
        public static string PageIndexIsInvalid {
            get {
                return ResourceManager.GetString("PageIndexIsInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The field {0} must be a string type with a maximum length of &apos;{1}&apos; and mininum of &apos;{2}&apos;. 的本地化字符串。
        /// </summary>
        public static string StringLengthInvalid {
            get {
                return ResourceManager.GetString("StringLengthInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The field {0} must be a string with a minimum length of {2} and a maximum length of {1}. 的本地化字符串。
        /// </summary>
        public static string StringLengthIsInvalid {
            get {
                return ResourceManager.GetString("StringLengthIsInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} is required. 的本地化字符串。
        /// </summary>
        public static string ValueIsRequired {
            get {
                return ResourceManager.GetString("ValueIsRequired", resourceCulture);
            }
        }
    }
}
