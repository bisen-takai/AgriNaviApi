﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AgriNaviApi.Common.Resources {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CommonValidationMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommonValidationMessages() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AgriNaviApi.Common.Resources.CommonValidationMessages", typeof(CommonValidationMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
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
        ///   開始日と終了日の Kind が一致していません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string DateKindMessage {
            get {
                return ResourceManager.GetString("DateKindMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   終了日は開始日以降でなければなりません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string DateRangeMessage {
            get {
                return ResourceManager.GetString("DateRangeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   有効なメールアドレスを入力してください。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string EmailMessage {
            get {
                return ResourceManager.GetString("EmailMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0}は{1}文字までです。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string MaxLengthMessage {
            get {
                return ResourceManager.GetString("MaxLengthMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   電話番号は10桁または11桁の数字でなければなりません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string PhoneMessage {
            get {
                return ResourceManager.GetString("PhoneMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   プロパティ名が正しくありません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string propertyNameMessage {
            get {
                return ResourceManager.GetString("propertyNameMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0}は{1}～{2}の範囲で指定してください。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string RangeMessage {
            get {
                return ResourceManager.GetString("RangeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0}は必須です。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string RequiredMessage {
            get {
                return ResourceManager.GetString("RequiredMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   無効な検索一致タイプが指定されました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string SearchMatchTypeMessage {
            get {
                return ResourceManager.GetString("SearchMatchTypeMessage", resourceCulture);
            }
        }
    }
}
