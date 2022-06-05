using UnityEngine;
using System.Numerics;

public class ImposableCurve : MyFunction
{
    [SerializeField]
    private int N;
    [SerializeField]
    private int k;

    public override float Func(int x)
    {
        BigInteger xk = BigInteger.Pow(x, k);
        BigInteger a = BigInteger.Pow(N, k);
        return (float)new BigRational(xk, a);
    }

    public override string GeoGebraUrl => "https://www.geogebra.org/classic/zwc24mmr";
}
