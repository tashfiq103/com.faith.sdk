namespace com.faith.sdk
{
#if APSdk_LionKit
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    using LionStudios;


    public class FaithLionKitWrapper : MonoBehaviour
    {

        #region Public Variables
        public static FaithLionKitWrapper Instance { get; private set; }
        #endregion



        #region Private Variables
        private static bool _IsMaxMediationDebuggerRequested = false;
        #endregion



        #region Mono Behaviour
        private void Awake()
        {
            StartCoroutine(ShowMaxMediationDebugger());   
        }
        #endregion



        #region Configuretion

        private IEnumerator ShowMaxMediationDebugger() {

            yield return new WaitForSeconds(5f);

            if (Instance != this)
            {
                Destroy(gameObject);
            }
            else {

                if (!_IsMaxMediationDebuggerRequested)
                {
                    FaithSdkConfiguretionInfo apSdkConfiguretionInfo = Resources.Load<FaithSdkConfiguretionInfo>("APSdkConfiguretionInfo");

                    if (apSdkConfiguretionInfo.ShowMaxMediationDebugger)
                    {
#if APSdk_LionKit
                        MaxSdk.ShowMediationDebugger();
#endif
                        FaithSdkLogger.Log("Showing Mediation Debugger");
                    }

                    _IsMaxMediationDebuggerRequested = true;
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnGameStart()
        {
            if (Instance == null)
            {
                GameObject newAPLionKitWrapper = new GameObject("APLionKitWrapper");
                Instance = newAPLionKitWrapper.AddComponent<FaithLionKitWrapper>();

                DontDestroyOnLoad(newAPLionKitWrapper);

            }
        }


        #endregion



        #region Public Callback
        public static void LogLionGameEvent(string prefix, LionGameEvent gameEvent)
        {

            string logEvent = " [" + prefix + "]";
            logEvent += " [EventName] : " + gameEvent.eventName + "";
            logEvent += " [EventParams]\n";

            List<string> keyList = new List<string>();
            List<string> valueList = new List<string>();

            Dictionary<string, object>.KeyCollection keys = gameEvent.eventParams.Keys;
            Dictionary<string, object>.ValueCollection values = gameEvent.eventParams.Values;

            foreach (object key in keys)
            {
                keyList.Add(key.ToString());
            }

            foreach (object value in values)
            {
                valueList.Add(value.ToString());
            }


            int numberOfEventParams = keyList.Count;

            for (int i = 0; i < numberOfEventParams; i++)
            {
                logEvent += keyList[i] + " = " + valueList[i] + ((i != (numberOfEventParams - 1)) ? " __ " : "");
            }

            FaithSdkLogger.Log(logEvent);
        }

        #endregion
    }
#endif
}


