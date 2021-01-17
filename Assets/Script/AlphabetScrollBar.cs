using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class AlphabetScrollBar : Scrollbar
{
    public float[] letterPercentages = new float[26];
    private bool initialized = false;
    public AlphabetScrollBar()
    {
        if (!initialized)
            Initialize();
    }
    private void Initialize()
    {
        for (int i = 0; i < 26; i++)
            letterPercentages[i] = (float)(26.0f - (float)i) / 26.0f;
        initialized = true;
    }
    public void SetLetterPercentages(ref List<string> names)
    {
        int[] letterCount = new int[26];
        for (int i = 0; i < names.Count; i++)
        {
            char curletter = names[i][0];
            if (curletter >= 'A' && curletter <= 'Z')
                letterCount[curletter - 'A']++;
            else if (curletter >= 'a' && curletter <= 'z')
                letterCount[curletter - 'a']++;
        }
        float lastPercentage = 0.0f;
        for (int i = 0; i < 26; i++)
        {
            letterPercentages[i] = lastPercentage;
            lastPercentage = (float)((float)letterCount[i] / (float)names.Count) + letterPercentages[i];
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        RectTransform barRect = (RectTransform)gameObject.transform;
        Vector3[] worldCorners = new Vector3[4];
        barRect.GetWorldCorners(worldCorners);
        float top = worldCorners[0].y;
        float bottom = worldCorners[1].y;
        float yHit = bottom - eventData.position.y;
        float scrollHeight = bottom - top;
        float percentage = yHit / scrollHeight;
        int index = (int)(percentage * 26.0f);
        value = 1.0f - letterPercentages[index];
    }
}
