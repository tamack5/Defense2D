using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

    [SerializeField]
    float scrollSpeedScale = 10f;

	// Update is called once per frame
	void Update () {

        // Get material, offset the main texture by the current position 
        // (if attached to an object, scrolls infinitely)
        Material mat = GetComponent<MeshRenderer>().material;

        Vector2 offset = mat.mainTextureOffset;

        offset.x = transform.position.x / scrollSpeedScale;
        offset.y = transform.position.y / scrollSpeedScale;

        mat.mainTextureOffset = offset;
	}
}
