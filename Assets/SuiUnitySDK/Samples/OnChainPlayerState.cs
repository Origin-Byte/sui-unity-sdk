public struct OnChainPlayerState
{
    public OnChainVector2 Position;
    public OnChainVector2 Velocity;

    public OnChainPlayerState(OnChainVector2 position, OnChainVector2 velocity)
    {
        Position = position;
        Velocity = velocity;
    }
}
