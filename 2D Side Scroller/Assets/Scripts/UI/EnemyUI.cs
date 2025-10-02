using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Enemy enemy;

    private void Awake()
    {
        healthBarSlider.maxValue = enemy.GetMaxEnemyHealth();
        UpdateEnemyHealthBar();
        
        healthBarSlider.gameObject.SetActive(false);
    }
    
    public void UpdateEnemyHealthBar()
    {
        healthBarSlider.gameObject.SetActive(true);
        healthBarSlider.value = enemy.GetCurrentEnemyHealth();
    }
}
