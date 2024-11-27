﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AuthifyPass.Entities.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RegisterClientValidatorErrors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RegisterClientValidatorErrors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AuthifyPass.Entities.Resources.RegisterClientValidatorErrors", typeof(RegisterClientValidatorErrors).Assembly);
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
        ///   Looks up a localized string similar to Email can&apos;t be empty.
        /// </summary>
        internal static string EmailEmpty {
            get {
                return ResourceManager.GetString("EmailEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Is not a valid email.
        /// </summary>
        internal static string EmailValidation {
            get {
                return ResourceManager.GetString("EmailValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must have less than 150characters.
        /// </summary>
        internal static string MaxLenght150 {
            get {
                return ResourceManager.GetString("MaxLenght150", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must have less than 256 characters.
        /// </summary>
        internal static string MaxLenght256 {
            get {
                return ResourceManager.GetString("MaxLenght256", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must have some number.
        /// </summary>
        internal static string PasswordDigit {
            get {
                return ResourceManager.GetString("PasswordDigit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must have 1 lowercase.
        /// </summary>
        internal static string PasswordLowercase {
            get {
                return ResourceManager.GetString("PasswordLowercase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minimum 13 characters.
        /// </summary>
        internal static string PasswordMinLenght {
            get {
                return ResourceManager.GetString("PasswordMinLenght", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must have a special character.
        /// </summary>
        internal static string PasswordSpecial {
            get {
                return ResourceManager.GetString("PasswordSpecial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must have 1 uppercase.
        /// </summary>
        internal static string PasswordUppercase {
            get {
                return ResourceManager.GetString("PasswordUppercase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required field.
        /// </summary>
        internal static string Required {
            get {
                return ResourceManager.GetString("Required", resourceCulture);
            }
        }
    }
}
