using UnityEngine;
using DG.Tweening;

public class testplayer : MonoBehaviour{
    void Start(){
        
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-10f * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(10f * Time.deltaTime, 0, 0);
        }
        if(Input.GetKeyUp(KeyCode.S)){
            Camera.main.DOShakePosition(1f, 0.3f);
        }
    }
}
