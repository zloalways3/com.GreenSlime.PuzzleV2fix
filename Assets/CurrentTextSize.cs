using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrentTextSize : MonoBehaviour
{
    public TMP_Text textComponent; // Ссылка на ваш компонент Text с текстом
    public RectTransform content;

    public ScrollRect rect;

    void Start()
    {
        CheckTextBlock();
    }

    public void CheckTextBlock() {
        float textHeight = textComponent.preferredHeight;

        Vector2 sizeDelta = content.sizeDelta;
        sizeDelta.y = textHeight;
        content.sizeDelta = sizeDelta;

        rect.normalizedPosition = new Vector2(0, 1);
    }
}
