#if APSdk_LionKit

namespace com.faith.sdk
{
#if UNITY_EDITOR

    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(FaithLionKitWrapper))]
    public class FaithLionKitWrapperEditor : Editor
    {
#region Private Variables

        private FaithLionKitWrapper _reference;

#endregion

#region Editor

        private void OnEnable()
        {
            _reference = (FaithLionKitWrapper)target;

            if (_reference == null)
                return;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            

            serializedObject.ApplyModifiedProperties();
        }

#endregion
    }
#endif
}

#endif


