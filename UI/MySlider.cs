using System;
using UnityEngine;
using UnityEngine.UI;

public class MySlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private int currentValue;

    public int SliderBarrier { set; get; }

    public int CurrentValue
    {
        get => currentValue;
        set
        {
            if (currentValue != value) {
                currentValue = value;
                ValueChanged?.Invoke(value);
            }
        }
    }

    public void Init(int maxValue)
    {
        if (!slider.wholeNumbers)
            throw new System.Exception("No only whole numbers in slider");
        SliderBarrier = Mathf.Min((int)slider.maxValue, maxValue);
    }

    private void Update()
    {
        if (slider.value > SliderBarrier)
            slider.value = SliderBarrier;
        CurrentValue = (int)slider.value;
    }

    public event Action<int> ValueChanged;
}
