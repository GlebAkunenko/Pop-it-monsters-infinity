using UnityEngine.SceneManagement;
using UnityEngine;

public class ChestProduct : Product
{
    [SerializeField]
    private ClassicChestLoot chestLoot;

    public override void GiveProduct()
    {
        MetaSceneData.ChestLoot = chestLoot;
        MetaSceneData.InShop = true;
        SceneManager.LoadScene(MetaSceneData.chest_scene_name);
    }
}
