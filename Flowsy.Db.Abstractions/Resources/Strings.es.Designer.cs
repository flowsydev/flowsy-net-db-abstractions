﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Flowsy.Db.Abstractions.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings_es {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings_es() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Flowsy.Db.Abstractions.Resources.Strings_es", typeof(Strings_es).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string ProviderXDoesNotHaveADefaultDatabaseName {
            get {
                return ResourceManager.GetString("ProviderXDoesNotHaveADefaultDatabaseName", resourceCulture);
            }
        }
        
        internal static string FailedToCreateConnectionForProviderX {
            get {
                return ResourceManager.GetString("FailedToCreateConnectionForProviderX", resourceCulture);
            }
        }
        
        internal static string ProviderXIsNotSupported {
            get {
                return ResourceManager.GetString("ProviderXIsNotSupported", resourceCulture);
            }
        }
        
        internal static string ProviderXCanNotReturnATableFromRoutineOfTypeY {
            get {
                return ResourceManager.GetString("ProviderXCanNotReturnATableFromRoutineOfTypeY", resourceCulture);
            }
        }
        
        internal static string ProviderXDoesNotSupportRoutineTypeY {
            get {
                return ResourceManager.GetString("ProviderXDoesNotSupportRoutineTypeY", resourceCulture);
            }
        }
        
        internal static string ProviderXDoesNotSupportConnectionStringBuilding {
            get {
                return ResourceManager.GetString("ProviderXDoesNotSupportConnectionStringBuilding", resourceCulture);
            }
        }
        
        internal static string ConnectionStringDoesNotContainAServerNameOrIpAddress {
            get {
                return ResourceManager.GetString("ConnectionStringDoesNotContainAServerNameOrIpAddress", resourceCulture);
            }
        }
        
        internal static string CannotParseValueForSqlTypeX {
            get {
                return ResourceManager.GetString("CannotParseValueForSqlTypeX", resourceCulture);
            }
        }
        
        internal static string ValueCannotBeNullOrEmptyForNonNullableColumns {
            get {
                return ResourceManager.GetString("ValueCannotBeNullOrEmptyForNonNullableColumns", resourceCulture);
            }
        }
        
        internal static string HexStringMustHaveAnEvenNumberOfCharacters {
            get {
                return ResourceManager.GetString("HexStringMustHaveAnEvenNumberOfCharacters", resourceCulture);
            }
        }
        
        internal static string ArrayColumnMustHaveUserDefinedType {
            get {
                return ResourceManager.GetString("ArrayColumnMustHaveUserDefinedType", resourceCulture);
            }
        }
        
        internal static string NameXIsNotAValidColumnName {
            get {
                return ResourceManager.GetString("NameXIsNotAValidColumnName", resourceCulture);
            }
        }
    }
}
