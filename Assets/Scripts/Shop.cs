using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] int sheepPrice = 100;
    [SerializeField] int sheepSell = 100;

    [SerializeField] int cowPrice = 100;
    [SerializeField] int cowSell = 100;
    
    [SerializeField] int CornPrice = 100;
    [SerializeField] int CornSell = 100;
    
    public void BuyCorn()
    {
        int currentMoney = GameManager.Instance.money;

        if (currentMoney >= CornPrice && GameManager.Instance.cornQuantity < 10)
        {
            GameManager.Instance.SubtractMoney(CornPrice);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void SellCorn()
    {
        if (GameManager.Instance.cornQuantity > 0)
            GameManager.Instance.AddMoney(CornSell);


    }public void BuySheep()
    {
        int currentMoney = GameManager.Instance.money;

        if (currentMoney >= sheepPrice && GameManager.Instance.sheepQuantity < 6)
        {
            GameManager.Instance.SubtractMoney(sheepPrice);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void SellSheep()
    {
        if (GameManager.Instance.sheepQuantity > 0)
            GameManager.Instance.AddMoney(sheepSell);


    }public void BuyCow()
    {
        int currentMoney = GameManager.Instance.money;

        if (currentMoney >= cowPrice && GameManager.Instance.cowQuantity < 10)
        {
            GameManager.Instance.SubtractMoney(cowPrice);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void SellCow()
    {
        if (GameManager.Instance.cowQuantity > 0)
            GameManager.Instance.AddMoney(cowSell);
    }
}
