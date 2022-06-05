using UnityEngine;

public class StepFunction : MyFunction
{
    public override string GeoGebraUrl => "https://www.geogebra.org/classic/xft48ypj";

    [SerializeField]
    private int n;
    [SerializeField]
    private float k;

    public override float Func(int x)
    {
        float pi = Mathf.PI;
        float t = Mathf.Max(0, Mathf.Tan(x * pi / (2 * n))) + Mathf.Max(0, Mathf.Tan((x + n) * pi / (2 * n)));
        float a = Mathf.Pow(n, k);
        return Mathf.Pow(t, k) / a + x / n;
    }
}