using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentRecordDetails
{
    /// <summary>
    /// 出荷記録詳細削除リクエスト
    /// </summary>
    public class ShipmentRecordDetailDeleteRequest
    {
        /// <summary>
        /// 出荷記録詳細ID
        /// </summary>
        [Display(Name = "出荷記録詳細ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
