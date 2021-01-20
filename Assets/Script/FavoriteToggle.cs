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

    public void CheckButtonOff()
    {
        isOn = false;
        check.SetActive(false);
    }

    public void ToggleOn()
    {
        SelectNotePopup.toggleOffCallBack = () =>
        {
            isOn = false;
            check.SetActive(false);
        };
        NetWorkManager.Instance.ShowSelectNotePopup(vocabData.id, GetComponent<FavoriteToggle>(), null);
    }

    public void ToggleOff()
    {
        UserDataManager.Instance.RemoveMyVocabUserNote(vocabData.id);
        NetWorkManager.Instance.RemoveMyVocab(vocabData.id);
    }

    public void SetCheck(bool isActive)
    {
        isOn = isActive;
        check.SetActive(isActive);
    }
}
