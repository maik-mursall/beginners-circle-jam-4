using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.Weapons
{
    public class WeaponCooldownUI : MonoBehaviour
    {
        [SerializeField] private WeaponBase weapon;
        [SerializeField] private String description;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image image;

        private void Awake()
        {
            image.fillAmount = 1f;
            text.text = description;
        }

        private void LateUpdate()
        {
            image.fillAmount = weapon.OnCooldown ? weapon.GetCurrentCooldownPercent() : 1f;
        }
    }
}
