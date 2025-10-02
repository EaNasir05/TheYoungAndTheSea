using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChargingBar : MonoBehaviour
{
    public Image chargingBar;

    public float chargeBarValue;

    [SerializeField] public GameObject chargingBarTotalImage;
    [SerializeField] public float chargeBarValueTotal;
    [SerializeField] public float chargeAmout;

    public static event Action isMaxCharged;
    private bool _maxCharged = false;
    private bool _isShoot = false;

    public void Start()
    {
        chargeBarValue = chargeBarValueTotal;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !FishingPointsManager.instance.stop && !_isShoot)
        {
            ChargeBar(chargeAmout);
            chargingBarTotalImage.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Space) && !FishingPointsManager.instance.stop)
        {
            _isShoot = true;
            chargingBarTotalImage.SetActive(false);
        }
    }
    public void ChargeBar(float value)
    {
        if (!_isShoot) 
        {
            if (!_maxCharged)
            {
                chargeBarValueTotal -= value;
                chargingBar.fillAmount = chargeBarValueTotal / 9f;
                if (chargeBarValueTotal <= 0)
                {
                    _maxCharged = true;
                    chargingBarTotalImage.SetActive(false);
                }
            }
            else
            { 
                isMaxCharged?.Invoke();
                return;
            }
        }
    }

    public void Restart()
    {
        _maxCharged = false;
        _isShoot = false;
        chargeBarValueTotal = chargeBarValue;
        chargingBar.fillAmount = chargeBarValue;
    }

    public void OnEnable()
    {
        Hook.onFishOutOfWater += Restart;
    }

    public void OnDisable()
    {
        Hook.onFishOutOfWater -= Restart;
    }
}
