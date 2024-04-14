using UnityEngine.UI;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utility
{
    [RequireComponent(typeof(CanvasRenderer))]
	public class RaycastTarget : Graphic
	{
		public override bool raycastTarget
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public override void SetMaterialDirty()
		{
			return;
		}

		public override void SetVerticesDirty()
		{
			return;
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			return;
		}

	#if UNITY_EDITOR
		[CanEditMultipleObjects, CustomEditor(typeof(RaycastTarget), false)]
		private class RaycastTargetEditor : Editor
		{
			public override void OnInspectorGUI()
			{
				serializedObject.Update();

				serializedObject.ApplyModifiedProperties();
			}
		}
	#endif
	}
}
