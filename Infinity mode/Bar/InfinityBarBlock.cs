using UnityEngine;
using UnityEngine.UI;


public class InfinityBarBlock : MonoBehaviour
{
    [SerializeField]
    private Image colorImage;
    [SerializeField]
    private Image whiteImage;

    /// <param name="icon">Main part of bar icon which was colored</param>
    /// <param name="color">Color of icon sprite</param>
    /// <param name="white">White sprite of bar icon. If null it will ont exist</param>
    public void Draw(Sprite icon, Color color, Sprite white=null)
    {
        colorImage.sprite = icon;
        colorImage.color = color;
        if (white != null) {
            whiteImage.sprite = white;
            whiteImage.color = Color.white;
        }
        else
            whiteImage.color = new Color(1, 1, 1, 0);
    }

    public void Draw(IBarElement info)
    {
        Draw(info.ColoredBarIcon, info.BarIconColor, info.WhiteBarIcon);
    }

}
