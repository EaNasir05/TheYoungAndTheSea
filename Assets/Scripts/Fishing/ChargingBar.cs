using System;
using UnityEngine;
using UnityEngine.UI;

public class ChargingBar : MonoBehaviour
{
    public Image chargingBar;

    [SerializeField] public GameObject chargingBarTotalImage;
    [SerializeField] public float chargeBarValueTotal;
    [SerializeField] public float chargeAmout;

    public static event Action isMaxCharged;
    private bool _maxCharged = false;
    private bool _isShoot = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ChargeBar(chargeAmout);
        }
        if (Input.GetKeyUp(KeyCode.Space))
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
                }
            }
            else
            { 
                isMaxCharged?.Invoke();
                return;
            }
        }
    }
}
