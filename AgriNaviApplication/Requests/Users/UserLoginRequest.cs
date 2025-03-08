using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Users
{
    /// <summary>
    /// ユーザログインリクエスト
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// ログインID
        /// </summary>
        [Required(ErrorMessage = "ログインIDは必須です。")]
        [StringLength(60)]
        public string LoginId { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        [Required(ErrorMessage = "パスワードは必須です。")]
        [MinLength(8, ErrorMessage = "パスワードは8文字以上必要です。")]
        [MaxLength(64, ErrorMessage = "パスワードは最大64文字までです。")]
        public string Password { get; set; } = string.Empty;
    }
}
