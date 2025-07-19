namespace AgriNaviApi.Shared.Interfaces
{
    public interface ILocalizedException
    {
        /// <summary>
        /// 使用するリソースファイルのタイプを指定
        /// </summary>
        /// <value>リソースファイルを表すType</value>
        Type ResourceType { get; }

        /// <summary>
        /// リソースファイル内のメッセージキーを指定
        /// </summary>
        /// <value>リソースキー文字列</value>
        string ResourceKey { get; }

        /// <summary>
        /// メッセージのフォーマット時に使用する引数の配列
        /// </summary>
        object[] Args { get; }
    }
}
