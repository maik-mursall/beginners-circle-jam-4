using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Gameplay.HypeMeter
{
    public class HypeMeterUI : MonoBehaviour
    {
        private HypeMeter _hypeMeter;
        [SerializeField] private Slider slider;

        void Start()
        {
            _hypeMeter = HypeMeter.Instance;
            _hypeMeter.HypeChanged += OnHypeChanged;
            
            slider.value = _hypeMeter.HypePercent;
        }
        
        private void OnHypeChanged(object src, EventArgs args)
        {
            slider.DOValue(_hypeMeter.HypePercent, 1f);
        }
    }
}
