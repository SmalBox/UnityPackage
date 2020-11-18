using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class PlayVideoContent : MonoBehaviour
{
    public MediaPlayer contentMediaPlayer;
    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode er)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.MetaDataReady:
                break;
            case MediaPlayerEvent.EventType.ReadyToPlay:
                break;
            case MediaPlayerEvent.EventType.Started:
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                contentMediaPlayer.Control.Play();
                gameObject.SetActive(false);
                break;
            case MediaPlayerEvent.EventType.Closing:
                break;
            case MediaPlayerEvent.EventType.Error:
                break;
            case MediaPlayerEvent.EventType.SubtitleChange:
                break;
            case MediaPlayerEvent.EventType.Stalled:
                break;
            case MediaPlayerEvent.EventType.Unstalled:
                break;
            default:
                break;
        }
    }
}
