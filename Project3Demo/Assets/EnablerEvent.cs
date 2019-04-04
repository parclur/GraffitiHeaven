using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablerEvent : MonoBehaviour, ITriggerable
{

    [SerializeField] private List<GameObject> enabledObjs;
    [SerializeField] private List<GameObject> allObjs;

    [SerializeField] private float disableWait;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent(){
        foreach (GameObject i in allObjs){
            if ((enabledObjs.Contains(i)) && (i.activeSelf == false)){
                i.SetActive(true);
            } else {
                StartCoroutine(DisableObj(i));
            }
        }
    }

    private IEnumerator DisableObj(GameObject target){
        yield return new WaitForSeconds(disableWait);
        target.SetActive(false);
    }
}
