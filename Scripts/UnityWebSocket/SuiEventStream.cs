using UnityEngine;

public class SuiEventStream : MonoBehaviour
{
    private WebSocketService _webSocketService;

    public bool autoConnectOnStartup = true;

    private const string WebsocketEndpoint = "wss://fullnode.testnet.sui.io:443";

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
            var filterJson = "{\"MoveModule\":{\"module\":\"clob\",\"package\":\"0x8da36ef392a7d2b1e7dac2a987767eea5a415d843d3d34cb66bec6434001f931\"}}";
            _webSocketService.SubscribeToEvents(filterJson);
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