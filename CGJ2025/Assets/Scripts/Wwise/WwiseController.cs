using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using ThGold.Common;
using ThGold.Event;
using EventHandler = ThGold.Event.EventHandler;

namespace ThGold.Wwise {
    public class WwiseController : Common.Singleton<WwiseController> {
        private WwiseManager _WwiseManager;

        //Main volume
        public float GlobalMainSoundValue = 1.0f;
        public bool canMainSound;
        public GameObject startVoiceobj;
        public int IsMainSound => SetIsSound(canMainSound);

        //Sound
        public float GlobalSoundValue = 1.0f;
        public bool canSound = true;

        public int IsSound => SetIsSound(canSound);

        //BGMSound
        public GameObject BGMObject;
        public GameObject EffectObject;
        public float GlobalBGMSoundValue = 1.0f;
        public bool canBGM = true;
        public int IsBGM => SetIsSound(canBGM);


        protected override void Awake() {
            base.Awake();
            _WwiseManager = transform.GetComponent<WwiseManager>();
        }

        private void Start() {
            InitBGM();
        }

        public void ChangeLevelState(string StateName) {
            SwtichState("playing_state", StateName);
        }

        public void ChangeGame_state(string StateName) {
            SwtichState("game_state", StateName);
        }

        private void InitRTPC() {
            AkSoundEngine.SetRTPCValue("health", CalculationSoundVolume(VolumeType.Sound));
            AKRESULT aKRESULT = AkSoundEngine.SetRTPCValue("distance", CalculationSoundVolume(VolumeType.Sound));
            Debug.Log("res:" + aKRESULT);
        }

        private void OnDisable() {
        }

        public void InitBGM() {
            BGMObject = new GameObject("BGMObj");
            BGMObject.AddComponent<AkGameObj>();
            BGMObject.AddComponent<AkAudioListener>();
            BGMObject.transform.parent = this.gameObject.transform;
            EffectObject = new GameObject("EffectObject");
            EffectObject.AddComponent<AkGameObj>();
            EffectObject.transform.parent = this.gameObject.transform;
            ChangeBGMListener(BGMObject);
            ChangeSoundListener(EffectObject);
            //ChangeLevelState("normal");
            //UpdatePlayerState(false);
            //PlayBGM("BGM");
        }

        public void SetRTPC(string RTPCName, float RTPCValue) {
            //Debug.Log($"RTPCName:{RTPCName},{RTPCValue}");
            _WwiseManager.SetRTPC(RTPCName, RTPCValue);
        }

        public void ChangeBGMListener(GameObject gameObject) {
            _WwiseManager.BGMListener = gameObject;
        }

        public void ChangeSoundListener(GameObject gameObject) {
            _WwiseManager.listener = gameObject;
        }

        public uint PlayBGM(string EventName, uint callbackType = 1, AkCallbackManager.EventCallback Callback = null) {
            return _WwiseManager.PlaySoundBGM(EventName, BGMObject, callbackType, Callback);
        }
        public uint PlayBGM(uint EventName, uint callbackType = 1, AkCallbackManager.EventCallback Callback = null) {
            return _WwiseManager.PlaySoundBGM(EventName, BGMObject, callbackType, Callback);
        }
        public void PlayEffect(string Name) {
            _WwiseManager.PlaySound(Name, EffectObject);
        }

        /// <summary>
        /// 播放Event
        /// </summary>
        /// <param name="EventName"></param>
        /// <param name="gameObject"></param>
        public void PlayEvent(string EventName, GameObject gameObject) {
            Debug.Log("Play" + EventName);
            _WwiseManager.PlaySound(EventName, gameObject);
        }

        /// <summary>
        /// 停止Event
        /// </summary>
        /// <param name="EventName"></param>
        /// <param name="gameObject"></param>
        public void StopSound(string EventName, GameObject gameObject) {
            _WwiseManager.StopSound(EventName, gameObject);
        }

        public void StopBGM(string EventName)
        {
            _WwiseManager.StopSound(EventName, BGMObject);
        }
        public void SwtichState(string str1, string str2) {
            Debug.Log("State:" + str2);
            _WwiseManager.SwtichState(str1, str2);
        }

        public void StopAllSound() {
            _WwiseManager.StopAll();
        }

        public void MuteAll() {
            _WwiseManager.isPlay = false;
        }

        public void OpenAll() {
            _WwiseManager.isPlay = true;
        }

        public void SetSwitch(string SwitchGroup, string SwitchType, GameObject gameObject) {
            // 获取Wwise音频引擎实例
            Debug.Log($"Switch:{SwitchGroup},{SwitchType}");
            AkSoundEngine.SetSwitch(SwitchGroup, SwitchType, gameObject);
        }

        [Button("Play")]
        public void Test() {
            PlayEvent(WwiseEventName.Play_Test, BGMObject);
        }

        /// <summary>
        /// 这个计算方法暂时不知道能不能用
        /// </summary>
        /// <param name="StrRPTC"></param>
        private void SetSoundVolume(string StrRPTC) {
            AkSoundEngine.SetRTPCValue(StrRPTC, CalculationSoundVolume(VolumeType.Sound));
        }

        #region privateMethod

        private int SetIsSound(bool can) {
            if (can) {
                return 1;
            }
            else {
                return 0;
            }
        }


        private float CalculationSoundVolume(VolumeType type) {
            if (canMainSound) {
                if (type == VolumeType.Sound) {
                    Debug.Log("RTPCvalue:" + IsMainSound * IsSound * GlobalMainSoundValue * GlobalSoundValue);
                    return IsMainSound * IsSound * GlobalMainSoundValue * GlobalSoundValue;
                }

                if (type == VolumeType.BGM) {
                    Debug.Log("RTPCvalue:" + IsMainSound * IsSound * GlobalMainSoundValue * GlobalSoundValue);
                    return IsMainSound * IsBGM * GlobalMainSoundValue * GlobalBGMSoundValue;
                }
            }
            else {
                return 0;
            }

            return 0;
        }

        #endregion
    }

    public enum VolumeType {
        Sound,
        BGM
    }
}