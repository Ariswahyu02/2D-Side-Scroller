using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isGameWin = false;
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        SoundManager.Instance.PlayBGM("Main BGM", true, 0.25f);
    }

    public void GameOver()
    {
        GameUI.Instance.FadeInGameOver();
    }

    public void GameWin()
    {
        GameUI.Instance.FadeInGameWin();
    }
}
