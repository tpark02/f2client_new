using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class NewNotePopupController : MonoBehaviour
{
    [SerializeField] public InputField inputFieldName;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickOK()
    {
        StartCoroutine(NetWorkManager.Instance.CreateNewNote(inputFieldName.text));
    }

    public void OnClickClose()
    {
        UIPopupManager.ClearQueue();
    }
}
