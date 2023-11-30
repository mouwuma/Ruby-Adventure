using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayController : MonoBehaviour
{
    EnemyController a;

    public static GamePlayController instance;

    [SerializeField]
    private TextMeshProUGUI robotFixCountTxt;

    private int robotFixCount;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void robotFixed()
    {
        a=GameObject.FindGameObjectWithTag("TagA").GetComponent<EnemyController>();
        a.Fix();

        robotFixCount++;
        robotFixCountTxt.text = "Robot Fixed: " + robotFixCount;
    }

    public void RestartGame()
    {
        Invoke("Restart", 3f);
    }

    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
