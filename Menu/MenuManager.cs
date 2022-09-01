using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Texture2D cursor;

    [SerializeField] Renderer background;

    [SerializeField] float backgroundSpeed;

    [SerializeField] GameObject mainUI, helpUI;

    [SerializeField] Transform rollingBlack;

    private void Start()
    {
        Vector2 hotspot = new Vector2(cursor.width / 2, cursor.height / 2);

        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }

    private void Update()
    {
        Vector2 offset;

        if(background.material.mainTextureOffset.y <= -1)
        {
            offset = new Vector2(0, 1 - (backgroundSpeed * Time.deltaTime));
        }
        else
        {
            offset = new Vector2(0, -backgroundSpeed * Time.deltaTime);
        }

        background.material.mainTextureOffset += offset;
    }

    public void Instructions()
    {
        helpUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public void Back()
    {
        helpUI.SetActive(false);
        mainUI.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine("Transition");
    }

    IEnumerator Transition()
    {
        AsyncOperation loadLevel = SceneManager.LoadSceneAsync(1);
        loadLevel.allowSceneActivation = false;

        mainUI.SetActive(false);

        while(backgroundSpeed < 0.5f)
        {
            backgroundSpeed += Time.deltaTime / 1.8f;
            yield return new WaitForEndOfFrame();
        }

        while(rollingBlack.position.y > 0.1f)
        {
            rollingBlack.position -= new Vector3(0, Time.deltaTime * 10f);
            yield return new WaitForEndOfFrame();
        }

        while (loadLevel.progress < 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }

        loadLevel.allowSceneActivation = true;
    }
}
