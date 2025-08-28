using UnityEngine;
using UnityEngine.UI;

public class ChargingBar : MonoBehaviour
{
    public Image chargingBar;

    [SerializeField] public float chargeBarValueTotal;
    [SerializeField] public float chargeAmout;


    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ChargeBar(chargeAmout);
        }
    }

    public void ChargeBar(float value)
    {
        chargeBarValueTotal -= value;
        chargingBar.fillAmount = chargeBarValueTotal / 9f;
    }

}
