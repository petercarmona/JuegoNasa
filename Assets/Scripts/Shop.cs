using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] int sheepPrice = 300;
    [SerializeField] int sheepSell = 800;

    [SerializeField] int cowPrice = 1300;
    [SerializeField] int cowSell = 2500;
    
    [SerializeField] int CornPrice = 100;
    [SerializeField] int CornSell = 150;
    
    public void BuyCorn()
    {
        int currentMoney = GameManager.Instance.money;

        if (currentMoney >= CornPrice && GameManager.Instance.cornQuantity < 10)
        {
            GameManager.Instance.SubtractMoney(CornPrice);
            GameManager.Instance.cornQuantity++;
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void SellCorn()
    {
        if (GameManager.Instance.cornQuantity > 0) {
            GameManager.Instance.AddMoney(CornSell);
            GameManager.Instance.cornQuantity--;
                }


    }public void BuySheep()
    {
        int currentMoney = GameManager.Instance.money;

        if (currentMoney >= sheepPrice && GameManager.Instance.sheepQuantity < 6)
        {
            GameManager.Instance.SubtractMoney(sheepPrice);
            GameManager.Instance.sheepQuantity++;
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void SellSheep()
    {
        if (GameManager.Instance.sheepQuantity > 0) 
        { 
        GameManager.Instance.AddMoney(sheepSell);
        GameManager.Instance.sheepQuantity--;
        }


    }public void BuyCow()
    {
        int currentMoney = GameManager.Instance.money;

        if (currentMoney >= cowPrice && GameManager.Instance.cowQuantity < 10)
        {
            GameManager.Instance.SubtractMoney(cowPrice);
            GameManager.Instance.cowQuantity++;
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void SellCow()
    {
        if (GameManager.Instance.cowQuantity > 0)
        {
            GameManager.Instance.AddMoney(cowSell);
            GameManager.Instance.cowQuantity--;
        }
    }
}
