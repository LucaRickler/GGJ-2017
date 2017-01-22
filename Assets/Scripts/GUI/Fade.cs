using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Fade : MonoBehaviour
{

    public enum FadeState
    {
        ZERO,
        FADE_IN,
        STAY_ON_ALPHA_MAX,
        FADE_OUT,
        STAY_ON_ALPHA_MIN,
        FINISHED
    }

    [Serializable]
    public class FadeInfo
    {
        [HideInInspector]
        public bool notFadeIn;
        [HideInInspector]
        public bool notFadeOut;
        [HideInInspector]
        public bool notTimed;
        [HideInInspector]
        public bool looped;
        public float fadeInTime;
        [HideInInspector]
        public float stayOnAlphaMaxTime;
        public float fadeOutTime;
        [HideInInspector]
        public float stayOnAlphaMinTime;
        [HideInInspector]
        public bool disableOnAlphaMax;
        [HideInInspector]
        [Range(0f, 1f)]
        public float alphaMin;
        [HideInInspector]
        [Range(0f, 1f)]
        public float alphaMax;
    }

    public FadeInfo fadeConfiguration;
    [HideInInspector]
    public FadeState state;
    public event Action onFinished;
    public event Action onFadeInFinished;
    public event Action onFadeOutFinished;
    public event Action onLoop;
    public event Action onFadeInStart;
    public event Action onFadeOutStart;

    private CanvasGroup canvasGroup;
    private bool started;
    private bool paused;
    private float startTime;
    private float pauseStartTime;

    public void Awake ()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            throw new UnityException ("CanvasGroup non trovato.");
        if (fadeConfiguration.looped && fadeConfiguration.notTimed)
            throw new UnityException("Fade Configuration cannot be looped and notTimed at the same time.");
        if (fadeConfiguration.alphaMin > fadeConfiguration.alphaMax)
            throw new UnityException("Min alpha value cannot be higher than max alpha value.");

    }

    public void OnEnable ()
    {
        restartFadeAndPlay();
    }

    public void OnDisable()
    {
        if (fadeConfiguration.disableOnAlphaMax)
            canvasGroup.alpha = fadeConfiguration.alphaMax;
        else
            canvasGroup.alpha = fadeConfiguration.alphaMin;
    }

    public void Update()
    {
        if (started && !paused)
        {
            bool next = false;
            if (state == FadeState.ZERO)
                next = true;
            else if (state == FadeState.FADE_IN)
            {
                float deltaAlpha = (Time.deltaTime / fadeConfiguration.fadeInTime) * (fadeConfiguration.alphaMax - fadeConfiguration.alphaMin);
                if (deltaAlpha > fadeConfiguration.alphaMax - canvasGroup.alpha)
                {
                    canvasGroup.alpha = fadeConfiguration.alphaMax;
                    next = true;
                }
                else
                    canvasGroup.alpha = canvasGroup.alpha + deltaAlpha;
            }
            else if (state == FadeState.FADE_OUT)
            {
                float deltaAlpha = (Time.deltaTime / fadeConfiguration.fadeOutTime) * (fadeConfiguration.alphaMax - fadeConfiguration.alphaMin);
                if (deltaAlpha > canvasGroup.alpha - fadeConfiguration.alphaMin)
                {
                    canvasGroup.alpha = fadeConfiguration.alphaMin;
                    next = true;
                }
                else
                    canvasGroup.alpha = canvasGroup.alpha - deltaAlpha;
            }
            else if (state == FadeState.STAY_ON_ALPHA_MAX)
            {
                if (fadeConfiguration.notTimed)
                    return;
                else if (Time.time - startTime >= fadeConfiguration.stayOnAlphaMaxTime)
                    next = true;
            }
            else if (state == FadeState.STAY_ON_ALPHA_MIN)
            {
                //if (fadeConfiguration.notTimed)
                //    return;
                //else
                if (Time.time - startTime >= fadeConfiguration.stayOnAlphaMinTime)
                    next = true;
            }
            else if (state == FadeState.FINISHED)
                throw new UnityException("Unsupported case.");
            else
                throw new UnityException("Unsupported case.");
            if (next)
                nextState();
        }       
    }

    public void restartFadeAndPlay ()
    {
        state = FadeState.ZERO;
        nextState();
        started = true;
        paused = false;
    }

    public void pause()
    {
        if (started && !paused)
        {
            paused = true;
            pauseStartTime = Time.time;
        }
    }

    public void play()
    {
        if (started && paused)
        {
            paused = false;
            startTime += Time.time - pauseStartTime;
        }
    }

    public void forceFadeOutAndFinish ()
    {
        if (fadeConfiguration.looped)
            throw new UnityException("Fade out cannot be forced if fade configuration is looped.");
        if (fadeConfiguration.notFadeOut)
            state = FadeState.FINISHED;
        else
            state = FadeState.FADE_OUT;
        if (state == FadeState.FINISHED)
        {
            started = false;
            if (onFinished != null)
                onFinished();
        }
    }

    private void nextState ()
    {
        switch (state)
        {
            case FadeState.ZERO:
                canvasGroup.alpha = fadeConfiguration.alphaMin;
                state = FadeState.FADE_IN;
                if (fadeConfiguration.notFadeIn)
                {
                    canvasGroup.alpha = fadeConfiguration.alphaMax;
                    state = FadeState.STAY_ON_ALPHA_MAX;
                }
                else if (onFadeInStart != null)
                    onFadeInStart();
                break;
            case FadeState.FADE_IN:
                canvasGroup.alpha = fadeConfiguration.alphaMax;
                state = FadeState.STAY_ON_ALPHA_MAX;
                if (onFadeInFinished != null)
                    onFadeInFinished();
                break;
            case FadeState.STAY_ON_ALPHA_MAX:
                canvasGroup.alpha = fadeConfiguration.alphaMax;
                state = FadeState.FADE_OUT;
                if (fadeConfiguration.notFadeOut)
                {
                    if (fadeConfiguration.looped)
                    {
                        state = FadeState.ZERO;
                        nextState();
                        if (onLoop != null)
                            onLoop();
                    }
                    else
                    {
                        canvasGroup.alpha = fadeConfiguration.alphaMin;
                        state = FadeState.STAY_ON_ALPHA_MIN;
                    }
                }
                else if (onFadeOutStart != null)
                    onFadeOutStart();
                break;
            case FadeState.FADE_OUT:
                canvasGroup.alpha = fadeConfiguration.alphaMin;
                state = FadeState.STAY_ON_ALPHA_MIN;
                if (onFadeOutFinished != null)
                    onFadeOutFinished();
                break;
            case FadeState.STAY_ON_ALPHA_MIN:
                if (fadeConfiguration.looped)
                {
                    state = FadeState.ZERO;
                    nextState();
                    if (onLoop != null)
                        onLoop();
                }
                else
                {
                    canvasGroup.alpha = fadeConfiguration.alphaMin;
                    state = FadeState.FINISHED;
                }
                break;
            case FadeState.FINISHED:
                break; // ignore
            default:
                throw new UnityException("Unsupported case.");
        }        
        startTime = Time.time;      
        if (state == FadeState.FINISHED)
        {
            started = false;
            if (onFinished != null)
                onFinished();
        }
    }

}
