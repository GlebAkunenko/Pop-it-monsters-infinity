using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarBar : MonoBehaviour
{
    [SerializeField]
    private GameObject starsFrame;
    [SerializeField]
    private Image[] progressStars = new Image[3];
    private bool[] pulsedStar = new bool[3];

    private IEnumerator StarFilling(float oldProgress, float newProgress)
    {
        float step = (newProgress - oldProgress) / 10f;
        float value = oldProgress;
        for (int i = 0; i < 10; i++) {
            value += step;
            int index = Mathf.FloorToInt(value);
            index = Mathf.Min(2, index);
            progressStars[index].fillAmount = value - index;

            if (index > 0) {
                if (!pulsedStar[index - 1]) {
                    progressStars[index - 1].fillAmount = 1;
                    progressStars[index - 1].gameObject.GetComponent<Animation>().Play();
                    pulsedStar[index - 1] = true;
                    FullOneStar?.Invoke();
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Init()
    {
        foreach (Image star in progressStars)
            star.fillAmount = 0;
    }

    public void Hide()
    {
        starsFrame.SetActive(false);
    }

    public void UpdateBar(int currentMonster, int monstersCount)
    {
        float new_progress = (float)currentMonster / monstersCount;
        float old_progress = Mathf.Max((float)(currentMonster - 1f) / monstersCount, 0);
        new_progress *= 3;
        old_progress *= 3;
        StartCoroutine(StarFilling(old_progress, new_progress));
    }

    public event Action FullOneStar;
}
