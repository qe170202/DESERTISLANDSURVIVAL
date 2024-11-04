using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFireReaction : CampFireHit
{
    public float time;
    public GameObject fire;
    GameObject toolbar;

    private void Start()
    {
        
    }

    public override void Hit()
    {
        if (toolbar == null)
        {
            toolbar = GameObject.FindWithTag("toolbar");  // Tìm toolbar khi hộp thoại đã xuất hiện
            if (toolbar == null)
            {
                Debug.LogError("Toolbar not found with tag 'toolbar' after dialog.");
                return;
            }
        }

        // Bật lửa và các thao tác với toolbar
        fire.SetActive(true);
        GameManager.instance.inventoryContainer.RemoveItem(GameManager.instance.toolbarControllerGlobal.GetItem, 5);
        toolbar.SetActive(!toolbar.activeInHierarchy);
        toolbar.SetActive(true);
        StartCoroutine(LateCall());
    }


    IEnumerator LateCall()
    {

        yield return new WaitForSeconds(40);
        fire.SetActive(false);
    }
}
