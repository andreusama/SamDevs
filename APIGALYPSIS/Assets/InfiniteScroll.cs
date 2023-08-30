using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{

    private RawImage image;

    public float speed;

    public Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        image = this.transform.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        //modify the UVrect x and y value to move the texture taking care of the speed and direction
        image.uvRect = new Rect(image.uvRect.x + direction.x * speed * Time.deltaTime, image.uvRect.y + direction.y * speed * Time.deltaTime, image.uvRect.width, image.uvRect.height);
    }
}
