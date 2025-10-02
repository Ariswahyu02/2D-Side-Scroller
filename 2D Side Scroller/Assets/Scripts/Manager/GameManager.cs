using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void GameOver()
    {
        GameUI.Instance.FadeIn();
    }
}
