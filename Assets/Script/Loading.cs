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
    MY_VOCAB_LIST = 0,
    MY_NOTE_LIST = 1,
    PREPARE_DATA = 2,
}

public class Loading : MonoBehaviour
{
    public static GameObject main = null;
    void Start()
    {
        main = gameObject;
    }
    
}
