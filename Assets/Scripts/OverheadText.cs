using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverheadText : MonoBehaviour
{
    // Components
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image panel;

    private float timeToDisplay;
    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
        panel.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToDisplay > 0f)
        {
            timeToDisplay -= Time.deltaTime;
        }
        else
        {
            text.enabled = false;
            panel.enabled = false;
        }
    }

    public void ShowMessage(string message, float duration)
    {
        text.enabled = true;
        panel.enabled = true;
        timeToDisplay = duration;
        text.text = message;
    }
}