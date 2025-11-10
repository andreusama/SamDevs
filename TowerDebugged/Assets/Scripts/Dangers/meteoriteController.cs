using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteoriteController : MonoBehaviour {

	private GameObject target;
	public float speed = 10;
	public string targetTag = "Castle";
	public float damage = 10;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag(targetTag);
	}
	
	// Update is called once per frame
	void Update () {

		// Ens anem movent cap al castell
		/*float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

		// Quan xoca, destrueix
		if(Vector3.Distance(transform.position, target.transform.position) < 0.1f)
		{
			// Falta fer mal
			GameObject.Destroy(this.gameObject);
		}*/

	}
}
