using UnityEngine;
using TMPro;

public class TimerShower : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    public void SetTimerValue(int seconds)
    {
        string m = (seconds / 60).ToString();
        if (m.Length == 1)
            m = "0" + m;
        string s = (seconds % 60).ToString();
        if (s.Length == 1)
            s = "0" + s;
        textMesh.text = string.Format("{0}:{1}", m, s);
    }
}
