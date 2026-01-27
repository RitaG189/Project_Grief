using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;

public class VideoManager : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public VideoPlayer player;
    public Image progress;

    // Evento que outros scripts podem ouvir
    public UnityEvent OnVideoFinished;

    void Awake()
    {
        if (player != null)
        {
            player.loopPointReached += VideoEnded;
        }
    }

    void OnDestroy()
    {
        if (player != null)
        {
            player.loopPointReached -= VideoEnded;
        }
    }

    void Update()
    {
        if (player != null && player.frameCount > 0)
            progress.fillAmount = (float)player.frame / (float)player.frameCount;
    }

    private void VideoEnded(VideoPlayer vp)
    {
        OnVideoFinished?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    private void SkipToPercent(float pct)
    {
        var frame = player.frameCount * pct;
        player.frame = (long)frame;
    }

    private void TrySkip(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            progress.rectTransform, eventData.position, null, out localPoint))
        {
            float pct = Mathf.InverseLerp(
                progress.rectTransform.rect.xMin,
                progress.rectTransform.rect.xMax,
                localPoint.x);

            SkipToPercent(pct);
        }
    }

    public void VideoPlayerPause()
    {
        if (player != null)
            player.Pause();
    }

    public void VideoPlayerPlay()
    {
        if (player != null)
            player.Play();
    }
}