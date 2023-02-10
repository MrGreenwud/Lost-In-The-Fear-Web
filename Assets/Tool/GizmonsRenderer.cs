using System;
using UnityEditor;
using UnityEngine;

namespace BCTSTool.DubugGizmos
{
    [ExecuteInEditMode]
    public class GizmonsRenderer : MonoBehaviour
    {
        public Action OnDrowGizmos;
        public Action OnDrowGizmosSelected;
        public Action OnGUIAction;

        private void OnGUI()
        {
            OnGUIAction?.Invoke();
        }

        private void OnDrawGizmos()
        {
            OnDrowGizmos?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            OnDrowGizmosSelected?.Invoke();
        }
    }
}
