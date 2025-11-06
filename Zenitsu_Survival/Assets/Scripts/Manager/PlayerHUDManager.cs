using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    [Header("Player(로직 스크립트 참조")]
    [SerializeField] private ZenitsuHealth _playerHealth;
    [SerializeField] private ZenitsuStats _playerStats;
    [SerializeField] private ZenitsuCombat _playerCombat;
    
    [Header("HP Bar(체력")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;
    
    [Header("EXP Bar(경험치)")]
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TextMeshProUGUI _levelText;

    [Header("Cooldowns(쿨타임)")]
    [SerializeField] Image _attackIconMask;
    [SerializeField] private TextMeshProUGUI _attackCooldownText;
    [SerializeField] Image _skillIconMask;
    [SerializeField] private TextMeshProUGUI _skillCooldownText;

    private void Awake()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged += UpdateHealthUI;
        }

        if (_playerStats != null)
        {
            _playerStats.OnLevelChanged += UpdateLevelUI;
            _playerStats.OnExperienceChanged += UpdateExperienceUI;
        }
    }

    private void OnDestory()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHealthUI;
        }

        if (_playerStats != null)
        {
            _playerStats.OnLevelChanged -= UpdateLevelUI;
            _playerStats.OnExperienceChanged -= UpdateExperienceUI;
        }
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (_hpSlider != null)
        {
            _hpSlider.value = currentHealth/maxHealth; 
        }

        if (_hpText != null)
        {
            _hpText.text = Mathf.Ceil(currentHealth)+ " / " + Mathf.Ceil(maxHealth);
        }
    }

    private void UpdateExperienceUI(float currentExp, float maxExp)
    {
        if (_expSlider != null)
        {
            _expSlider.value = currentExp/maxExp;
        }
    }
    private void UpdateLevelUI(int newLevel)
    {
        if (_levelText != null)
        {
            _levelText.text = "Lv." + newLevel;
        }
    }

    private void Update()
    {
        if (_playerCombat == null) return;

        float attackRemaining = _playerCombat.AttackCoolDownRemaining;
        float attackTotal = _playerCombat.AttackCooldownTotal;
        UpdateCooldownUI(_attackIconMask, _attackCooldownText, attackRemaining, attackTotal);
        
        float skillRemaining = _playerCombat.SkillCooldownRemaining;
        float skillTotal = _playerCombat.SkillCooldownTotal;
        UpdateCooldownUI(_skillIconMask, _skillCooldownText, skillRemaining, skillTotal);
    }

    private void UpdateCooldownUI(Image mask, TextMeshProUGUI text, float remaining, float total)
    {
        if (mask == null) return;
        if (remaining > 0f)
        {
            mask.gameObject.SetActive(true);
            mask.fillAmount = remaining / total;
            if (text != null)
            {
                text.text = remaining.ToString("F1");
            }
        }
        else
        {
            mask.gameObject.SetActive(false);
            if (text != null)
            {
                text.text = "";
            }
        }
    }
}
