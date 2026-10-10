using UnityEngine;
using TMPro;
using System;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text messages;
    [SerializeField] private float printTime = 3; 
    private float printRemain;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (messages.text != null)
        {
            printRemain -= Time.deltaTime;
            if (printRemain <=0)
            {
                messages.text = null;
            }
        }
    }

    public void PrintMessage(String message)
    {
        messages.text = message;
        printRemain = printTime;
    }
}
