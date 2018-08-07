using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CTool.Tween
{
	public class CTweenPath :MonoBehaviour {

		public bool pathVisible = true;
		public int nodeSize;
		public List<Transform> nodes = new List<Transform>();
		public Transform parent { set; get; }
		int currNodeIndex;
		bool canMove;
		float lastDis;

		void OnDrawGizmos() {
			if (pathVisible) {
				if (nodeSize > 0) {
					Gizmos.color = Color.cyan;
					for (int i = 0;i < nodeSize - 1;i++) {
						Gizmos.DrawLine(nodes[i].position,nodes[i + 1].position);
					}
				}
			}
		}

	}
}