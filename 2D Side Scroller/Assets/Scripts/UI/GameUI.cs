using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    [Header("Player Stats UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private WeaponUI currentWeaponUi;
    [SerializeField] private WeaponUI[] weaponUis;
    [SerializeField] private BuffUI[] currentBuffUi;
    [SerializeField] private BuffUI[] buffUis;


    private Vector3 gameOverPanelOriginalPosition;

    protected override void Awake()
    {
        base.Awake();
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(() => GameSceneManager.Instance.RestartGame());
        gameOverPanelOriginalPosition = gameOverPanel.transform.position;
    }

    private void Start()
    {
        UpdateInventoryUI();
        UpdateCurrentWeaponAndBuffUI();
    }

    public void FadeIn()
    {
        float fadeInDuration = GetAnimationLengthByName("FadeInScreen");
        fadeAnimator.SetTrigger("FadeIn");

        // wait fadeinduration then show game over panel
        DOVirtual.DelayedCall(fadeInDuration, ShowGameOverPanel);
    }

    public void FadeOut()
    {
        fadeAnimator.SetTrigger("FadeOut");
    }

    private void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.DOMoveY(gameOverPanelOriginalPosition.y, 0.5f).From(-Screen.height).SetEase(Ease.OutBack);
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // healthText.text = currentHealth + "/" + maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private float GetAnimationLengthByName(string animationName)
    {
        if (fadeAnimator == null) return 0f;

        RuntimeAnimatorController rac = fadeAnimator.runtimeAnimatorController;
        foreach (var clip in rac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f; // Animation not found
    }

    public void UpdateCurrentWeaponAndBuffUI()
    {
        if (InventoryManager.Instance.currentWeapon != null)
        {
            currentWeaponUi.transform.GetChild(0).gameObject.SetActive(true);
            currentWeaponUi.SetWeaponUI(InventoryManager.Instance.currentWeapon);
        }
        else
        {
            currentWeaponUi.transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < currentBuffUi.Length; i++)
        {
            if (InventoryManager.Instance.currentBuffs[i] != null)
            {
                currentBuffUi[i].transform.GetChild(0).gameObject.SetActive(true);
                currentBuffUi[i].SetBuffUI(InventoryManager.Instance.currentBuffs[i]);
            }
            else
            {
                currentBuffUi[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < weaponUis.Length; i++)
        {
            if (InventoryManager.Instance.availableWeapons[i] != null)
            {
                weaponUis[i].transform.GetChild(0).gameObject.SetActive(true);
                weaponUis[i].SetWeaponUI(InventoryManager.Instance.availableWeapons[i]);
            }
            else
            {
                weaponUis[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < buffUis.Length; i++)
        {
            if (InventoryManager.Instance.availableBuffs[i] != null)
            {
                buffUis[i].transform.GetChild(0).gameObject.SetActive(true);
                buffUis[i].SetBuffUI(InventoryManager.Instance.availableBuffs[i]);
            }
            else
            {
                buffUis[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
        {
            Time.timeScale = 0f;
            UpdateInventoryUI();
            UpdateCurrentWeaponAndBuffUI();
        }else
        {
            Time.timeScale = 1f;
        }
    }
}
