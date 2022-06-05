using System.Collections;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    [SerializeField]
    private GameObject textObject;
    private TextMeshProUGUI timeText;
    private Coroutine routine;

    public bool TimePause { get; set; }
    public bool GamePause { get; set; }
    public int Time { get; set; }

    private IEnumerator Counter()
    {
        while (true) {

            while (TimePause || GamePause) {
                yield return new WaitForEndOfFrame();
            }

            Time += 1;
            string s = (Time / 10).ToString() + "." + (Time % 10).ToString();
            timeText.text = s;

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Init()
    {
        timeText = textObject.GetComponent<TextMeshProUGUI>();
        if (timeText == null)
            throw new System.NullReferenceException();
    }

    public void Hide()
    {
        textObject.SetActive(false);
    }

    public void Begin()
    {
        routine = StartCoroutine(Counter());
    }

    public void Stop()
    {
        StopCoroutine(routine);
    }
}
