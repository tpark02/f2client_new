using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

public class NewNotePopupController : MonoBehaviour
{
    [SerializeField] public InputField inputFieldName;
 
    public void OnClickOK()
    {
        if (MyNoteList.myNoteList.GetComponent<MyNoteList>().content.transform.childCount >= 10)
        {
            NetWorkManager.Instance.ShowWarningPopup("10 개 이상 노트를 만들 수 없습니다.");
            return;
        }
        StartCoroutine(NetWorkManager.Instance.CreateNewNote(inputFieldName.text));
    }

    public void OnClickClose()
    {
        UIPopupManager.ClearQueue();
    }
}
