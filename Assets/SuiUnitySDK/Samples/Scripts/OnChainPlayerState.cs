public struct OnChainPlayerState
{
    public OnChainVector2 Position { get; }
    public OnChainVector2 Velocity { get; }
    public ulong SequenceNumber { get; }
    public bool IsExploded { get; }
    
    public OnChainPlayerState(OnChainVector2 position, OnChainVector2 velocity, ulong sequenceNumber, bool isExploded)
    {
        Position = position;
        Velocity = velocity;
        SequenceNumber = sequenceNumber;
        IsExploded = isExploded;
    }
}
