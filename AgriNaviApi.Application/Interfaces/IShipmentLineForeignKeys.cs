namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 出荷明細に紐づく外部キー ID を表す共通インターフェース
    /// </summary>
    public interface IShipmentLineForeignKeys
    {
        int ShipDestinationId { get; }
        int QualityStandardId { get; }
        int UnitId { get; }
    }
}
