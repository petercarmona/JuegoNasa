using UnityEngine;
using System;
using TMPro;

using TMPro;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float elapsedTime;
    public float timeInterval = 10f;
    private float timer;

    public int score = 0;
    public int money = 10000;

    public GameObject MoneyUI;
    private TextMeshProUGUI moneyText; // cached reference

    public int cornQuantity = 0;
    public int sheepQuantity = 0;
    public int cowQuantity = 0;

    public static event Action OnTimeIntervalPassed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Cache the TMP component once
        if (MoneyUI != null)
            moneyText = MoneyUI.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= timeInterval)
        {
            timer = 0f;
            Debug.Log("Time interval reached! Increasing animal age...");
            OnTimeIntervalPassed?.Invoke();
        }

        // Update UI only if component is found
        if (moneyText != null)
            moneyText.text = $"{money} $";
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Score increased! Current score: {score}");
    }

    public void SubtractMoney(int amount)
    {
        money -= amount;
        if (money < 0) money = 0;
        Debug.Log($"New balance: {money}");
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log($"New balance: {money}");
    }
}