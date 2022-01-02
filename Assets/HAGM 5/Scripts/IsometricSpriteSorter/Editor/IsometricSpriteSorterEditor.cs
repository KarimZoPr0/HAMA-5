using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(IsometricSpriteSorter)), CanEditMultipleObjects]
public class IsometricSpriteSorterEditor: Editor
{

	private SerializedProperty	m_spritesProperty;
	private ReorderableList		m_spritesList;
	private SerializedProperty	m_sortingThresholdProperty;
	private SerializedProperty	m_pivotOffsetYProperty;

	private void OnEnable ()
	{
		m_spritesProperty = serializedObject.FindProperty("m_sprites");
		m_spritesList = new ReorderableList(this.serializedObject, m_spritesProperty, true, true, true, true);
		m_sortingThresholdProperty = serializedObject.FindProperty("m_sortingThreshold");
		m_pivotOffsetYProperty = serializedObject.FindProperty("m_pivotOffsetY");

		m_spritesList.drawHeaderCallback = this.OnDrawSpriteListHeader;
		m_spritesList.drawElementCallback = this.OnDrawSpriteListElement;
		m_spritesList.elementHeightCallback = this.OnGetSpriteListElementHeight;
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.UpdateIfRequiredOrScript();

		//if (EditorApplication.isPlaying)
			this.DisplayRuntimeInfos();

		GUILayout.Space(10);
		this.DisplaySortingProperties();
		GUILayout.Space(10);

		if (m_spritesProperty.isExpanded = EditorGUILayout.Foldout(m_spritesProperty.isExpanded, "Contolled sptites: " + m_spritesList.count + " elements"))
			m_spritesList.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
		if (GUILayout.Button("AddAllChildSprites"))
		{
			((IsometricSpriteSorter)target).AddAllSprites();
			EditorUtility.SetDirty(target);
		}
	}

	public override bool RequiresConstantRepaint ()
	{
		return (true);
	}

	private void DisplaySortingProperties ()
	{
		EditorGUILayout.PropertyField(m_pivotOffsetYProperty, new GUIContent("Pivot offset Y"));
		EditorGUILayout.PropertyField(m_sortingThresholdProperty, new GUIContent("SortingThreshold"));
	}

	private void DisplayRuntimeInfos ()
	{
		string msg = "Order : " + (target as IsometricSpriteSorter).YStep;

		EditorGUILayout.HelpBox(msg, MessageType.Info);
	}

	private void OnDrawSpriteListHeader ( Rect rect )
	{
		EditorGUI.LabelField(rect, "Sorted Sprites");
	}

	private float OnGetSpriteListElementHeight ( int idx )
	{
		SerializedProperty element = m_spritesList.serializedProperty.GetArrayElementAtIndex(idx);

		if (element.isExpanded)
			return (element.CountInProperty() * EditorGUIUtility.singleLineHeight);

		return (EditorGUIUtility.singleLineHeight);
	}

	private void OnDrawSpriteListElement ( Rect rect, int index, bool isActive, bool isFocused )
	{
		SerializedProperty	element = m_spritesList.serializedProperty.GetArrayElementAtIndex(index);
		SerializedProperty	renderer = element.FindPropertyRelative("renderer");
		SerializedProperty	orderInLayer = element.FindPropertyRelative("orderInLayer");

		rect.x += 15;
		rect.height = EditorGUIUtility.singleLineHeight;
		rect.width -= 15;

		element.isExpanded = EditorGUI.Foldout(rect, element.isExpanded, (renderer.objectReferenceValue != null ? renderer.objectReferenceValue.name : (null)));

		if (element.isExpanded)
		{
			EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField( new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width , EditorGUIUtility.singleLineHeight), renderer, new GUIContent("Sprite renderer"));
			if (EditorGUI.EndChangeCheck())
			{
				if (renderer.objectReferenceValue != null)
					orderInLayer.intValue = (renderer.objectReferenceValue as SpriteRenderer).sortingOrder;
			}

			EditorGUI.PropertyField( new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, rect.width, EditorGUIUtility.singleLineHeight), orderInLayer, new GUIContent("Order In Layer"));
		}
	}

}
