using System;

public class AlternateRandom
{
    private Random random;

    public AlternateRandom(int seed, int mod, int offset)
    {
        random = new Random(seed);

    }
}
