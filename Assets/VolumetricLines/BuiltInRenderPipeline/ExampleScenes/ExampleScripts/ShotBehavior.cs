using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * 20;
	
	}
}
