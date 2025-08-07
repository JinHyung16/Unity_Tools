using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HughGame.HEditor
{
    public class MenuItemCollect : MonoBehaviour
    {
        public enum ECustomKey
        {
            None,
        }

        static Dictionary<ECustomKey, Action> _dict = new Dictionary<ECustomKey, Action>();

        public static void AddFunc(ECustomKey key, Action action)
        {
            _dict[key] = action;
        }

        public static void RemoveFunc(ECustomKey key)
        {
            _dict.Remove(key);
        }


        [MenuItem("Util/CanvasScalerChange")]
        static void CanvasScalerChangeEditor()
        {
            HughGame.HEditor.CanvasScalerChangeEditor.OpenWindow();
        }
    }
}
