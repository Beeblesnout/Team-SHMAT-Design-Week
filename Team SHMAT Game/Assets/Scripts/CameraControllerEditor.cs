// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(CameraController))]
// [CanEditMultipleObjects]
// public class CameraControllerEditor : Editor {
//     SerializedProperty position, rotation;
//     SerializedProperty lockPosX, lockPosY, lockPosZ;
//     SerializedProperty lockRotX, lockRotY;

//     void OnEnable()
//     {
//         position = serializedObject.FindProperty("position");
//         rotation = serializedObject.FindProperty("rotation");
//         lockPosY = serializedObject.FindProperty("lockPosX");
//         lockPosX = serializedObject.FindProperty("lockPosY");
//         lockPosZ = serializedObject.FindProperty("lockPosZ");
//         lockRotX = serializedObject.FindProperty("lockRotX");
//         lockRotY = serializedObject.FindProperty("lockRotY");
//     }

//     public override void OnInspectorGUI() {
//         // base.OnInspectorGUI();
//         serializedObject.Update();

//         EditorGUILayout.BeginVertical();
//             EditorGUILayout.BeginHorizontal();
//                 EditorGUILayout.BeginVertical(GUILayout.MaxWidth(100));
//                     EditorGUILayout.LabelField("Position");
//                     EditorGUILayout.LabelField("Constraints");
//                     EditorGUILayout.LabelField("Rotation");
//                     EditorGUILayout.LabelField("Constraints");
//                 EditorGUILayout.EndVertical();
//                 EditorGUILayout.BeginVertical();
//                     position.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, position.vector3Value);
//                     EditorGUILayout.BeginHorizontal();
//                         lockPosX.boolValue = EditorGUILayout.Toggle(lockPosX.boolValue);
//                         lockPosY.boolValue = EditorGUILayout.Toggle(lockPosY.boolValue);
//                         lockPosZ.boolValue = EditorGUILayout.Toggle(lockPosZ.boolValue);
//                     EditorGUILayout.EndHorizontal();
//                     rotation.vector2Value = EditorGUILayout.Vector2Field(GUIContent.none, rotation.vector2Value);
//                     EditorGUILayout.BeginHorizontal();
//                         lockRotX.boolValue = EditorGUILayout.Toggle(lockRotX.boolValue);
//                         lockRotY.boolValue = EditorGUILayout.Toggle(lockRotY.boolValue);
//                     EditorGUILayout.EndHorizontal();
//                 EditorGUILayout.EndVertical();
//             EditorGUILayout.EndHorizontal();
//         EditorGUILayout.EndVertical();

//         serializedObject.ApplyModifiedProperties();
//     }
// }