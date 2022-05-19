using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private static EquipmentManager instance = null;

    public static EquipmentManager Instance
    {
        get
        {
            var target = FindObjectOfType<EquipmentManager>();
            if (target)
            {
                instance = target;
            }
            else
            {
               var gm = GameObject.Find("GM");
                if (!gm) gm = new GameObject("GM");
                instance = gm.AddComponent<EquipmentManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private BulletList _bullets = default;

    public BulletList Bullets => _bullets;

    public Bullet[] Equipments = new Bullet[3];

    private int _equipID = 1;

    public void SetEquipments(int equipNum,Bullet bullet)
    {
        Equipments[equipNum - 1] = bullet;
    }

    public void SetSkill(int equipNum,int skillNum, PassiveSkill skill)
    {
        var equip = Equipments[equipNum - 1];
        var mySkill = skillNum == 1 ? equip.passiveSkill_1 : equip.passiveSkill_2 = skill;
    }

    public void SetEquipID(int value) => _equipID = value;
}
