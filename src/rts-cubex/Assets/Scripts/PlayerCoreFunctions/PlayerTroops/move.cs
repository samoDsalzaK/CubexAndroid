using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class move : MonoBehaviour
{
    NavMeshAgent agent;
    ClickOn click;
    [SerializeField] GameObject unitPosition;
    [SerializeField] bool onItsWay;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(transform.position);
        click = GetComponent<ClickOn>();
        FindObjectOfType<GameSession>().addTroopAmount(1);
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
    void Update()
    {
        unitMove();
    }
    private void unitMove()
    {
        if (!click.GetSelected())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") || hit.transform.gameObject.layer == LayerMask.NameToLayer("LvlMap"))
                    {
                        agent.isStopped = false;
                        agent.destination = hit.point;
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