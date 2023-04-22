using UnityEngine;
using UnityEditor;
using static Dialogue;
using static Dialogue.DialogueItem;

public class NPCDialogueEditor
{
    [CustomPropertyDrawer(typeof(DialogueItem))]
    public class DialogueItemEditor : PropertyDrawer
    {
        private float SINGLE_LINE_HEIGHT = EditorGUIUtility.singleLineHeight;
        private float VERTICAL_SPACING = EditorGUIUtility.standardVerticalSpacing;
        private float LABEL_WIDTH = EditorGUIUtility.labelWidth;

        private const float IMAGE_HEIGHT = 70;
        private const float TRIGGER_SPACING = 6;

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            var trigger = prop.FindPropertyRelative("trigger");
            var choices = prop.FindPropertyRelative("choices");
            var onEnd = prop.FindPropertyRelative("onEnd");

            float triggerHeight = EditorGUI.GetPropertyHeight(trigger);

            switch ((TriggerType)trigger.intValue)
            {
                case TriggerType.Choice:
                    triggerHeight += VERTICAL_SPACING + EditorGUI.GetPropertyHeight(choices);
                    break;
                case TriggerType.Custom:
                    triggerHeight += TRIGGER_SPACING + EditorGUI.GetPropertyHeight(onEnd);
                    break;
            }

            return
                VERTICAL_SPACING + IMAGE_HEIGHT
                + VERTICAL_SPACING + triggerHeight
                + VERTICAL_SPACING;
        }

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            Rect originalPosition = position;

            var sprite = prop.FindPropertyRelative("sprite");
            var text = prop.FindPropertyRelative("text");
            var trigger = prop.FindPropertyRelative("trigger");
            var choices = prop.FindPropertyRelative("choices");
            var onEnd = prop.FindPropertyRelative("onEnd");

            EditorGUI.BeginProperty(position, label, prop);

            // Element Label

            position.x -= 22;
            position.y += VERTICAL_SPACING + (IMAGE_HEIGHT - SINGLE_LINE_HEIGHT) / 2f;
            position.width = LABEL_WIDTH;
            position.height = SINGLE_LINE_HEIGHT;

            string labelString = label.ToString();
            string elementIndex = labelString.Substring(labelString.LastIndexOf(' ') + 1);
            EditorGUI.LabelField(position, elementIndex, new GUIStyle(EditorStyles.boldLabel));

            // Sprite and Text

            position = originalPosition;
            position.y += VERTICAL_SPACING;
            position.width = IMAGE_HEIGHT;
            position.height = IMAGE_HEIGHT;

            sprite.objectReferenceValue = EditorGUI.ObjectField(position, sprite.objectReferenceValue, typeof(Sprite), false);

            position.x += IMAGE_HEIGHT + 10;
            position.width = originalPosition.width - IMAGE_HEIGHT - 10;

            text.stringValue = EditorGUI.TextArea(position, text.stringValue, new GUIStyle(EditorStyles.textArea));

            position.y += IMAGE_HEIGHT;

            // Trigger Dropdown

            position.x = originalPosition.x;
            position.y += VERTICAL_SPACING;
            position.width = originalPosition.width;
            position.height = SINGLE_LINE_HEIGHT;

            trigger.intValue = (int)(TriggerType)EditorGUI.EnumPopup(position, trigger.displayName, (TriggerType)trigger.intValue);

            position.y += SINGLE_LINE_HEIGHT;

            // Trigger options

            switch ((TriggerType)trigger.intValue)
            {
                case TriggerType.Choice:

                    position.y += VERTICAL_SPACING;
                    EditorGUI.PropertyField(position, choices);
                    break;

                case TriggerType.Custom:

                    position.y += TRIGGER_SPACING;
                    EditorGUI.PropertyField(position, onEnd);
                    break;
            }

            EditorGUI.EndProperty();
        }
    }
}