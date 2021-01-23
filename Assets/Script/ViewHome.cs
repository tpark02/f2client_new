using System;
using System.Collections;
using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

public class ViewHome : MonoBehaviour
{
    public static Action hideBackButtonCallBack = null;
    public static ViewHome main = null;
    [SerializeField] public VocabTouchNotice homeVocabTouchNotice;
    [SerializeField] public Text vocab;
    [SerializeField] public Text def;
    [SerializeField] public Text ex;
    [SerializeField] public VocabPanel vocabPanel;
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
        
        StartCoroutine(NetWorkManager.Instance.GetTodayVocabDetailCo());
        yield return new WaitWhile(() =>
        {
            return false == NetWorkManager.Instance.isJsonDone;
        });
        FileReadWrite.Instance.PrepareUserDataJson();
        UserDataManager.Instance.InitUserNoteCount();
        
        SetTodayVocabDetail();

        hideBackButtonCallBack();
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

    public void SetTodayVocabDetail()
    {
        var vocabdata = UserDataManager.Instance.todayVocabData;
        
        vocab.text = vocabdata.vocab;
        vocabPanel.vocab = vocabdata.vocab;
        //CurrentVocab cData = new CurrentVocab( vocabdata.def, vocabdata.type, vocabdata.e1, vocabdata.t1, vocabdata.t2, vocabdata.e2);
        var cData = OX_DataLoader.GetVocabDataByVocab(vocabdata.vocab);
        vocabPanel.vocabData = cData;

        if (vocabdata.def.Equals("empty"))
        {
            return;
        }
        var deflist = vocabdata.def.Split(new string[] { "[t]" }, StringSplitOptions.None);

        def.text = string.Empty;
        for (int i = 0; i < deflist.Length; i++)
        {
            def.text += deflist[i];
            if (i == deflist.Length - 1)
            {
                break;
            }
            def.text += "\n";
        }

        var ee1 = OX_DataLoader.ColorVocab(vocabdata.e1.ToLower(), vocabdata.vocab);
        var tt1 = vocabdata.t1;
        var ee2 = OX_DataLoader.ColorVocab(vocabdata.e2.ToLower(), vocabdata.vocab);
        var tt2 = vocabdata.t2;

        ex.text = ee1
                  + "<size=25>" + "\n"
                  + tt1
                  + "</size>" + "\n\n"
                  + ee2
                  + "<size=25>" + "\n"
                  + tt2 + "</size>";

        vocabPanel.SetColor(false);

        bool isExist = UserDataManager.Instance.IsVocabExist(cData.id);

        if (isExist)
        {
            vocabPanel.SetColor(true);
        }
    }
}
