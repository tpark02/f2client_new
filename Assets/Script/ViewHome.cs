using System.Collections;
using Doozy.Engine;
using UnityEngine;

public class ViewHome : MonoBehaviour
{
    public static ViewHome main = null;
    [SerializeField] public VocabTouchNotice homeVocabTouchNotice;

    IEnumerator Start()
    {
        Application.targetFrameRate = 60;
        homeVocabTouchNotice.SetLabel("단어를 두번 터치하면단어장에 추가됩니다.");
        main = GetComponent<ViewHome>();
#if UNITY_EDITOR
        OX_DataLoader.InitOriginalData();
        //OX_DataLoader.TestMyList();
        //NetWorkManager.Instance.LoadDataFromServer();
        StartCoroutine(NetWorkManager.Instance.GetVocabList());
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });
        StartCoroutine(NetWorkManager.Instance.GetMyNoteList());
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });
        FileReadWrite.Instance.PrepareUserDataJson();
        UserDataManager.Instance.InitUserNoteCount();

        GameEventMessage.SendEvent("PrepareDataDone");
#endif
    }

    public void OnView()
    {
        StatusBar.SetStatusTitle((int) Title.HOME);
        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(false);
    }

    public void OnClickVocab()
    {
        UnityEngine.Debug.Log("Vocab Touched !!!");
    }
}
