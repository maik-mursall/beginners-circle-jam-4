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

        private void Start()
        {
            image.fillAmount = 1f;
            text.text = description;

            weapon.CooldownStarted += OnCooldownStarted;
            weapon.CooldownFinished += OnCooldownFinished;
            weapon.CooldownUpdate += (sender, args) => OnCooldownUpdated(sender, (CooldownUpdateEventArgs)args);
        }
        
        private void OnCooldownStarted(object src, EventArgs args)
        {
            image.fillAmount = 0f;
        }

        private void OnCooldownFinished(object src, EventArgs args)
        {
            image.fillAmount = 1f;
        }
        
        private void OnCooldownUpdated(object src, CooldownUpdateEventArgs args)
        {
            image.fillAmount = args.TimeLeftPercent;
        }
    }
}
