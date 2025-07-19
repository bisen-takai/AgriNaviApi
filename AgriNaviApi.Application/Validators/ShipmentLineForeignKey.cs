using AgriNaviApi.Application.Interfaces;

namespace AgriNaviApi.Application.Validators
{
    /// <summary>
    /// 出荷記録詳細の外部キー（出荷先・品質規格・単位）をまとめたレコード
    /// </summary>
    public sealed record ShipmentLineForeignKey(
        int ShipDestinationId,
        int QualityStandardId,
        int UnitId
    ) : IShipmentLineForeignKeys;
}
