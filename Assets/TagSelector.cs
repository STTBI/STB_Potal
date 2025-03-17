using UnityEngine;
using UnityEditor;

public class TagSelector : MonoBehaviour
{
    [TagSelector] public string selectedTag;
}

#if UNITY_EDITOR


[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 태그 목록 가져오기
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

        // 현재 선택된 태그의 인덱스 찾기
        int selectedIndex = -1;
        for (int i = 0; i < tags.Length; i++)
        {
            if (tags[i] == property.stringValue)
            {
                selectedIndex = i;
                break;
            }
        }

        // 팝업 메뉴 표시
        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, tags);

        // 선택된 태그를 속성에 저장
        if (selectedIndex >= 0 && selectedIndex < tags.Length)
        {
            property.stringValue = tags[selectedIndex];
        }

        EditorGUI.EndProperty();
    }
}

public class TagSelectorAttribute : PropertyAttribute { }
#endif