using UnityEngine;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private int price = 100;

    public void Buy()
    {
        int currentMoney = GameManager.Instance.money;

        if (currentMoney >= price)
        {
            GameManager.Instance.SubtractMoney(price);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

}
