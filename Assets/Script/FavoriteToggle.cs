using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class FavoriteToggle : MonoBehaviour
{
    [SerializeField] public GameObject check;
    [SerializeField] public OX_DataLoader.VocabData vocabData;
    private bool isOn = false;
    public void OnClickToggle()
    {
        if (isOn)
        {
            isOn = false;
            check.SetActive(false);
            ToggleOff();
        }
        else
        {
            isOn = true;
            check.SetActive(true);
            ToggleOn();
        }
    }
    public void ToggleOn()
    {
        NetWorkManager.Instance.ShowSelectNotePopup(vocabData.id);
    }

    public void ToggleOff()
    {
        NetWorkManager.Instance.RemoveMyVocab(vocabData.id);
    }

    public void SetCheck(bool isActive)
    {
        isOn = isActive;
        check.SetActive(isActive);
    }
}
