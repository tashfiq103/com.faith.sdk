namespace com.faith.sdk
{
    using UnityEngine;

    public abstract class FaithBaseClassForConfiguretion : ScriptableObject
    {
        #region Public Variables

        public string NameOfConfiguretion { get { return _nameOfConfiguretion; } }

        #endregion

        #region Protected Variables

        [HideInInspector, SerializeField] protected bool _showSettings;
        [SerializeField] protected string _nameOfConfiguretion;
        [HideInInspector, SerializeField] protected bool _isSDKIntegrated;

        #endregion

        #region Protected Method

        /// <summary>
        /// Editor Only
        /// </summary>
        /// <param name="scriptDefineSymbol"></param>
        protected void SetNameOfConfiguretion(string scriptDefineSymbol,string concatinate = "")
        {

            string[] splited = scriptDefineSymbol.Split('_');
            _nameOfConfiguretion = splited[1] + concatinate;
        }

        #endregion

        #region Abstract Method

        public abstract void SetNameAndIntegrationStatus();

        public abstract void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, bool isATTEnable = false);

        /// <summary>
        /// You can write your editor script for the variables on your derived class before the template editor script
        /// </summary>
        public abstract void PreCustomEditorGUI();

        /// <summary>
        /// You can write your editor script for the variables on your derived class after the template editor script
        /// </summary>
        public abstract void PostCustomEditorGUI();

        #endregion
    }
}

