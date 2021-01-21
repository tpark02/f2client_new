using System;
using System.Collections;
using System.Runtime.InteropServices;
using DG.Tweening;
using Doozy.Engine;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
    
    
enum PacketType
{
    GET_MY_VOCAB_LIST = 0,
    GET_MY_NOTE_LIST = 1,
    PREPARE_DATA = 2,
    Get_VOCAB_DETAIL = 3,
}

public class Loading : MonoBehaviour
{
    public static GameObject main = null;
    void Start()
    {
        main = gameObject;
    }
    
}
