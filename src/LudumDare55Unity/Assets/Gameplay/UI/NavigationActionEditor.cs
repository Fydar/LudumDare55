using UnityEditor;

[CustomEditor(typeof(NavigationAction))]
public class NavigationActionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
	}
}
