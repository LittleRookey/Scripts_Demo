using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance;


    public EquipmentTier weapon;
    public EquipmentTier topArmor;

    //[SerializeField] private 
    public UnityAction OnUpgradeSuccess;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }
    public bool UpgradeWeapon(EquipmentTier eTier)
    {
        if (ResourceManager.Instance.HasGold(eTier.requiredGold))
        {
            ResourceManager.Instance.UseGold(eTier.requiredGold);
            eTier.UpgradeLevel();
            OnUpgradeSuccess?.Invoke();
            Debug.Log("Upgrade Success");
            return true;
        } else
        {
            // Not enouugh gold to upgrade
            Debug.Log("Not ENough Gold");
            return false;
        }
        
    }

    // ���
    // ���� ��� ����
    // Start is called before the first frame update
   
}

