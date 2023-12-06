using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotCounterUI : MonoBehaviour
{
    public TMP_Text counterText;
    public string fixedNum;

    // Start is called before the first frame update
    void Start()
    {
        counterText = FindObjectOfType<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        RubyController player = GameObject.FindGameObjectWithTag("Player").GetComponent<RubyController>();
        if (player != null)
        {
            fixedNum = player.fixedRobotsString;
        }
        if (player == null)
        {
            fixedNum = "N/A";
        }
        counterText.text = fixedNum;
    }
}
