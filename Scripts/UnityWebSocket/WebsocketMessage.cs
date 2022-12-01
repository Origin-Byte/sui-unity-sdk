using Suinet.Rpc.Types;

public class WebsocketMessage
{
    public string Method { get; set; }

    public WebsocketMessageParams Params { get; set; }
}

public class WebsocketMessageParams
{
    public ulong Subscription { get; set; }
    public SuiEventEnvelope Result { get; set; }
}
