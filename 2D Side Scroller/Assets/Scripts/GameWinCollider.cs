using UnityEngine;

public class GameWinCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.isGameWin = true;
            GameManager.Instance.GameWin();
        }
    }
}
