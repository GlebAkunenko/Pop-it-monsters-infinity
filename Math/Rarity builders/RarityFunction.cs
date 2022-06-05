using UnityEngine;

public class RarityFunction : MyFunction
{
    [SerializeField]
    private int N;
    [SerializeField]
    private int E;
    [SerializeField]
    private int S;
    [SerializeField]
    private float k;
    [SerializeField]
    private float b;
    
    public override float Func(int x)
    {
        float Nbk = Mathf.Pow(N + b, k);
        float bk = Mathf.Pow(b, k);
        float c = (E * Nbk - S * bk) / (Nbk - bk);
        float a = (S - c) * bk;
        return a / Mathf.Pow(x + b, k) + c;
    }

    public override string GeoGebraUrl => "https://www.geogebra.org/classic/ykx6p5ex";
}
