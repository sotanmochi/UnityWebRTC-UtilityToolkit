using UnityEngine;

namespace Unity.WebRTC.UtilityToolkit
{
    public sealed class RtcEngine : MonoBehaviour
    {
        [SerializeField] private bool _autoInitializeOnAwake = false;

        public EncoderType EncoderType = EncoderType.Hardware;
        public bool LimitTextureSize = true;

        private bool _initialized;
        private bool _disposed;

#region Singleton class handling

        private static RtcEngine _instance;

        public static RtcEngine Instance
        {
            get 
            {
                if (_instance == null)
                {
                    var previous = FindObjectOfType<RtcEngine>();
                    if (previous)
                    {
                        _instance = previous;
                        DebugLogger.LogWarning("[RtcEngine] RtcEngine has been initialized twice."
                                            + $"The instance attached on \"{previous.gameObject.name}\" is used.");
                    }
                    else
                    {
                        var go = new GameObject("__RtcEngine");
                        _instance = go.AddComponent<RtcEngine>();
                        // DontDestroyOnLoad(go);
                        // go.hideFlags = HideFlags.HideInHierarchy;
                    }
                }
                return _instance;
            }
        }

#endregion

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (_autoInitializeOnAwake)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Initialize()
        {
            if (_initialized){ return; }

            DebugLogger.Log("[RtcEngine] Initialize()");

            WebRTC.Initialize(EncoderType, LimitTextureSize);
            StartCoroutine(WebRTC.Update());

            _initialized = true;
            _disposed = false;
        }

        public void Dispose()
        {
            if (_disposed){ return; }

            DebugLogger.Log("[RtcEngine] Dispose()");

            WebRTC.Dispose();

            _initialized = false;
            _disposed = true;
        }
    }
}
