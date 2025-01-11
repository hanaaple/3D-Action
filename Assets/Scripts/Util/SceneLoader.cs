using System;
using System.Collections;
using Data;
using Manager;
using Save;
using SceneManagement.Scriptable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Util
{
    public enum SceneLoadType
    {
        NewGame,
        LoadGame,
        Continue,
        MapChange,
        Title,
        GameEnd
    }
    
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;

        public static SceneLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = FindObjectOfType<SceneLoader>();
                    if (obj != null)
                    {
                        _instance = obj;
                    }
                    else
                    {
                        _instance = Create();
                    }

                    _instance.gameObject.SetActive(false);
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        [SerializeField] private CanvasGroup sceneLoaderCanvasGroup;
        [SerializeField] private Image progressBar;
        [SerializeField] private float fadeSec;

        private string _loadSceneName;

        private Action _onLoadSceneBegin;
        private Action _onLoadSceneEnd;

        private static SceneLoader Create()
        {
            var sceneLoaderPrefab = Resources.Load<SceneLoader>("Singleton/SceneLoader");
            return Instantiate(sceneLoaderPrefab);
        }
        
        /// <summary>
        /// Teleport to Default Spawn Position
        /// </summary>
        public void LoadScene(string targetSceneName, SceneLoadType sceneLoadType)
        {
            // Empty가 나쁜건 아닌데 기분이 나쁨.
            LoadScene(targetSceneName, string.Empty, sceneLoadType);
        }
        
        public void LoadScene(SpawnPointData spawnPointData, SceneLoadType sceneLoadType)
        {
            LoadScene(spawnPointData.sceneName, spawnPointData.id, sceneLoadType);
        }

        private void LoadScene(string targetSceneName, string targetId, SceneLoadType sceneLoadType)
        {
            AddOnLoadScene(sceneLoadType, targetId);
            
            _onLoadSceneBegin?.Invoke();
            _onLoadSceneBegin = () => { };
            Debug.Log($"Load Scene - {targetSceneName}, {(string.IsNullOrEmpty(targetId) ? "Default" : targetId)}");
            
            gameObject.SetActive(true);
            SceneManager.sceneLoaded += LoadSceneEnd;
            _loadSceneName = targetSceneName;
            StartCoroutine(Load(targetSceneName));
        }

        private IEnumerator Load(string targetSceneName)
        {
            progressBar.fillAmount = 0f;
            yield return StartCoroutine(Fade(true));

            var op = SceneManager.LoadSceneAsync(targetSceneName);
            op.allowSceneActivation = false;

            var timer = 0f;
            const float timeInterval = 0.02f;

            while (!op.isDone)
            {
                yield return YieldInstructionProvider.WaitForSecondsRealtime(timeInterval);
                timer += timeInterval;

                if (op.progress < 0.9f)
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                    if (progressBar.fillAmount >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                    if (!Mathf.Approximately(progressBar.fillAmount, 1f)
                        // || SaveManager.IsWorking()
                        )
                    {
                        continue;
                    }
                    
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

        private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name != _loadSceneName)
            {
                return;
            }

            Debug.Log("OnLoadSceneEnd");

            _onLoadSceneEnd?.Invoke();
            _onLoadSceneEnd = () => { };
            SceneManager.sceneLoaded -= LoadSceneEnd;

            StartCoroutine(Fade(false));
        }

        // Scene Loading의 목적에 따라 크게 바뀌는 것이 별로 없기에 따로 클래스를 생성하지 않음.
        // 다만, 앞으로 있을 변경에 대응하기 위해 함수로 캡슐화
        
        private void AddOnLoadScene(SceneLoadType sceneLoadType, string portalId)
        {
            switch (sceneLoadType)
            {
                case SceneLoadType.NewGame:
                    _onLoadSceneEnd += () =>
                    {
                        // Error 장착하고 다른 슬롯에 장착하면 기존 아이템 해제안되고 2곳에 장착되었음
                        // 플레이어 생성 및 로드에 관련하여    Default 스탯에 관련된 데이터
                        
                        PlayDataManager.instance.IsLoad = false;
                        PlaySceneManager.instance.InstantiatePlayer();
                        PlaySceneManager.instance.Teleport(portalId);
                    };
                    break;
                case SceneLoadType.LoadGame: // LoadGame - List (Index)가 추가되면 추가 구현.
                case SceneLoadType.Continue:
                    _onLoadSceneBegin += () =>
                    {
                        PlayDataManager.instance.IsLoad = true;
                        SaveManager.Load();
                    };
                    _onLoadSceneEnd += () =>
                    {
                        Debug.LogWarning("OnLoadSceneEnd");
                        // Npc의 위치에 따라 매번 체크하여 Scene에 생성하던가 해야될 것이며
                        
                        PlaySceneManager.instance.InstantiatePlayer();
                        // 자동으로 Load? 아니면
                        PlaySceneManager.instance.Teleport(portalId);
                        

                        // NpcList.Foreach => if(npc.isInScene(sceneName)) npc.Instance();

                        // Quest? -> Quest는 여러 Npc와 엮일 수 있으므로 Npc에 묶이면 안된다.
                        // QuestManager.What?
                    };
                    break;
                case SceneLoadType.MapChange: // 현재 상태 Save
                    PlayDataManager.instance.Save(false);
                    _onLoadSceneEnd += () =>
                    {
                        PlaySceneManager.instance.Teleport(portalId);
                        
                        // Npc 삭제 or 생성
                        // 몬스터 Save&Load
                        // Quest? 퀘스트는 뭐 있나
                    };
                    break;
                case SceneLoadType.Title:
                case SceneLoadType.GameEnd:
                    PlayDataManager.instance.Save();
                    PlaySceneManager.instance.Release();
                    break;
            }
        }

        private IEnumerator Fade(bool isFadeIn, Action onEndAction = default)
        {
            var timer = 0f;
            const float timeInterval = 0.02f;

            while (timer <= 1f)
            {
                yield return YieldInstructionProvider.WaitForSecondsRealtime(timeInterval);
                timer += timeInterval / fadeSec;
                sceneLoaderCanvasGroup.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, timer);
            }

            if (!isFadeIn)
            {
                gameObject.SetActive(false);
            }

            onEndAction?.Invoke();
        }
    }
}