using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
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
        
        if (inputFieldName.text.Equals(""))
        {
            NetWorkManager.Instance.ShowWarningPopup("이름을 입력해 주세요.");
            return;
        }

        if (UserDataManager.Instance.CreateMyNote(inputFieldName.text) == false)
        {
            NetWorkManager.Instance.ShowWarningPopup("같은 이름의 노트는 만들 수 없습니다.");
            return;
        }

        StartCoroutine(CreateNewNote());
    }

    private IEnumerator CreateNewNote()
    {
        StartCoroutine(NetWorkManager.Instance.CreateNewNote(inputFieldName.text));
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });
        UserDataManager.Instance.AddUserNote(inputFieldName.text);
        DrawerLeft.main.InitNoteList();
        yield return new WaitWhile(() =>
        {
            return false == DrawerLeft.isInitNoteListDone;
        });
        GameEventMessage.SendEvent("CreateNewNoteDone");
        UIPopupManager.ClearQueue();
    }
    public void OnClickClose()
    {
        UIPopupManager.ClearQueue();
    }
}
