using UnityEditor;
using UnityEngine;

public class CenterPivotTool : EditorWindow
{
    private GameObject targetObject;

    [MenuItem("Tools/Center Pivot Tool")]
    public static void ShowWindow()
    {
        GetWindow<CenterPivotTool>("Center Pivot Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Center Pivot Tool", EditorStyles.boldLabel);

        targetObject = EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Center Pivot") && targetObject != null)
        {
            CenterPivot(targetObject);
        }
    }

    private void CenterPivot(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.LogError("Target object is null.");
            return;
        }

        // Check if the object has a MeshRenderer component
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError("Target object must have a MeshRenderer component.");
            return;
        }

        // Calculate the center of the mesh bounds
        Vector3 center = meshRenderer.bounds.center;

        // Create an empty GameObject as the parent
        GameObject refObject = new GameObject(gameObject.name + "_Centered");
        GameObject parentObject =  Instantiate(refObject, center, Quaternion.identity, gameObject.transform.parent);
        parentObject.transform.position = center;

        // Parent the original object to the new parent
        Transform originalTransform = gameObject.transform;
        originalTransform.SetParent(parentObject.transform, true); // Preserve local position

        // Mark the original object as dirty so that changes are saved
        EditorUtility.SetDirty(gameObject);
    }
}
