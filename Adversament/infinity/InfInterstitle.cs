using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfInterstitle : MonoBehaviour
{
    private static SavableInt step;
    public static int Step
    {
        get => step;
        set => step.Set(value);
    }

    private static bool startGame = false;

    [SerializeField]
    private int monstersToShow;

    private void Start()
    {
        if (step == null)
            step = new SavableInt(1, "interstitle_step");

        if (!startGame) {
            startGame = true;
            return;
        }

        if (step > monstersToShow) {
            if (InfAds.Interstitle.IsReady()) {
                InfAds.Interstitle.ShowAds();
                monstersToShow = 0;
            }
        }
    }


}
