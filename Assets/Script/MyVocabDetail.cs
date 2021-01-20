using System;
using DG.Tweening;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyVocabDetail : MonoBehaviour
{
    [SerializeField] public Text vocab;
    [SerializeField] public Text def;
    [SerializeField] public Text ex;
    [SerializeField] public VocabPanel vocabPanel;
    public static Action showBackButtonCallBack = null;

    public static GameObject myVocabDetail;

    public static bool isDetailDone = false;

    void Start()
    {
        myVocabDetail = gameObject;
    }

    public void OnView()
    {
        showBackButtonCallBack();
        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(true);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().addNewNoteButton.gameObject.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().ResetSelectedVocabScrollPos();
    }
    
    public void SetVocabDetail(OX_DataLoader.VocabData data, string v, string d, string e1, string t1, string e2, string t2)
    {
        var deflist = d.Split(new string[] { "[t]" }, StringSplitOptions.None);
        vocab.text = v;
        vocabPanel.vocab = v;
        //vocabPanel.vocabId = data.id;
        vocabPanel.vocabData = data;

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

        ex.text = e1 + "<size=25>" + "\n" + t1 + "</size>" + "\n\n" + e2 + "<size=25>" + "\n" + t2 + "</size>";

        vocabPanel.SetColor(false);

        bool isVocabExist = UserDataManager.Instance.IsVocabExist(data.id);
        if (isVocabExist)
        {
            vocabPanel.SetColor(true);
        }

        // 여기에 나중에 add vocab DB에 넣는 기능 넣어줘야한다.
    }
}