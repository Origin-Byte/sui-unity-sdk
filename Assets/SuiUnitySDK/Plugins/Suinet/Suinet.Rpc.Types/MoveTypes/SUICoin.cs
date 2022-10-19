namespace Suinet.Rpc.Types.MoveTypes
{
    [MoveType("coin::Coin<0x2::sui::SUI>")]
    public class SUICoin 
    {
        public UID Id { get; set; }

        public ulong Balance { get; set; }
    }
}
