using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshProUGUI useText;
    [SerializeField] private TextMeshProUGUI cantHeistText;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void ToggleCrosshairVisibility()
    {
        crosshair.SetActive(!crosshair.activeSelf);
    }

    public void SetUseText(string text)
    {
        useText.text = text;
    }

    public IEnumerator FadeInAndOutText(string text, float duration)
    {
        float fadeDuration = duration/2;
        float timer = 0;
        cantHeistText.text = text;
        //fade in
        while(timer < fadeDuration)
        {
            cantHeistText.alpha = timer/fadeDuration;
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        while(timer < fadeDuration)
        {
            cantHeistText.alpha = 1 - timer/fadeDuration;
            timer += Time.deltaTime;
            yield return null;
        }

        cantHeistText.alpha = 0;
    } 

    
}
