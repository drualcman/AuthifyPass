namespace AuthifyPass.API.Resources
{
    using System;
    using System.Resources;
    using System.Globalization;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class RegisterPageContent {
        
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal RegisterPageContent() {
        }

        internal static ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager(".RegisterPageContent", typeof(RegisterPageContent).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string CancelButtonText {
            get {
                return ResourceManager.GetString("CancelButtonText", resourceCulture);
            }
        }

        public static string ConfirmLabelText {
            get {
                return ResourceManager.GetString("ConfirmLabelText", resourceCulture);
            }
        }

        public static string ConfirmPlaceholderText {
            get {
                return ResourceManager.GetString("ConfirmPlaceholderText", resourceCulture);
            }
        }

        public static string EmailLabelText {
            get {
                return ResourceManager.GetString("EmailLabelText", resourceCulture);
            }
        }

        public static string EmailPlaceholderText {
            get {
                return ResourceManager.GetString("EmailPlaceholderText", resourceCulture);
            }
        }

        public static string HelpBodyHtml {
            get {
                return ResourceManager.GetString("HelpBodyHtml", resourceCulture);
            }
        }

        public static string HelpHeaderText {
            get {
                return ResourceManager.GetString("HelpHeaderText", resourceCulture);
            }
        }

        public static string NameLabelText {
            get {
                return ResourceManager.GetString("NameLabelText", resourceCulture);
            }
        }

        public static string NamePlaceholderText {
            get {
                return ResourceManager.GetString("NamePlaceholderText", resourceCulture);
            }
        }

        public static string NotificationBodyHtmlTemplate {
            get {
                return ResourceManager.GetString("NotificationBodyHtmlTemplate", resourceCulture);
            }
        }

        public static string PasswordLabelText {
            get {
                return ResourceManager.GetString("PasswordLabelText", resourceCulture);
            }
        }

        public static string PasswordPlaceholderText {
            get {
                return ResourceManager.GetString("PasswordPlaceholderText", resourceCulture);
            }
        }

        public static string RegisterButtonText {
            get {
                return ResourceManager.GetString("RegisterButtonText", resourceCulture);
            }
        }

        public static string TitleText {
            get {
                return ResourceManager.GetString("TitleText", resourceCulture);
            }
        }

        public static string ValidationButtonText {
            get {
                return ResourceManager.GetString("ValidationButtonText", resourceCulture);
            }
        }

        public static string ValidationCodeError {
            get {
                return ResourceManager.GetString("ValidationCodeError", resourceCulture);
            }
        }

        public static string ValidationMessage {
            get {
                return ResourceManager.GetString("ValidationMessage", resourceCulture);
            }
        }

        public static string ValidationText {
            get {
                return ResourceManager.GetString("ValidationText", resourceCulture);
            }
        }

    }
}