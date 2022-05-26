using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour,IDeselectHandler
{
    [SerializeField]
    private GameObject _checkMark = default;
    [SerializeField]
    private Image _bulletImage = default;
    [SerializeField]
    private Text _bulletName = default;

    Button _button;
    Bullet _set;
    PassiveSkill _skill;

    public int ID { get; set; }

    public Bullet Set => _set;

    public void OnDeselect(BaseEventData eventData)
    {
        _checkMark.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _checkMark.SetActive(false);
        _button.onClick.AddListener(() => InformationPresenter.Instance.SetExplanation(_set));
        _button.onClick.AddListener(() => InformationPresenter.Instance.SetExplanation(_skill));
    }

    public void SetInformation(Bullet bullet)
    {
        _bulletImage.sprite = bullet.Image;
        _bulletName.text = bullet.Name;
        _set = bullet;
    }

    public void SetInformation(PassiveSkill skill)
    {
        _bulletImage.sprite = skill.ImageBullet;
        _bulletName.text = skill.SkillName;
        _skill = skill;
    }
}
