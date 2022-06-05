using UnityEngine;

[System.Serializable]
public class Challange
{
    [SerializeField]
    private int challengId;
    [SerializeField]
    private bool time;
    [SerializeField]
    private ChallangeStack stack;

    public int ChallengId => challengId;
    public bool Time => time;
    public int PlayerHealth { get; set; }
    public ChallangeStack Stack => stack;
}

