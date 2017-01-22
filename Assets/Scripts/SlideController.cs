using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlideController : MonoBehaviour
{
    private static SlideController instance;
    //----------------------------------------------------------------//
    public static SlideController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = SlideController.FindObjectOfType<SlideController>();
                if (instance == null)
                {
                    throw new UnityException("SlideController not found.");
                }
            }
            return instance;
        }
    }
    //----------------------------------------------------------------//
    void Awake()
    {
        instance = GameObject.FindObjectOfType<SlideController>();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public SlideInfo[] slides;
    public string nextSceneName;

    private int _slideIndex;
    private int _textIndex;

    // Use this for initialization
    void Start() {
        if (slides != null)
        {
            foreach (SlideInfo s in slides)
            {
                if (s != null && s.background != null)
                {
                    s.background.gameObject.SetActive(false);
                    if (s.timeAfterTexts < 0)
                        s.timeAfterTexts = 0;
                    if (s.timeBeforeText < 0)
                        s.timeBeforeText = 0;
                    if (s.timeBetweenTexts < 0)
                        s.timeBetweenTexts = 0;
                    Fade sFade = s.background.gameObject.GetComponent<Fade>();
                    if (sFade != null)
                    {
                        initSlideFade(sFade);
                    }
                    if (s.testi != null)
                    {
                        foreach (TextInfo t in s.testi)
                        {
                            if (t != null && t.text != null)
                            {
                                Fade tFade = t.text.gameObject.GetComponent<Fade>();
                                if (tFade != null)
                                {
                                    initTextFade(tFade, t.time);
                                }
                                if (t != null && t.text != null)
                                {
                                    t.text.gameObject.SetActive(false);
                                    if (t.text.text == null)
                                        t.text.text = "";
                                    if (t.time < 0)
                                        t.time = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        _textIndex = -1;
        _slideIndex = 0;
        StartCoroutine(ChangeSlideCoroutine(null, slides[_slideIndex]));
    }

    private float initTextFade(Fade f, float contentTime)
    {
        float time = contentTime + f.fadeConfiguration.fadeInTime + f.fadeConfiguration.fadeOutTime;
        f.fadeConfiguration.alphaMax = 1;
        f.fadeConfiguration.alphaMin = 0;
        f.fadeConfiguration.disableOnAlphaMax = false;
        f.fadeConfiguration.looped = false;
        f.fadeConfiguration.notFadeIn = false;
        f.fadeConfiguration.notFadeOut = false;
        f.fadeConfiguration.notTimed = false;
        f.fadeConfiguration.stayOnAlphaMaxTime = time;
        f.fadeConfiguration.stayOnAlphaMinTime = 0;
        return time;
    }

    private void initSlideFade(Fade f)
    {
        f.fadeConfiguration.alphaMax = 1;
        f.fadeConfiguration.alphaMin = 0;
        f.fadeConfiguration.disableOnAlphaMax = false;
        f.fadeConfiguration.looped = false;
        f.fadeConfiguration.notFadeIn = false;
        f.fadeConfiguration.notFadeOut = true;
        f.fadeConfiguration.notTimed = true;
        f.fadeConfiguration.stayOnAlphaMaxTime = 0;
        f.fadeConfiguration.stayOnAlphaMinTime = 0;
    }

    IEnumerator StartTextCoroutine (TextInfo t, float waitTimeBeforeText)
    {
        yield return new WaitForSecondsRealtime(waitTimeBeforeText);
        if (t.text != null)
            t.text.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(t.time);
        next();
    }

    IEnumerator ChangeSlideCoroutine (SlideInfo oldSlide, SlideInfo newSlide)
    {
        _textIndex = -1;
        if (oldSlide != null)
            yield return new WaitForSecondsRealtime(oldSlide.timeAfterTexts);
        if (newSlide != null)
        {
            if (newSlide.background != null)
                newSlide.background.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(newSlide.timeAfterTexts);
            next();
        }
        else
        {
            Debug.Log("Debug: Slide controller stopped.");
        }
    }

    public void next ()
    {
        if (_slideIndex < slides.Length)
        {
            SlideInfo currentSlide = null;
            TextInfo currentText = null;
            currentSlide = slides[_slideIndex];
            if (_textIndex >= 0 && _textIndex < currentSlide.testi.Length)
                currentText = currentSlide.testi[_textIndex];
            if (currentText != null && currentText.text != null)
                currentText.text.gameObject.SetActive(false);
            do
            {
                _textIndex++;
            } while (_textIndex < currentSlide.testi.Length && currentSlide.testi[_textIndex] == null);
            if (_textIndex < currentSlide.testi.Length)
            {
                StartCoroutine(StartTextCoroutine(currentSlide.testi[_textIndex], currentSlide.timeBetweenTexts));
            }
            else
            {
                do
                {
                    _slideIndex++;
                } while (_slideIndex < slides.Length && slides[_slideIndex] == null);
                if (_slideIndex < slides.Length)
                {
                    StartCoroutine(ChangeSlideCoroutine(currentSlide, slides[_slideIndex]));
                }
                else
                {
                    SceneManager.LoadScene(nextSceneName);
                }
            }
        }
    }
}

[System.Serializable]
public class SlideInfo
{
    public Image background;
    public TextInfo[] testi;
    public float timeBeforeText;
    public float timeAfterTexts;
    public float timeBetweenTexts;
}

[System.Serializable]
public class TextInfo
{
    public Text text;
    public float time;
}