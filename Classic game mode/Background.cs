using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = MetaSceneData.LevelData.Background;
    }
}
