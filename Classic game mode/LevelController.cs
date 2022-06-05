using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public abstract class LevelController : MonoBehaviour
{
    public static LevelController Self { protected set; get; }

    [SerializeField]
    private string menuSceneName = "Menu";
    public string MenuSceneName { get => menuSceneName; set => menuSceneName = value; }

    [SerializeField]
    protected Transform spawnTarget;
    [SerializeField]
    protected Transform leftTarget;
    [SerializeField]
    protected Transform rightTarget;
    [SerializeField]
    protected Transform middleTarget;

    private new Camera camera;
    public Camera Camera { get => camera; set => camera = value; }

    public bool TimePause { get; set; } = true;
    public bool OpenVictoryWindow { protected set; get; }
    public bool Win { get; set; }

    private bool notLose;
    public bool NotLose
    {
        get { return notLose && !MetaSceneData.LevelData.TimeLevel; }
    }

    public AudioSource SceneSounds { get; set; }

    protected Interactable.Mode lastMode;
    protected Animator currentMonterAnim;

    public bool GameGoing
    {
        get { return currentMonterAnim != null; }
    }

    [SerializeField]
    protected GameObject pausePanel;

    private bool gamePause;
    public bool GamePause
    {
        get { return gamePause; }
        set
        {
            if (gamePause == value)
                return;

            gamePause = value;
            if (gamePause) {
                pausePanel.SetActive(true);
                lastMode = Interactable.CurrentMode;
                currentMonterAnim.speed = 0;
                Interactable.CurrentMode = Interactable.Mode.pause;
            }
            else {
                pausePanel.SetActive(false);
                currentMonterAnim.speed = 1;
                Interactable.CurrentMode = lastMode;
            }
        }
    }

    public Transform LeftTarget { get => leftTarget; set => leftTarget = value; }
    public Transform RightTarget { get => rightTarget; set => rightTarget = value; }
    public Transform MiddleTarget { get => middleTarget; set => middleTarget = value; }

    public void SkipMonster()
    {
        currentMonterAnim.gameObject.GetComponent<Monster>().Kill();
    }

    public abstract void StartGame();
    public abstract void Exit();
    public abstract void NextMonster();
    public abstract void TryWin();
    public abstract IEnumerator Victory();
    public abstract void Lose();

}

public enum LevelState
{
    stay,
    going
}
