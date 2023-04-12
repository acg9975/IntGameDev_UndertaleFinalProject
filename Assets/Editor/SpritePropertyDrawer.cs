using UnityEngine;
using UnityEditor;

namespace IronCore.Editor
{
    [CustomPropertyDrawer(typeof(NPCDialogue.DialogueItem))]
    public class SpritePropertyDrawer : PropertyDrawer
    {
        private const float imageHeight = 70;

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return imageHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            var sprite = prop.FindPropertyRelative("sprite");
            var text = prop.FindPropertyRelative("text");

            EditorGUI.BeginProperty(position, label, prop);

            position.width = imageHeight;
            position.height = imageHeight;

            sprite.objectReferenceValue = EditorGUI.ObjectField(position, sprite.objectReferenceValue, typeof(Sprite), false);

            position.x += imageHeight + 10;
            position.width = EditorGUIUtility.currentViewWidth - 155;

            text.stringValue = EditorGUI.TextArea(position, text.stringValue, new GUIStyle(EditorStyles.textArea));

            EditorGUI.EndProperty();
        }
    }
}