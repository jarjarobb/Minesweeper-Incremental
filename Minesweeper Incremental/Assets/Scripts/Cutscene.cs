using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    [SerializeField]bool fadeInBackground;
    [SerializeField]bool fadeOutBackground;
    [SerializeField] bool fadeInText;
    [SerializeField]bool fadeOutText;
    float fadeSpeed = 1f;
    [SerializeField] Canvas cutsceneCanvas;
    [SerializeField] Image whiteBackground;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float waitTimeBetweenActions = 2;

    private void Awake()
    {
        int NumberOfCutscenes = FindObjectsOfType<Cutscene>().Length;
        if (NumberOfCutscenes > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void SelfDestruct()
    {
        if (SceneManager.GetActiveScene().name != "Heaven Start Scene" || SceneManager.GetActiveScene().name != "Win Scene")
        {
            Destroy(gameObject);
        }
    }
    
    public IEnumerator HeavenCutscene()
    {
        text.text = "Welcome";
        text.color = new Color(0,0,0,0);
        whiteBackground.color = new Color(255,255,255,0);
        cutsceneCanvas.gameObject.SetActive(true);
        fadeInBackground = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        fadeInText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        fadeOutText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        text.text = "To";
        fadeInText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        fadeOutText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        text.text = "Heaven";
        fadeInText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        FindObjectOfType<SceneLoader>().LoadHeavenStartScene(false);
        fadeOutText = true;
        fadeOutBackground = true;
    }
    public IEnumerator EndgameCutscene(int endgames)
    {
        text.text = "All of your progress vanishes before your eyes...";
        text.color = new Color(255,255,255, 0);
        whiteBackground.color = new Color(0,0,0, 0);
        cutsceneCanvas.gameObject.SetActive(true);
        fadeInBackground = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        fadeInText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        fadeOutText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        text.text = "The end has come...";
        fadeInText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        fadeOutText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        text.text = "Endgames: " + endgames.ToString();
        fadeInText = true;
        yield return new WaitForSeconds(waitTimeBetweenActions);
        FindObjectOfType<PlayerStats>().HideEndgameDisplay();
        FindObjectOfType<SceneLoader>().LoadRealWinScene();
        fadeOutText = true;
        fadeOutBackground = true;
    }
    private void FadeInText()
    {
        Color textColor = text.color;
        float fadeAmount = textColor.a + (fadeSpeed * Time.deltaTime);

        textColor = new Color(textColor.r, textColor.g, textColor.b, fadeAmount);
        text.color = textColor;
        if (textColor.a >= 1)
        {
            fadeInText = false;
        }
    }
    private void FadeOutText()
    {
        Color textColor = text.color;
        float fadeAmount = textColor.a - (fadeSpeed * Time.deltaTime);

        textColor = new Color(textColor.r, textColor.g, textColor.b, fadeAmount);
        text.color = textColor;
        if (textColor.a <= 0)
        {
            fadeOutText = false;
        }
    }
    private void FadeInBackground()
    {
        Color whiteBackgroundColor = whiteBackground.color;
        float fadeAmount = whiteBackgroundColor.a + (fadeSpeed * Time.deltaTime);

        whiteBackgroundColor = new Color(whiteBackgroundColor.r, whiteBackgroundColor.g, whiteBackgroundColor.b, fadeAmount);
        whiteBackground.color = whiteBackgroundColor;
        if (whiteBackgroundColor.a >= 1)
        {
            fadeInBackground = false;
        }
    }

    private void Update()
    {
        if (fadeInBackground)
        {
            FadeInBackground();
        }
        if (fadeOutBackground)
        {
            FadeOutBackground();
        }
        if (fadeInText)
        {
            FadeInText();
        }
        if (fadeOutText)
        {
            FadeOutText();
        }
    }

    private void FadeOutBackground()
    {
        Color whiteBackgroundColor = whiteBackground.color;
        float fadeAmount = whiteBackgroundColor.a - (fadeSpeed * Time.deltaTime);

        whiteBackgroundColor = new Color(whiteBackgroundColor.r, whiteBackgroundColor.g, whiteBackgroundColor.b, fadeAmount);
        whiteBackground.color = whiteBackgroundColor;
        if (whiteBackgroundColor.a <= 0)
        {
            fadeOutBackground = false;
            Destroy(gameObject);
        }
    }
}
