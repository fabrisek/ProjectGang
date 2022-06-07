using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByPass
{
    public class RouteShader : MonoBehaviour
    {
        [SerializeField] Material material;
        float time;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime*0.1f;
            material.mainTextureOffset = new Vector2(0,time);
        }
    }
}
