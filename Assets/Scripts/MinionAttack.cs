using UnityEngine;
using System.Collections;
using System;

public class MinionAttack : MonoBehaviour
{
    public float attackProgress;
    public bool isMyMinion;
    public int index;
    public bool targetSuccess;
    public Animator anim;
    public GameObject target;
    public Vector3 flyTargetPos;
    public Vector3 flyOriginPos;
    [SerializeField]
//    PlaceManager placingManager;
    private bool isTarget;
    Camera mainCamera;
    GameObject demegadi;

    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
//        placingManager = GameObject.FindGameObjectWithTag("PlaceManager").GetComponent<PlaceManager>() as PlaceManager;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("minionAttack"))
        {
            transform.position = Vector3.Lerp(flyOriginPos, flyTargetPos, attackProgress);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("lift"))
        {

        }
    }

    GameObject GetCursorObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {

            return hit.collider.gameObject;
        }
        return null;
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public void AnimLiftUp()
    {
        iTween.MoveBy(gameObject, new Vector3(0, 1.0f, 0), 0.2f);
 //       placingManager.inLiftState = true;

    }

    public void OnMouseDown()
    {
        //此时应当  抬起卡，浮空   开启kinematic  生成虚线
        if (isMyMinion)
        {
            anim.SetBool("Lifted", true);
        }
 //       placingManager.inLiftState = true;
 //       placingManager.liftedMinion = gameObject;
    }

    // OnMouseUp is called when the user has released the mouse button
    public void OnMouseUp()
    {
        //       if (placingManager.inLiftState && placingManager.targetMinion)
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("lift") && target)
        {
//            placingManager.inLiftState = false;
            anim.SetBool("DecideAttack", true);
            flyOriginPos = transform.position;
            flyTargetPos = target.transform.position;
            demegadi = target.transform.FindChild("demegadi").gameObject;

        }
        else
        {
            anim.SetBool("Lifted", false);
        }
    }
    public void AnimAttackHit()
    {
        //收到动画给的事件，再把
        demegadi.SetActive(true);
    }

    public void AnimDropMinion()
    {
        //rigidbody.isKinematic = false;
        anim.SetBool("Lifted", false);
        anim.SetBool("DecideAttack", false);
        demegadi.SetActive(false);
//        placingManager.inLiftState = false;
  //      placingManager.liftedMinion = null;
    }

    // OnMouseOver is called every frame while the mouse is over the GUIElement or Collider
    public void OnMouseOver()
    {
        //位置不变，但实时更新虚线；并且（若指到对方）虚线尽头出个红心
        //后续可加：若其中一方会死，则加上死亡图标

    }

    // OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider
/*    public void OnMouseExit()
    {
        if (isTarget) { QuitTarget(); }
    }
*/
    private void QuitTarget()
    {
// todo         throw new NotImplementedException();
    }

    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    public void OnMouseEnter()
    {
 /*/       if (!placingManager.inLiftState) { return; }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(""))
        if (placingManager.liftedMinion == gameObject) { return; }
        BeTarget();        //暂时不加逻辑，后续可以刷一圈光效；在旁边显示这张卡，以让玩家看清楚
*/
    }

    // OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse
    public void OnMouseDrag()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("lift")) { return; }
        //Camera.current.ray
        GameObject obj = GetCursorObject();
        if (!obj || !obj.CompareTag("Minion") || obj == gameObject || obj.GetComponent<MinionAttack>().isMyMinion)
        {
            target = null;
            targetSuccess = false;
            return;
        }
        else
        {
            target = obj;
            targetSuccess = true;
            //todo update the red line
        }
    }
    public void BeTarget()
    {
        isTarget = true;
        // TODO  边上放光；进行预计算，如果要死则显示骷髅图案 ;show detail of this card
    }

}
