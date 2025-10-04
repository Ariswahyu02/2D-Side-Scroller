using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameUI : Singleton<GameUI>
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI winLoseText;

    [Header("Player Stats UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private WeaponUI currentWeaponUi;
    [SerializeField] private WeaponUI[] weaponUis;
    [SerializeField] private BuffUI[] currentBuffUi;
    [SerializeField] private BuffUI[] buffUis;

    [Header("Weapon Stat")]
    [SerializeField] private TextMeshProUGUI baseDamageText;
    [SerializeField] private TextMeshProUGUI baseFireRateText;
    [SerializeField] private TextMeshProUGUI afterBuffedDamageText;
    [SerializeField] private TextMeshProUGUI afterBuffedFireRateText;


    private Vector3 gameOverPanelOriginalPosition;
    PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(() => GameSceneManager.Instance.RestartGame());
        gameOverPanelOriginalPosition = gameOverPanel.transform.position;

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        UpdateInventoryUI();
        UpdateCurrentWeaponAndBuffUI();

    }

    public void FadeInGameOver()
    {
        float fadeInDuration = GetAnimationLengthByName("FadeInScreen");
        fadeAnimator.SetTrigger("FadeIn");

        SoundManager.Instance.PlayBGM("Game Over", false);
        // wait fadeinduration then show game over panel
        DOVirtual.DelayedCall(fadeInDuration, ShowGameDonePanel);
    }

    public void FadeInGameWin()
    {
        float fadeInDuration = GetAnimationLengthByName("FadeInScreen");
        fadeAnimator.SetTrigger("FadeIn");

        SoundManager.Instance.PlayBGM("Game Win", false);
        // wait fadeinduration then show game over panel
        DOVirtual.DelayedCall(fadeInDuration, ShowGameDonePanel);
    }

    private void ShowGameDonePanel()
    {
        if (!GameManager.Instance.isGameWin) winLoseText.text = "Game Over";
        else winLoseText.text = "Game Win";

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
        if (playerController.IsDead() || GameManager.Instance.isGameWin) return;

        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
        {
            Time.timeScale = 0f;
            UpdateInventoryUI();
            UpdateCurrentWeaponAndBuffUI();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void UpdateWeaponStats(float baseDamage, float baseFireRate, float buffedDamage, float buffedFireRate)
    {
        baseDamageText.text = baseDamage.ToString();
        baseFireRateText.text = baseFireRate.ToString();

        if (buffedDamage > baseDamage)
        {
            afterBuffedDamageText.gameObject.SetActive(true);
            afterBuffedDamageText.text = "->" + buffedDamage.ToString();
        }
        else afterBuffedDamageText.gameObject.SetActive(false);


        if (buffedFireRate > baseFireRate)
        {
            afterBuffedFireRateText.gameObject.SetActive(true);
            afterBuffedFireRateText.text = "->" + buffedFireRate.ToString();
        }
        else afterBuffedFireRateText.gameObject.SetActive(false);
    }

}
