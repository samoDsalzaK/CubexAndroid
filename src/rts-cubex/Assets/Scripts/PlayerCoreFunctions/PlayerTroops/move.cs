﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class move : MonoBehaviour
{
    NavMeshAgent agent;
    ClickOn click;
    TroopGroupSelection clickGroup;
    [SerializeField] GameObject unitPosition;
    [SerializeField] bool onItsWay;
    [SerializeField] bool isHero = false;
    [SerializeField] bool isBloom = false;
    [SerializeField] GameObject mainModel;
    [SerializeField] bool lockMove = false;
    [SerializeField] bool isStory = false;
    [SerializeField] string saveTag = "usave";
    private TroopAttack ta;
    private bool needsCamp = true;
    public bool LockMove { set { lockMove = value; } get { return lockMove; } }
    public NavMeshAgent Agent { get { return agent; } }
    public bool NeedsCamp { set { needsCamp = value; } get { return needsCamp; } }
    public bool IsStory { set { isStory = value; } get { return isStory; } }
    void Start()
    {
        ta = GetComponent<TroopAttack>();
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(transform.position);
        click = GetComponent<ClickOn>();
        clickGroup = GetComponent<TroopGroupSelection>();
        var gs = FindObjectOfType<GameSession>();
        if (gs)
            gs.addTroopAmount(1);

        if (!isHero)
        {
            if (needsCamp)
            {
                var camps = GameObject.FindGameObjectsWithTag("Camp");
                if (camps.Length > 0)
                {
                    foreach (var item in camps)
                    {
                        var army = item.GetComponent<ArmyCamp>();
                        if (army.Occupied < army.Capacity)
                        {
                            agent.destination = item.transform.position;
                            onItsWay = true;
                            break;
                        }
                    }
                }
            }
        }
    }
    void Update()
    {
        if (isStory)
        {
            if (gameObject.tag != saveTag)
            {
                var playerBase = FindObjectOfType<Base>();
                if (playerBase)
                {
                    playerBase.addPlayerTroopsAmount(1);
                    isStory = false;
                }
            }
        }
        if (!lockMove)
            unitMove();
    }
    private void unitMove()
    {
        if (click)
        {
            if (!click.GetSelected())
            {
                return;
            }
        }
        if (clickGroup)
        {
            print("Group found");
            if (!clickGroup.GetSelected())
            {
                return;
            }
        }
        // Checking when to stop the moving unit
        if (agent.velocity.magnitude > 0f)
        {
            // ta.LockFire = true;
            // print("Unit is moving");
            if (agent.remainingDistance < 1f)
            {
                //ta.LockFire = false;
                agent.isStopped = true;
            }
            //print(agent.remainingDistance);
        }


        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("LvlMap")))
                {
                    if (isBloom && hit.transform.gameObject.tag == "maphole")
                    {
                        agent.isStopped = false;
                        agent.SetDestination(hit.point);
                        if (mainModel)
                            mainModel.transform.position = transform.position;
                        //mainModel.transform.position = hit.point;
                    }
                    if (hit.transform.gameObject.tag == "lootbox" || hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") || hit.transform.gameObject.layer == LayerMask.NameToLayer("LvlMap"))
                    {
                        agent.isStopped = false;
                        agent.SetDestination(hit.point);
                        if (mainModel)
                            mainModel.transform.position = transform.position;
                        //mainModel.transform.position = hit.point;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
    public Vector3 getUnitPosition()
    {
        return unitPosition.transform.position;
    }

    public bool getOnItsWay()
    {
        return onItsWay;
    }

    public void setOnItsWay(bool isOnWay)
    {
        onItsWay = isOnWay;
    }



}