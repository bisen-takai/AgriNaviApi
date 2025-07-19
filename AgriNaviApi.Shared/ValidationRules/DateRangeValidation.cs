using AgriNaviApi.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AgriNaviApi.Shared.ValidationRules
{
    /// <summary>
    /// 開始日が終了日以前であることをチェック
    /// </summary>
    /// <remarks>
    /// クラス内で複数回適用可能
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DateRangeValidation : ValidationAttribute
    {
        private readonly string _startDatePropertyName;
        private readonly string _endDatePropertyName;

        public DateRangeValidation(string startDatePropertyName, string endDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
            _endDatePropertyName = endDatePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // プロパティ情報を取得
            PropertyInfo? startProp = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            PropertyInfo? endProp = validationContext.ObjectType.GetProperty(_endDatePropertyName);

            if (startProp == null || endProp == null)
            {
                throw new InvalidOperationException(
                            $"{validationContext.ObjectType.Name} に「{_startDatePropertyName}」または「{_endDatePropertyName}」が見つかりません。"
                        );
            }

            var displayAttribute = startProp.GetCustomAttribute<DisplayAttribute>();
            string startDisplayName = displayAttribute?.Name ?? _startDatePropertyName;
            var endDisplayAttribute = endProp.GetCustomAttribute<DisplayAttribute>();
            string endDisplayName = endDisplayAttribute?.Name ?? _endDatePropertyName;

            // 値を取得(Nullチェック含む)
            DateTime? startDateNullable = (DateTime?)startProp.GetValue(validationContext.ObjectInstance);
            DateTime? endDateNullable = (DateTime?)endProp.GetValue(validationContext.ObjectInstance);

            if (startDateNullable == null)
            {
                return new ValidationResult(string.Format(CommonValidationMessages.RequiredMessage, startDisplayName));
            }

            if (endDateNullable == null)
            {
                return new ValidationResult(string.Format(CommonValidationMessages.RequiredMessage, endDisplayName));
            }

            DateTime startDate = startDateNullable.Value;
            DateTime endDate = endDateNullable.Value;

            // Kindプロパティの一致をチェック
            if (startDate.Kind != endDate.Kind)
            {
                return new ValidationResult(CommonValidationMessages.DateKindMessage);
            }

            // 日付（または時刻）での整合性チェック
            if (startDate > endDate)
            {
                return new ValidationResult(CommonValidationMessages.DateRangeMessage);
            }

            return ValidationResult.Success;
        }
    }
}
