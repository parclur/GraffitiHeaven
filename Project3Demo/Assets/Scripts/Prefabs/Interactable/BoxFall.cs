using UnityEngine;

public class BoxFall : MonoBehaviour {

    BoxCollider col;
    public Rigidbody rb;

    public bool dontPush;

    void Start(){
        col = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }
    
    void Update(){
        if(Mathf.Abs(rb.velocity.y) >= 0.3f){
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            dontPush = true;
        }
        else dontPush = false;
    }
}
