using UnityEngine;
using System.Collections;

public class Node {}

public class Street : Node {
	public Node[] nodes;

	public Street(params Node[] nodes) {
		this.nodes = nodes;
	}
}

public class Building : Node {
	public float groundSize = Mathf.Pow(Random.value, 2)*10;
	public float height = 1f + Mathf.Pow(Random.value, 2) * 50;
	public Vector3 color = new Vector3 (0.1f+Random.value*0.3f, 0.1f+Random.value*0.3f, 0.1f+Random.value*0.3f);
}

public class Block {
	public GameObject gameObject;
	public float width;
	public float length;

}