using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayableTextPanel : MonoBehaviour
{
    // Components
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image panel;

    // State for behaviour
    private float timeToDisplay;
    private bool isTimed = false;

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
        if (timeToDisplay > 0f)
        {
            timeToDisplay -= Time.deltaTime;
        }
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
        isTimed = duration > 0f;
        timeToDisplay = duration;
    }

    public void Hide()
    {
        text.enabled = false;
        panel.enabled = false;
    }
}