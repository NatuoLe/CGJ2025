using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ThGold.Wwise;
using UnityEngine;
using AK.Wwise;
using MoreMountains.Feedbacks;
using ThGold.Common;
using Event = AK.Wwise.Event;

public class MuiscCallback : Singleton<MuiscCallback>
{
    // Start is called before the first frame update
    [ShowInInspector] private uint _musicPlayingId = 0;
    public Event music;
    private uint bgm_id = 0;

    public MMF_Player cameraShake;
    private float _currentPosition;
    [ShowInInspector] private int musicTime = 0;

    private static WwiseManager _wwiseManager => WwiseManager.Instance;


    public float lastHitBeatTime = 0;
    public float lastHitBarTime = 0;
    public float RemainWindowTime = 0; 
    void Start()
    {
    }

    [Button("Play")]
    public void Play()
    {
        uint callbackFlags = (uint) (AkCallbackType.AK_EnableGetMusicPlayPosition | AkCallbackType.AK_MusicSyncEntry |
                                     AkCallbackType.AK_EndOfEvent | AkCallbackType.AK_MusicSyncBeat |
                                     AkCallbackType.AK_MusicSyncBar | AkCallbackType.AK_SpeakerVolumeMatrix);

        // _musicPlayingId = music.Post(this.gameObject, callbackFlags, AkEventCallback);
        _musicPlayingId = WwiseController.Instance.PlayBGM("Play_shadow",
            (uint) callbackFlags, //  | 
            OnMusicEvent);
        /*_musicPlayingId = AkSoundEngine.PostEvent(music.Id, (UnityEngine.GameObject) gameObject,
            (uint) AkCallbackType.AK_EnableGetMusicPlayPosition, OnMusicEvent, null);*/
        Debug.Log($"Play,cur:{_musicPlayingId}");
        _wwiseManager.SetVolume(VolumeType.BGM, gameObject, 0.25f);
    }


    [Button("Stop")]
    public void Stop()
    {
        _wwiseManager.StopSound("Play_bgm", gameObject);
    }

    [Button("CheckPlaying")]
    public void CheckPlaying()
    {
        _wwiseManager.CheckEventIsPlaying(gameObject, _wwiseManager.GetMusicIDByName("BGM"));
    }

    [Button("SetValue")]
    public void SetVolume(float value)
    {
        _wwiseManager.SetVolume(VolumeType.BGM, gameObject, value);
    }

    // Update is called once per frame
    void Update()
    {
        if (_musicPlayingId != 0)
        {
            uint tempid = _wwiseManager.GetMusicIDByName("BGM");
            AkSegmentInfo segmentInfo = new AkSegmentInfo();
            var result = AkSoundEngine.GetPlayingSegmentInfo(_musicPlayingId, segmentInfo, true);
            musicTime = segmentInfo.iCurrentPosition;
            //Debug.Log(segmentInfo);
            if (result == AKRESULT.AK_Success)
            {
                //返回成功
                _currentPosition = (float) segmentInfo.iCurrentPosition / 1000f;
                //Debug.Log($"返回成功:{currentPosition}");
            }
            else
            {
                Debug.Log(result + "," + _musicPlayingId.ToString());
            }
        }
        else
        {
            Debug.Log(_musicPlayingId);
        }
        if (RemainWindowTime >=0)
        {
            if (lastHitBeatTime >0)
            {
                //CheckBeat();
            }
            RemainWindowTime -= Time.deltaTime;
        }

    }

    private void OnMusicEvent(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        //Debug.Log("Beat,OutSide");
        if (in_type is AkCallbackType.AK_MusicSyncBeat)
        {

        }
        else if (in_type is AkCallbackType.AK_MusicSyncBar)
        {

        }
        else if (in_type is AkCallbackType.AK_MusicSyncEntry)
        {

        }
        else if (in_type is AkCallbackType.AK_EndOfEvent)
        {

        }
    }

    void AkEventCallback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        switch (in_type)
        {
            case AkCallbackType.AK_MusicSyncEntry:
                AkMusicSyncCallbackInfo musicSyncCallbackInfo = in_info as AkMusicSyncCallbackInfo;
                Debug.Log("AkCallbackType.AK_MusicSyncEntry");
                break;
            case AkCallbackType.AK_EndOfEvent:
                AkEventCallbackInfo endInfo = in_info as AkEventCallbackInfo;
                Debug.Log("AkCallbackType.AK_EndOfEvent");
                break;
            case AkCallbackType.AK_EnableGetMusicPlayPosition:
                AkEventCallbackInfo Position = in_info as AkEventCallbackInfo;
                Debug.Log("AkCallbackType.AK_EnableGetMusicPlayPosition");
                break;
        }
    }

    public void SetLastBarTime()
    {
        lastHitBarTime = _currentPosition;
    }

    public void SetLastBeatTime()
    {
        lastHitBeatTime = _currentPosition;
    }

    public void CheckBeat()
    {
        if (lastHitBeatTime != 0)
        {
           
        }
    }
    public void CheckBar()
    {
    }
}