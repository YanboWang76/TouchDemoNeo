using UnityEngine;
using System.Collections;
using UnityEditor;
namespace CTool.Tween
{
	[CustomEditor(typeof(CTweenPath))]
	public class CTweenPathEditor :Editor {

		CTweenPath cTweenPath;
		bool addItem,delItem;

		void OnEnable() {
			cTweenPath = (CTweenPath)target;
			cTweenPath.parent = cTweenPath.transform.parent;
		}

		public override void OnInspectorGUI() {
			cTweenPath.pathVisible = EditorGUILayout.Toggle("Path Visible",cTweenPath.pathVisible);

			cTweenPath.nodeSize = EditorGUILayout.IntField("Node Size",cTweenPath.nodeSize);
			if (cTweenPath.nodeSize < 0) {
				cTweenPath.nodeSize = 0;
			}
			if (delItem) {
				cTweenPath.nodeSize -= 1;
				if (cTweenPath.nodeSize < 0) {
					cTweenPath.nodeSize = 0;
				}
				delItem = false;
			}
			if (addItem) {
				cTweenPath.nodeSize += 1;
				addItem = false;
			}

			if (cTweenPath.nodes.Count < cTweenPath.nodeSize) {
				int count = cTweenPath.nodeSize - cTweenPath.nodes.Count;
				for (int i = 0;i < count;i++) {
					GameObject go = new GameObject("point");
					go.transform.SetParent(cTweenPath.transform);
					Vector3 pos = cTweenPath.transform.position;
					if (cTweenPath.nodes.Count > 1) {
						pos = cTweenPath.nodes[cTweenPath.nodes.Count - 1].position + new Vector3(Random.Range(-0.5f,0.5f),0,Random.Range(-0.5f,0.5f));
					}
					go.transform.position = pos;
					cTweenPath.nodes.Add(go.transform);
				}
				for (int i = 0;i < cTweenPath.nodeSize;i++) {
					cTweenPath.nodes[i].name = "point_" + i;
				}
				Selection.activeGameObject = cTweenPath.nodes[cTweenPath.nodeSize - 1].gameObject;
			}
			if (cTweenPath.nodes.Count > cTweenPath.nodeSize) {
				int count = cTweenPath.nodes.Count - cTweenPath.nodeSize;
				for (int i = 0;i < count;i++) {
					int index = cTweenPath.nodes.Count - 1;
					if (index >= 0) {
						if (cTweenPath.nodes[index]) {
							DestroyImmediate(cTweenPath.nodes[index].gameObject);
						}
						cTweenPath.nodes.RemoveAt(index);
					}
				}
				for (int i = 0;i < cTweenPath.nodeSize;i++) {
					cTweenPath.nodes[i].name = "point_" + i;
				}
			}
			//for (int i = 0;i < cTweenPath.nodeSize;i++) {
			//    if (GUILayout.Button(cTweenPath.nodes[i].name)) {

			//    }
			//}

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("-")) {
				delItem = true;
			}
			if (GUILayout.Button("+")) {
				addItem = true;
			}
			EditorGUILayout.EndHorizontal();
			if (GUI.changed) {
				EditorUtility.SetDirty(target);
			}
		}
	}

}
