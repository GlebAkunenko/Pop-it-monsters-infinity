using System;
using UnityEngine;

public class CrashlyticsTester : MonoBehaviour
{
    public void MakeCrash()
    {
        throw new Exception("Test exception");
    }
}