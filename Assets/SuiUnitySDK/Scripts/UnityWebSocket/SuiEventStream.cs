using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiEventStream : MonoBehaviour
{
    [HideInInspector]
    public WebSocketService webSocketService;

    public bool autoConnectOnStartup = true;

    //private const string WebsocketEndpoint = "wss://pubsub.devnet.sui.io:443";
    private const string WebsocketEndpoint = "ws://pubsub.devnet.sui.io:80";
    
    public virtual void Awake()
    {
        webSocketService = new WebSocketService();

        if (autoConnectOnStartup)
        {
            webSocketService.StartConnection(WebsocketEndpoint);
            WebSocketActions.WebSocketEventAction += WebSocketEventAction;
            WebSocketActions.CloseWebSocketConnectionAction += CloseWebSocketConnectionAction;
        }
    }

    public virtual void Start()
    {
        if (autoConnectOnStartup)
        {
            webSocketService.SubscribeToEvents("{\"MoveEventType\":\"0x2::devnet_nft::MintNFTEvent\"}");
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
        webSocketService.CloseConnection();
    }
}
