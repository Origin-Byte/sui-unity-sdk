namespace Suinet.Rpc.Types.MoveTypes
{
    [MoveType("0x2::dynamic_field::Field")]
    public class DynamicField
    {
        public UID Id { get; set; }
        public MoveObject Name { get; set; }
        public MoveObject Value { get; set; }
    }
}
