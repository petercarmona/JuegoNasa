using TMPro;
using UnityEngine;

public class UpdateTimerUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject TimerTXT;
    [SerializeField] string current_timer = "00:00"; 
    void Start()
    {
        current_timer = TimerTXT.GetComponent<TextMeshPro>().text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
