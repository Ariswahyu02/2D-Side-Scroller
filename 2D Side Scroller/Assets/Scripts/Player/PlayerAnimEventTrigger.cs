using UnityEngine;

// To Execute events from Animation Clips
public class PlayerAnimEventTrigger : MonoBehaviour
{
    public void ExecuteOnDeadEvent()
    {
        GameManager.Instance.GameOver();
    }
}
