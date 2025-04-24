using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;

    [SerializeField] private TextMeshProUGUI currentSouls;
    
    //[SerializeField] private Transform[] childrenTransform;
    //private int lastHealth;

    private SkillManager skills;
    void Start()
    {
        //lastHealth = playerStats.GetMaxHealthValue();
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }
        skills = SkillManager.instance;

    }
    
    void Update()
    {
        currentSouls.text = PlayerManager.instance.GetCurrentCurrency().ToString("#,#");
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked) 
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);
        
        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);
        
        if (Input.GetKeyDown(KeyCode.R) && skills.blackHole.blackHoleUnlocked)
            SetCooldownOf(blackHoleImage);
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);
        
        CheckCooldownOf(dashImage,skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordImage, skills.sword.cooldown);
        CheckCooldownOf(blackHoleImage,skills.blackHole.cooldown);
        CheckCooldownOf(flaskImage,Inventory.instance.flaskCoolDown);
    }
    private void UpdateHealthUI()
    {
        //IncreasingHealthBarLength(lastHealth);
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
        
    }

    /*private void IncreasingHealthBarLength(int lastHealth)
    {
        childrenTransform = gameObject.GetComponentsInChildren<Transform>();
        RectTransform rect = childrenTransform[1].GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x ,
            rect.sizeDelta.y * (playerStats.GetMaxHealthValue() / lastHealth)+ slider.value);
        
    }*/
    
    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCooldownOf(Image _image,float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
