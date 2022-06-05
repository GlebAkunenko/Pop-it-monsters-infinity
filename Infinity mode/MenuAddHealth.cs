using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuAddHealth : PopButton
{
    [SerializeField]
    private UnityEvent<Color> ChangeSpriteColor;
    [SerializeField]
    private TMPro.TextMeshProUGUI countText;
    [SerializeField]
    private PopButton adsButton;

    [SerializeField]
    private Color enableColor;
    [SerializeField]
    private Color disableColor;
    [SerializeField]
    private Color zeroCoinsTextColor;

    private Color defaultTextColor;
    private PlayerStorageData storage;

    private void UpdateCountText(int value)
    {
        countText.text = value.ToString();
        if (value == 0)
            countText.color = zeroCoinsTextColor;
        else
            countText.color = defaultTextColor;
    }

    private void UpdateCoinsButtonState(int healthCoins)
    {
        if (healthCoins < 1)
            ChangeSpriteColor?.Invoke(disableColor);
        else
            ChangeSpriteColor?.Invoke(enableColor);
    }

    public void Init()
    {
        defaultTextColor = countText.color;
        storage = PlayerStorageData.GetInstance();

        UpdateState(storage.HealthCoins);
    }

    public void UpdateState(int healthCoins)
    {
        if (healthCoins > 0) {
            UpdateCoinsButtonState(healthCoins);
            adsButton.gameObject.SetActive(false);
        }
        else {
            if (InfAds.AddHealthBlock.IsReady())
                adsButton.gameObject.SetActive(true);
            else {
                UpdateCoinsButtonState(healthCoins);
                adsButton.gameObject.SetActive(false);
            }
        }
        UpdateCountText(healthCoins);
    }

    public void TryUseCoin()
    {
        if (storage.HealthCoins > 0) {
            storage.HealthCoins--;
            UpdateCoinsButtonState(storage.HealthCoins);
            PlayerInfo.Self.AddLife();
        }
    }

    public void TryAddByAds()
    {
        InfAds.AddHealthBlock.ShowAds(PlayerInfo.Self.AddLife);
    }
}
