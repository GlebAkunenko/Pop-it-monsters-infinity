using UnityEngine;
using UnityEngine.Events;

public class AddHealthButton : PopButton
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
    [SerializeField]
    private FlyCoin[] flyCoins;
    [SerializeField]
    private bool trainingMode;

    private Color defaultTextColor;
    private PlayerStorageData storage;

    private int GetFlyingCoinsCount()
    {
        int result = 0;
        foreach (FlyCoin fly in flyCoins)
            result += fly.IsFlying ? 1 : 0;
        return result;
    }

    private void UpdateCountText(int value)
    {
        countText.text = value.ToString();
        if (value == 0 && !trainingMode)
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

    private void AddHealthByAds()
    {
        if (PlayerInfo.Health + GetFlyingCoinsCount() >= PlayerInfo.Self.HealthMax)
            return;
        if (trainingMode)
            throw new System.Exception();
        flyCoins[PlayerInfo.Health + GetFlyingCoinsCount()].Fly();
    }

    public void Init()
    {
        defaultTextColor = countText.color;
        storage = PlayerStorageData.GetInstance();
        
        if (!trainingMode)
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
        if (PlayerInfo.Health + GetFlyingCoinsCount() >= PlayerInfo.Self.HealthMax)
            return;
        if (trainingMode)
            flyCoins[PlayerInfo.Health + GetFlyingCoinsCount()].Fly();
        else if (storage.HealthCoins > 0) {
            storage.HealthCoins--;
            UpdateCoinsButtonState(storage.HealthCoins);
            UpdateCountText(storage.HealthCoins);
            flyCoins[PlayerInfo.Health + GetFlyingCoinsCount()].Fly();
        }
    }

    public void TryAddByAds()
    {
        InfAds.AddHealthBlock.ShowAds(AddHealthByAds);
    }
}
