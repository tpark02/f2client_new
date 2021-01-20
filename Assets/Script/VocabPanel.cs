using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VocabPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Image vocabPanel;
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;
    Color32[] vocabPanelColorList = new Color32[2];
    private int currentColorIndex = 0;
    public string vocab = string.Empty;
    public OX_DataLoader.VocabData vocabData;

    //[HideInInspector] public int vocabId = -1;
    void Start()
    {
        vocabPanelColorList[0] = new Color32(255, 255, 255, 150);
        vocabPanelColorList[1] = new Color32(55, 71, 79, 150);
    }

    public void SetColor(bool isChecked)
    {
        if (isChecked)
        {
            vocabPanel.color = vocabPanelColorList[1];
            currentColorIndex = 1;
        }
        else
        {
            vocabPanel.color = vocabPanelColorList[0];
            currentColorIndex = 0;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        clicked++;
        if (clicked == 1)
        {
            clicktime = Time.time;
        }

        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            
            Debug.Log("Double CLick: " + this.GetComponent<RectTransform>().name);
            
            if (currentColorIndex == 0)
            {
                currentColorIndex = 1;
                NetWorkManager.Instance.ShowSelectNotePopup(vocabData.id, null, gameObject.GetComponent<VocabPanel>());
            }
            else if (currentColorIndex == 1)
            {
                currentColorIndex = 0;
                UserDataManager.Instance.RemoveMyVocabUserNote(vocabData.id);
                NetWorkManager.Instance.RemoveMyVocab(vocabData.id);
            }

            vocabPanel.color = vocabPanelColorList[currentColorIndex];

        }
        else if (clicked > 2 || Time.time - clicktime > 1)
        {
            clicked = 0;
        }
    }
}
