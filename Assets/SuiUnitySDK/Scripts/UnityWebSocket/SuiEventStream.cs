using UnityEngine;

public class SuiEventStream : MonoBehaviour
{
    private WebSocketService _webSocketService;

    public bool autoConnectOnStartup = true;

    private const string WebsocketEndpoint = "wss://pubsub.devnet.sui.io:443";

    public virtual void Awake()
    {
        _webSocketService = new WebSocketService();

        if (autoConnectOnStartup)
        {
            _webSocketService.StartConnection(WebsocketEndpoint);
            WebSocketActions.WebSocketEventAction += WebSocketEventAction;
            WebSocketActions.CloseWebSocketConnectionAction += CloseWebSocketConnectionAction;
        }
    }

    public virtual void Start()
    {
        if (autoConnectOnStartup)
        {
            _webSocketService.SubscribeToEvents("{\"MoveEventType\":\"0x2::devnet_nft::MintNFTEvent\"}");
        }
    }

    private void CloseWebSocketConnectionAction()
    {
        Debug.Log("CloseWebSocketConnectionAction");

    }

    private void WebSocketEventAction(string obj)
    {
        Debug.Log(obj);
    }

    public void OnDestroy()
    {
        _webSocketService.CloseConnection();
    }
}