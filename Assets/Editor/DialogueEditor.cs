using UnityEngine;
using UnityEditor;
using static Dialogue;
using static Dialogue.DialogueItem;

namespace DialogueEditor
{
    [CustomPropertyDrawer(typeof(DialogueItem))]
    public class DialogueItemEditor : PropertyDrawer
    {
        private float SINGLE_LINE_HEIGHT = EditorGUIUtility.singleLineHeight;
        private float VERTICAL_SPACING = EditorGUIUtility.standardVerticalSpacing;
        private float LABEL_WIDTH = EditorGUIUtility.labelWidth;

        private const float IMAGE_HEIGHT = 70;
        private const float VERTICAL_SPACING_MED = 6;

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            var conditions = prop.FindPropertyRelative("conditions");
            var type = prop.FindPropertyRelative("type");
            var setValues = prop.FindPropertyRelative("setValues");
            var onEnd = prop.FindPropertyRelative("onEnd");
            var choices = prop.FindPropertyRelative("choices");

            float extraHeight = 0;

            switch ((DialogueItemType)type.intValue)
            {
                case DialogueItemType.Standard:
                    extraHeight += VERTICAL_SPACING + IMAGE_HEIGHT;
                    extraHeight += VERTICAL_SPACING_MED + EditorGUI.GetPropertyHeight(setValues);
                    extraHeight += VERTICAL_SPACING_MED + EditorGUI.GetPropertyHeight(onEnd);
                    break;
                case DialogueItemType.Decision:
                    extraHeight += VERTICAL_SPACING_MED + EditorGUI.GetPropertyHeight(choices);
                    break;
            }

            return
                VERTICAL_SPACING + EditorGUI.GetPropertyHeight(conditions)
                + VERTICAL_SPACING + EditorGUI.GetPropertyHeight(type)
                + extraHeight
                + VERTICAL_SPACING;
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            Rect originalPosition = position;
            string index = label.text.Substring(label.text.LastIndexOf(' ') + 1);

            var conditions = prop.FindPropertyRelative("conditions");
            var type = prop.FindPropertyRelative("type");
            var sprite = prop.FindPropertyRelative("sprite");
            var text = prop.FindPropertyRelative("text");
            var setValues = prop.FindPropertyRelative("setValues");
            var onEnd = prop.FindPropertyRelative("onEnd");
            var choices = prop.FindPropertyRelative("choices");

            EditorGUI.BeginProperty(position, label, prop);

            // Conditions

            position.x += 14;
            position.y += VERTICAL_SPACING;
            position.width -= 10;

            EditorGUI.PropertyField(position, conditions);

            position.y += EditorGUI.GetPropertyHeight(conditions, true);

            // Type

            position.x = originalPosition.x;
            position.y += VERTICAL_SPACING;
            position.width = originalPosition.width;
            position.height = SINGLE_LINE_HEIGHT;

            type.intValue = (int)(DialogueItemType)EditorGUI.EnumPopup(position, type.displayName, (DialogueItemType)type.intValue);

            position.y += SINGLE_LINE_HEIGHT;

            // Element Label

            Rect indexPosition = position;

            indexPosition.x -= 22;
            indexPosition.y = originalPosition.y + VERTICAL_SPACING * 2 + SINGLE_LINE_HEIGHT;
            indexPosition.width = LABEL_WIDTH;
            indexPosition.height = SINGLE_LINE_HEIGHT;

            EditorGUI.LabelField(indexPosition, index, new GUIStyle(EditorStyles.boldLabel));

            // if type == Standard, show Sprite, Text, SetValues, OnEnd
            // if type == Decision, show Choices

            position.x = originalPosition.x;
            position.width = originalPosition.width;
            position.height = originalPosition.height;

            switch ((DialogueItemType) type.intValue)
            {
                case DialogueItemType.Standard:

                    // Sprite and Text

                    position.y += VERTICAL_SPACING;
                    position.width = IMAGE_HEIGHT;
                    position.height = IMAGE_HEIGHT;

                    sprite.objectReferenceValue = EditorGUI.ObjectField(position, sprite.objectReferenceValue, typeof(Sprite), false);

                    position.x += IMAGE_HEIGHT + 10;
                    position.width = originalPosition.width - IMAGE_HEIGHT - 10;

                    text.stringValue = EditorGUI.TextArea(position, text.stringValue, new GUIStyle(EditorStyles.textArea));

                    position.y += IMAGE_HEIGHT;

                    // setValues

                    position.x = originalPosition.x + 14;
                    position.y += VERTICAL_SPACING_MED;
                    position.width = originalPosition.width - 10;
                    position.height = originalPosition.height;

                    EditorGUI.PropertyField(position, setValues, true);

                    position.y += EditorGUI.GetPropertyHeight(setValues);

                    // onEnd

                    position.x = originalPosition.x;
                    position.y += VERTICAL_SPACING_MED;
                    position.width = originalPosition.width;

                    EditorGUI.PropertyField(position, onEnd);

                    break;

                case DialogueItemType.Decision:

                    // Choices

                    position.y += VERTICAL_SPACING_MED;

                    EditorGUI.PropertyField(position, choices, true);

                    break;
            }

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(Preference))]
    public class PrefrenceEditor : PropertyDrawer
    {
        private float SINGLE_LINE_HEIGHT = EditorGUIUtility.singleLineHeight;
        private float VERTICAL_SPACING = EditorGUIUtility.standardVerticalSpacing;

        private const float SIGN_SPACING = 20f;

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return
                VERTICAL_SPACING + SINGLE_LINE_HEIGHT
                + VERTICAL_SPACING + SINGLE_LINE_HEIGHT
                + VERTICAL_SPACING;
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            Rect originalPosition = position;
            float fieldWidth = (originalPosition.width - SIGN_SPACING) / 2f;

            var parameter = prop.FindPropertyRelative("parameter");
            var value = prop.FindPropertyRelative("value");

            EditorGUI.BeginProperty(position, label, prop);

            // Labels

            position.y += VERTICAL_SPACING;
            position.width = fieldWidth;
            position.height = SINGLE_LINE_HEIGHT;

            EditorGUI.LabelField(position, "Parameter");

            position.x += fieldWidth + SIGN_SPACING;

            EditorGUI.LabelField(position, "Value");

            position.y += SINGLE_LINE_HEIGHT;

            // Fields

            position.x = originalPosition.x;
            position.y += VERTICAL_SPACING;

            parameter.stringValue = EditorGUI.TextField(position, parameter.stringValue);

            position.x += fieldWidth;
            position.width = SIGN_SPACING;

            GUIStyle centeredStyle = new GUIStyle(EditorStyles.label);
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            EditorGUI.LabelField(position, "=", centeredStyle);

            position.x += SIGN_SPACING;
            position.width = fieldWidth;

            value.stringValue = EditorGUI.TextArea(position, value.stringValue);

            EditorGUI.EndProperty();
        }
    }
}