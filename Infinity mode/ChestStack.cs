using System.Collections.Generic;
using UnityEngine;

public class ConstantChestStack : IStack<InfChestLoot>
{
    private InfChestLoot chest;

    public ConstantChestStack(InfChestLoot chest)
    {
        this.chest = chest;
    }

    public override int offset => 0;

    public override InfChestLoot PopElementAndPushStack()
    {
        return chest;
    }

}
