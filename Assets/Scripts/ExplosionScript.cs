using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private float scale;
    private GameObject sprite;

    // Start is called before the first frame update
    void Awake()
    {
        scale = 1.0f;
        StartCoroutine(shrink());
    }

    IEnumerator shrink()
    {
        while (scale > 0.1f)
        {
            scale -= 0.1f;
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
