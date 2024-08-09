using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextDisplay))]
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private AudioClip [] heistClips;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Image heistPanel;
    [SerializeField] private TextMeshProUGUI useText;
    [SerializeField] private TextMeshProUGUI cantHeistText;
    [SerializeField] private TextMeshProUGUI subtitles;
    [SerializeField] private Texture [] storyboards;
    [SerializeField] private RawImage heistImage;

    public TextDisplay textDisplay{get;set;}


    void Awake()
    {
        if(instance == null || instance != this)
        {
            instance = this;
            heistPanel.gameObject.SetActive(false);
            heistImage.gameObject.SetActive(false);
            textDisplay = GetComponent<TextDisplay>();
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetHeistImage(bool successful)
    {
        int idx = successful ? 1 : 0;
        heistImage.texture = storyboards[idx];
    }

    public void SetSubtitlesVisibility(bool active)
    {
        subtitles.gameObject.SetActive(active);
    }

    public void SetUseText(string text)
    {
        useText.text = text;
    }

    public void ToggleCrosshairVisibility()
    {
        crosshair.SetActive(!crosshair.activeSelf);
    }

    public void PlayHeistCutscene(bool successful)
    {
        SetHeistImage(successful);
        StartCoroutine(HeistCutscene(successful));
    }

    public IEnumerator HeistCutscene(bool successful)
    {
        int idx = successful ? 1 : 0;
        float timer = 0;
        heistPanel.gameObject.SetActive(true);
        while(timer < 2f)
        {
            timer += Time.deltaTime;
            Color c = Color.black;
            c.a = timer/2f;
            heistPanel.color = c;
            yield return null;
        }
        heistImage.gameObject.SetActive(true);
        AudioManager.instance.Play2DAudio(heistClips[idx],1,false,false);

        yield return new WaitForSeconds(heistClips[idx].length);
        if(successful)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            ScenesManager.instance.LoadScene(0);
        }
        else ScenesManager.instance.ReloadActiveScene();

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
