using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayableTextPanel : MonoBehaviour
{
    // External components
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image panel;

    // State
    private bool isTimed = false;

    // Data
    private float timeToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimed)
        {
            TickDownTimer();
        }
        
    }

    private void TickDownTimer()
    {
        // Tick down the timer with ellapsed time
        if (timeToDisplay > 0f)
        {
            timeToDisplay -= Time.deltaTime;
        }
        // Handle timed out action
        else
        {
            Hide();
        }
    }

    public void ShowMessage(string message, float duration = 0f)
    {
        text.enabled = true;
        panel.enabled = true;
        text.text = message;
        // If no duration is set, display message until Hide() is called
        isTimed = duration > 0f;
        timeToDisplay = duration;
    }

    public void Hide()
    {
        text.enabled = false;
        panel.enabled = false;
    }
}