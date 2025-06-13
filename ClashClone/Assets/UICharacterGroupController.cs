using UnityEngine;
using DG.Tweening;

public class UICharacterGroupController : MonoBehaviour
{
    [Header("Portrait GameObjects (0-2)")]
    public GameObject[] portraitObjects = new GameObject[3];

    [Header("Tween Settings")]
    public float selectedScale = 1.2f;
    public float normalScale = 1f;
    public float tweenDuration = 0.3f;

    private int currentIndex = -1;

    void Awake()
    {
        PlayerController.CharacterIndexSelected += OnPortraitSelected;
    }

    void OnDestroy()
    {
        PlayerController.CharacterIndexSelected -= OnPortraitSelected;
    }

    // Call this method when the event is received with the new index
    public void OnPortraitSelected(int index)
    {
        if (index < 0 || index >= portraitObjects.Length) return;
        if (currentIndex == index) return;

        for (int i = 0; i < portraitObjects.Length; i++)
        {
            var targetScale = (i == index) ? selectedScale : normalScale;
            portraitObjects[i].transform.DOScale(targetScale, tweenDuration).SetEase(Ease.OutBack);
        }
        currentIndex = index;
    }
}
