using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace Frame.Core
{
    public enum LuancherState
    {
        None,
        Core,
        GameAssets,
        UIManager,
        SDK,
        Config,
        LoadScene,
        Done,
    }

    public class Luancher : MonoBehaviour
    {
        public static bool IsInitComplete;
        public static Action OnInitComplete;
        public static Luancher Instance;

        public UILuancher _loading;
        public static string LoadingBarText = "";

        private bool startLoadModules = false;

        private LuancherState _state;

        private LuancherState State
        {
            get { return _state; }
            set
            {
                if (value == _state)
                {
                    return;
                }

                _state = value;
            }
        }

        private LuancherState[] _states =
        {
            LuancherState.Core,
            LuancherState.GameAssets, LuancherState.UIManager, LuancherState.SDK, LuancherState.Config,
            LuancherState.LoadScene
        };

        async UniTask Start()
        {
#if UNITY_EDITOR
            Application.targetFrameRate = -1;
#endif

#if UNITY_IOS || UNITY_ANDROID
            Application.targetFrameRate = 60;
#endif
            Instance = this;
            //Debug.unityLogger.logEnabled = false;
            for (int i = 0; i < _states.Length; i++)
            {
                await LoadModule(_states[i]);
                await UniTask.Delay(1000);
            }
            //加载资源
        }

        async UniTask LoadModule(LuancherState state)
        {
            string text = "";
            switch (state)
            {
                case LuancherState.None:
                    break;
                case LuancherState.Core:
                    //加载底层模块
                    /*ThGold.Event.EventHandler _eventHandler =
                        new GameObject("EventHandler").AddComponent<ThGold.Event.EventHandler>();
                    DontDestroyOnLoad(_eventHandler);*/
                    break;
                case LuancherState.GameAssets:
                    _loading?.ChangeBar("[正在加载] 加载β世界线信息", 0f, 0.15f);
                    //await InitAsset();
                    // await UniTask.WaitUntil(() => startLoadModules);
                    break;
                case LuancherState.UIManager:
                    _loading?.ChangeBar("[正在加载] 加载α世界线信息", 0.15f, 0.25f);
                    await InitUIAsync();
                    break;
                case LuancherState.SDK:
                    _loading?.ChangeBar("[正在加载] 半拉伊埃斯信息", 0.25f, 0.5f);
                    //await InitSDK();
                    break;
                case LuancherState.Config:
                    _loading?.ChangeBar("[正在加载] 赫利俄斯研究所", 0.65f, 0.9f);
                    await InitConfig();
                    break;
                case LuancherState.LoadScene:
                    _loading?.ChangeBar("[正在加载] 赫利俄斯研究所旧址", 0.25f, 0.9f);
                    await InitScene();
                    break;
                case LuancherState.Done:
                    break;
                default:
                    break;
            }
        }


        public void InitMoudleAsync()
        {
            startLoadModules = true;
        }

        public async UniTask InitAsset()
        {
            /*loader = new YooAssetsLoader();
            loader.Initialized += InitMoudleAsync;
            await loader.InitAssets();*/
        }


        public async UniTask InitUIAsync()
        {
            /*UILoader uiloader = new UILoader();
            uiloader.load = loader.LoadAsync;
            uiloader.release = loader.Release;
            UIManager.Initialized += OnUIInitComplete;
            UIManager.Initialize(uiloader);*/
        }

        public void InitUI()
        {
        }

        void OnUIInitComplete()
        {
        }

        public async UniTask InitSDK()
        {
            Debug.Log("InitSDK");

            Debug.unityLogger.logEnabled = true;
            Debug.Log("SDK init success.");

            StartTiming();
            //});
        }

        private void StartTiming()
        {
        }

        private static long GetTimeStamp()
        {
            // 获取当前时间
            DateTime currentDateTime = DateTime.UtcNow;
            // Unix Epoch 时间
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            // 计算自 Unix Epoch 起的秒数
            long unixTimestamp = (long) (currentDateTime - unixEpoch).TotalSeconds;

            return unixTimestamp;
        }

        // 示例代码
        public void InitOSDK()
        {
            // TO DO OSDK
            /*
            PTNWXSDK.Instance.FetchArkConfig("AppTheme", ((success, configs) =>
            {
                if (success)
                {
                    foreach (var abData in configs)
                    {
                        foreach (var kv in abData.fields)
                        {
                            if (kv.fkey.Equals("AppTheme"))
                            {
                                // true：打开log、false：关闭log打印
                                OSDK.Debug(PTNWXConfig.IsDebug);
                                // 初始化SDK配置
                                OSDK.InitSDK(HttpUMK.Token, PTNWXConfig.AppKey,
                                    PTNWXConfig.AppSecret, PTNWXConfig.ServerUrl,
                                    PTNWXConfig.StatUrl);
                                //kv.fvalue是AppTheme配置字符串
                                OSDK.InitSplash(kv.fvalue);
                                return;
                            }
                        }
                    }
                }
            }));*/
        }

        public async Task InitConfig()
        {
            GameObject maindata = new GameObject("MainData");
            MainData _mainData = maindata.AddComponent<MainData>();
            DontDestroyOnLoad(maindata);
            await _mainData.InitConfig();
        }

        public async UniTask InitScene()
        {
            await CustomSceneManager.LoadSceneAsychonized("Combat",
                async (operation) =>
                {
                    if (operation.isDone && operation.progress >= 0.9f) // Unity场景加载progress最大到0.9
                    {
                        Scene loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
                        SceneManager.SetActiveScene(loadedScene);

                        Debug.Log("Scene loaded successfully");
                        IsInitComplete = true;
                        OnInitComplete?.Invoke();
                        OnInitComplete = null;
                    }
                    else
                    {
                        Debug.LogError("Scene loading progress: " + operation.progress);
                    }
                });
        }
    }
}