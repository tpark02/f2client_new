using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewVocabDay : MonoBehaviour
{
    [SerializeField] private Transform content;

    public static Action showBackButtonCallBack = null;

    private Vector3 startPos;

    void Start()
    {
        var d = OX_DataLoader.eachDayVocabCount;
        for (int i = 0; i < d; i++)
        {
            content.transform.GetChild(i).GetComponent<VocabDayButton>().SetDay("Day " + (i + 1).ToString(), i);
        }

        startPos = content.localPosition;
        BackButtonController.resetVocabDayScrollPos = ResetScrollPos;
    }

    public void OnView()
    {
        showBackButtonCallBack();
        ResetScrollPos();
        StatusBar.statusBar.GetComponent<StatusBar>().sortButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().sortPanel.SetActive(false);

        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabButton.SetActive(false);
        StatusBar.statusBar.GetComponent<StatusBar>().selectVocabScroll.SetActive(false);
    }
    public void ResetScrollPos()
    {
        content.localPosition = new Vector3(startPos.x, startPos.y, 0f);
    }
}
