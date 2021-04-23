using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopSkinManager : MonoBehaviour
{
    // playerbase type variable
    private Base playerbase;

    [SerializeField] string troopType;

    public string returnTroopType {get {return troopType;}}
    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectOfType<Base>() == null)
        {
          return;
        }
        else{
          playerbase = FindObjectOfType<Base>();
        }
        playerbase.GetComponent<changeSkinManager>().applyChosenSkin(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
