using UnityEngine;
using UnityWebSocket;
using System;
using Newtonsoft.Json;

public class WebSocketService
{
    private IWebSocket _socket;

    public IWebSocket Socket => _socket;

    /// <summary>
    /// Starts a websocket connection to the forwarded address
    /// </summary>
    /// <param name="address">The address to which we will start the connection</param>
    public void StartConnection(string address)
    {
        _socket = new WebSocket(address);
        _socket.OnOpen += OnOpen;
        _socket.OnMessage += OnMessage;
        _socket.OnClose += OnClose;
        _socket.OnError += OnError;
        _socket.ConnectAsync();
    }

    /// <summary>
    /// Close opened connection
    /// </summary>
    public void CloseConnection()
    {
        _socket?.CloseAsync();
    }
    
    public void SubscribeToEvents(string eventFilterString)
    {
        if (_socket is null) return;

        SendParameter(ReturnSubscribeParameter(eventFilterString));
    }
    
    public void UnSubscribeToEvents(ulong streamId)
    {
        if (_socket is null) return;

        SendParameter(ReturnUnsubscribeParameter(streamId));
    }

    /// <summary>
    /// Returns error if it happens
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Received message arguments</param>
    private void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("OnError: " + JsonConvert.SerializeObject(e));
    }

    /// <summary>
    /// Returns message that connection is opened
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Received message arguments</param>
    private void OnOpen(object sender, OpenEventArgs e)
    {
    }

    /// <summary>
    /// Returns message that connection is closed
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Received message arguments</param>
    private void OnClose(object sender, CloseEventArgs e)
    {
        _socket = null;
        MainThreadDispatcher.Instance().Enqueue(() => { WebSocketActions.CloseWebSocketConnectionAction?.Invoke(); });
    }

    /// <summary>
    /// Function that is called when a message is received from a websocket.
    /// In our case it was done only for account subscription and unsubscription.function needs to be expanded
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Received message arguments</param>
    private void OnMessage(object sender, MessageEventArgs e)
    {
        try
        {
            //var data = JsonConvert.DeserializeObject<SubscriptionModel>(e.Data);
            MainThreadDispatcher.Instance().Enqueue(() => { WebSocketActions.WebSocketEventAction.Invoke(e.Data); });
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    /// <summary>
    /// Function by which we async send a parameter to the websocket
    /// </summary>
    /// <param name="parameter">Parameter to send to websocket</param>
    private void SendParameter(string parameter)
    {
        if (_socket == null) return;

        _socket.SendAsync(parameter);
    }

    /// <summary>
    /// Returns JSONRPC message for event subscription
    /// </summary>
    /// <returns></returns>
    private string ReturnSubscribeParameter(string eventFilterString)
    {
        if (_socket is null) return null;

        var parameterToSend = "{\"jsonrpc\":\"2.0\", \"id\": 1, \"method\": \"suix_subscribeEvent\", \"params\": [" + eventFilterString + "]}";
        return parameterToSend;
    }

    /// <summary>
    /// Returns JSONRPC message for event unsubscription
    /// </summary>
    private string ReturnUnsubscribeParameter(ulong streamId)
    {
        if (_socket is null) return null;

        var unsubscribeParameter = "{\"jsonrpc\":\"2.0\", \"id\":1, \"method\":\"suix_unsubscribeEvent\", \"params\":[" + streamId + "]}";
        return unsubscribeParameter;
    }
}

