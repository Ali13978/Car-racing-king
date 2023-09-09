using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AliScripts
{
    public class AliExtras : MonoBehaviour
    {
        public static void DestroyChildren(GameObject targetObject)
        {
            int childCount = targetObject.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = targetObject.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        public static bool GetBoolFromString(string inputString)
        {
            try
            {
                bool parsedBool = bool.Parse(inputString);
                Debug.Log("Parsed bool using bool.Parse(): " + parsedBool);

                return parsedBool;
            }
            catch (System.Exception)
            {
                Debug.LogError("Failed to parse using bool.Parse()");
                return false;
            }
        }
    }
}