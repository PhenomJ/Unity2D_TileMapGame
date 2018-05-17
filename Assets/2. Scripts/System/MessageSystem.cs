using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem
{   //Singleton
    static MessageSystem _instance;
    public static MessageSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MessageSystem();
                _instance.Init();
            }
            return _instance;
        }
    }

    void Init()
    {

    }

    Queue<MessageParam> _messageQueue = new Queue<MessageParam>();

    public void Send(MessageParam msgParam)
    {
        _messageQueue.Enqueue(msgParam);
    }

    public void ProcessMessage()
    {
        while (_messageQueue.Count != 0)
        {
            //ReceiveObjMessage
            MessageParam msgParam = _messageQueue.Dequeue();
            msgParam.receiver.ReceiverObjMessage(msgParam);
        }
    }
}