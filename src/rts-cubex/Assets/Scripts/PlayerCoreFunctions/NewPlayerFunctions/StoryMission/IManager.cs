using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IManager : MonoBehaviour
{
    [Header("Building saving cnf.:")]
    [SerializeField] string baseToSaveTag = "basesave";
    [Header("Troop saving cnf.:")]
    [SerializeField] string saveTroopTag = "usave";
    [SerializeField] string tagNewToAdd = "Unit";
    [SerializeField] float range = 30f;
    [SerializeField] bool saveMode = false;
    [SerializeField] bool baseSaved = false;
    public bool SaveMode {set { saveMode = value; } get { return saveMode; }}
    private GameObject foundedPlayerBase;
    void Start()
    {
        if (saveMode)
            print("Hero " + gameObject.name + " is checking troops to save!");
    }

    // Update is called once per frame
    void Update()
    {
        //Interaction detection logic
        if (saveMode)
        {
            castIDetectionField();
            
            //If base is not yet saved, then don't remember it for a bit...
            if (!baseSaved)
            {
                checkIfBNotInRange();
            }
        }
    }
    private void checkIfBNotInRange()
    {
        //Checking if the detected building is not in range
        if (foundedPlayerBase)
        {
            var basePos = foundedPlayerBase.transform.position;
            var checkerPos = transform.position;
            var distance = Vector3.Distance(basePos, checkerPos);

            if (distance > range)
            {
                print("We are no longer near the base");
                foundedPlayerBase = null;
            }
        }

    }
    private void castIDetectionField()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == saveTroopTag)
            {
                var troopToSave = hitCollider.gameObject;
                troopToSave.tag = tagNewToAdd;
                var click = troopToSave.GetComponent<ClickOn>();
                if (click)
                    click.IsSelected = true;

                print("Troop: " + troopToSave.name + " is " + (troopToSave.tag == tagNewToAdd ? "saved!" : "not saved!"));

            }
            if (!baseSaved)
            {
                if (hitCollider.tag == baseToSaveTag && !foundedPlayerBase)
                {
                    print("We found the lost alpha base!");
                    var playerBase = hitCollider.gameObject;
                    foundedPlayerBase = playerBase;
                    if (playerBase)
                    {
                        var pMgr = playerBase.GetComponent<Base>();
                        if (pMgr)
                        {
                            var creditsRes = pMgr.getEnergonAmount();
                            var energonMgr = pMgr.getCreditsAmount();

                            if (creditsRes <= 0 && energonMgr <= 0)
                            {
                                print("The base is unavailable to use. It is rquired to save nergon and credits!");
                            }
                            else 
                            {
                                print("The base has been saved!");
                               
                                baseSaved = true;
                                
                                //  pMgr.ExistMaxWorkersAmount.SetActive(true);
                                //  pMgr.CPlayerTroopsAmount.SetActive(true);
                                 pMgr.IsSaved = baseSaved;
                            }
                        }
                    }
                }
            }
        }
    }
}
