using AgriNaviApi.Shared.Interfaces;
using System.Runtime.Serialization;

namespace AgriNaviApi.Shared.Exceptions
{
    /// <summary>
    /// データが見つからなかった場合の例外
    /// </summary>
    /// <remarks>
    /// データが見つからなかったときの例外を明示することが目的のため、特に処理は無し
    /// </remarks>
    [Serializable]
    public class EntityNotFoundException : Exception, ILocalizedException
    {
        /// <summary>
        /// どのresxファイルを使うかを登録
        /// </summary>
        public Type ResourceType { get; }

        /// <summary>
        /// リソースキー名
        /// </summary>
        public string ResourceKey { get; }

        /// <summary>
        /// リソース文字列のフォーマット プレースホルダーに対応する引数を保持
        /// </summary>
        public object[] Args { get; }

        public EntityNotFoundException(Type resourceType, string resourceKey, params object[] args) : base()
        {
            ResourceType = resourceType;
            ResourceKey = resourceKey;
            Args = args;
        }

        /// <summary>
        /// シリアライズ用のコンストラクタ
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [Obsolete("This constructor is only for serialization.", DiagnosticId = "SYSLIB0051")]
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // カスタムプロパティの復元
            ResourceType = (Type?)info.GetValue(nameof(ResourceType), typeof(Type)) ?? typeof(object);
            ResourceKey = info.GetString(nameof(ResourceKey)) ?? string.Empty;
            Args = (object[]?)info.GetValue(nameof(Args), typeof(object[])) ?? Array.Empty<object>();
        }

        /// <summary>
        /// シリアライズデータにカスタムプロパティを追加
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [Obsolete("This method is only for serialization.", DiagnosticId = "SYSLIB0051")]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context); // 基底クラスの情報もシリアライズ
            info.AddValue(nameof(ResourceType), ResourceType, typeof(Type));
            info.AddValue(nameof(ResourceKey), ResourceKey);
            info.AddValue(nameof(Args), Args, typeof(object[]));
        }
    }
}
