using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Doozy.Engine;
using Doozy.Engine.UI;
using UnityEngine;

public class DeleteNotePopup : MonoBehaviour
{
    [HideInInspector] public string noteName = string.Empty;
    public void OnClickConfirmDeleteButton()
    {
        StartCoroutine(DeleteMyNote());
    }

    private IEnumerator DeleteMyNote()
    {
        StartCoroutine(NetWorkManager.Instance.DeleteMyNote(noteName));
        yield return new WaitWhile(() => { return false == NetWorkManager.Instance.isJsonDone; });
        StartCoroutine(NetWorkManager.Instance.DeleteAllMyVocabs(noteName));
        yield return new WaitWhile(() => { return false == NetWorkManager.Instance.isJsonDone; });
        UserDataManager.Instance.DeleteMyNote(noteName);
        DrawerLeft.main.InitNoteList();
        yield return new WaitWhile(() => { return false == DrawerLeft.isInitNoteListDone; });
        GameEventMessage.SendEvent("DeleteNoteDone");
        UIPopupManager.ClearQueue();
    }

    public void OnClickCancelButton()
    {
        UIPopupManager.ClearQueue();
    }
}
