using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AssignImageToModel : MonoBehaviour
{
    public Texture textureToApply;
    public GameObject meshObject;
    void Start()
    {
        meshObject.GetComponent<Renderer>().materials[0].SetTexture("_MainTex", textureToApply);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
