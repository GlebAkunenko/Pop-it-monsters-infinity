//This script from tutorial by AndroidHelper.
//Link to the channel: https://www.youtube.com/c/huaweisonichelpAHRU
// код изменён

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SnapScrolling : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public int panCount => locations.Count;
    [Range(0f, 20f)]
    public float snapSpeed;
    public float scaleOffset;
    [Range(1f, 20f)]
    public float scaleSpeed;
    [Range(1, 2)]
    public float rootButtonsSpeedCoefficient;
    [Range(0, 1f)]
    public float minSize;
    [Range(0.5f, 2f)]
    public float maxSize;
    [Header("Other Objects")]
    public ScrollRect scrollRect;

    public List<ChooseLocationElement> locations;
    private Vector2[] pansPos;

    private RectTransform contentRect;
    private Vector2 contentVector;

    private int selectedPanID;
    private bool isScrolling;

    private void Start()
    {
        foreach (ChooseLocationElement location in locations)
            location.Init(minSize, maxSize, rootButtonsSpeedCoefficient); 

        contentRect = GetComponent<RectTransform>();
        pansPos = new Vector2[panCount];
        locations.Sort((a, b) => b.PosY.CompareTo(a.PosY));
        for (int i = 0; i < panCount; i++)
            pansPos[i] = -locations[i].transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (Interactable.CurrentMode != Interactable.Mode.game) {
            scrollRect.enabled = false;
            return;
        }
        scrollRect.enabled = true;

        if (contentRect.anchoredPosition.y >= pansPos[0].y && !isScrolling || contentRect.anchoredPosition.y <= pansPos[pansPos.Length - 1].y && !isScrolling)
            scrollRect.inertia = false;
        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++) {
            float distance = Mathf.Abs(contentRect.anchoredPosition.y - pansPos[i].y);
            if (distance < nearestPos) {
                nearestPos = distance;
                selectedPanID = i;
            }
            float scale = Mathf.Clamp(1 / distance * scaleOffset, minSize, maxSize);
            locations[i].TransormScale(scale, scaleSpeed * Time.fixedDeltaTime);
        }
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.y);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;
        contentVector.y = Mathf.SmoothStep(contentRect.anchoredPosition.y, pansPos[selectedPanID].y, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Scrolling(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Scrolling(false);
    }
}