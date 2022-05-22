using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletPresenter : MonoBehaviour
{
    public static BulletPresenter Instance { get => _instance; }

    private static BulletPresenter _instance;
    [SerializeField]
    private Text _bulletExplain = default;
    [SerializeField]
    private Text _bulletName = default;
    [SerializeField]
    private Text _bulletCost = default;

    static readonly float FixRate = 100f;

    private void Awake()
    {
        _instance = this;
    }
    public void SetExplanation(Bullet bullet) 
    {
        if (!bullet) return;
        _bulletName.text = "機能名 : " + bullet.Name;
        _bulletCost.text = "コスト : " + (bullet.ConsumeStanceValue * FixRate).ToString();
        _bulletExplain.text = bullet.ExplainText;
    }
}
