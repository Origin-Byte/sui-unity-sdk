public struct OnChainPlayerState
{
    public OnChainVector2 Position;
    public OnChainVector2 Velocity;
    public ulong SequenceNumber;

    public OnChainPlayerState(OnChainVector2 position, OnChainVector2 velocity, ulong sequenceNumber)
    {
        Position = position;
        Velocity = velocity;
        SequenceNumber = sequenceNumber;
    }
}
