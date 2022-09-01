using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public int score, hunger;

    [SerializeField] Text scoreText, endScoreText, highScoreText, reasonText;
    [SerializeField] Slider hungerBar;

    [SerializeField] GameObject mainUI, gameOverScreen, warningIcon, floatingText;

    [SerializeField] Transform rollingBlack;

    [SerializeField] Color red, green;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine("RestartCoroutine");
    }

    void Update()
    {
        if (GameplayManager.instance.playing)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        scoreText.text = score.ToString();
        hungerBar.value = hunger;
    }

    public void GameOver(string reason)
    {     
        mainUI.SetActive(false);

        StartCoroutine("GameOverCoroutine", reason);
    }

    public void Restart()
    {
        gameOverScreen.SetActive(false);

        StartCoroutine("RestartCoroutine");
    }

    public void PredatorWarning(Transform predator)
    {
        GameObject icon = Instantiate(warningIcon, mainUI.transform);

        icon.GetComponent<WarningIcon>().target = predator;
    }

    public void AddScore(int addition, Vector3 position)
    {
        GameObject txtObj = Instantiate(floatingText, mainUI.transform);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(position) + new Vector3(30, 30);
        Vector3 cappedScreenPos = new Vector3(Mathf.Clamp(screenPos.x, 30, Screen.width - 30), Mathf.Clamp(screenPos.y, 30, Screen.height - 50));
        txtObj.GetComponent<RectTransform>().position = cappedScreenPos;

        Text txt = txtObj.GetComponentInChildren<Text>();

        txt.text = addition.ToString();
        txt.color = addition > 0 ? green: red;
    }

    IEnumerator GameOverCoroutine(string reason)
    {
        float startTime = Time.timeSinceLevelLoad;

        while(rollingBlack.position.y > 0.1f)
        {
            rollingBlack.position -= new Vector3(0, 8f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        while(Time.timeSinceLevelLoad < startTime + 1)
        {
            yield return new WaitForEndOfFrame();
        }

        rollingBlack.GetComponent<SpriteRenderer>().sortingOrder = 15;

        gameOverScreen.SetActive(true);

        endScoreText.text = "Score: " + System.Environment.NewLine + score;
        highScoreText.text = "High Score: " + System.Environment.NewLine + PlayerPrefs.GetInt("High Score");
        reasonText.text = reason;

        yield return null;
    }

    IEnumerator RestartCoroutine()
    {
        GameplayManager.instance.DeleteDinos();

        while (rollingBlack.position.y < 10.6f)
        {
            rollingBlack.position += new Vector3(0, 8f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.2f);

        mainUI.SetActive(true);
        rollingBlack.GetComponent<SpriteRenderer>().sortingOrder = 5;

        GameplayManager.instance.Restart();

        yield return null;
    }
}
