using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperMagazine : MonoBehaviour
{
    [SerializeField] int ammo = 5;
    [SerializeField] int maxAmmo = 5;
    [SerializeField] Text magazine;
    [SerializeField] GameObject reload1;
    [SerializeField] GameObject reload2;
    public bool fullMag = true;
    // Start is called before the first frame update
    void Update()
    {
        if (ammo == 0)
        {
            fullMag=false;
            reload1.SetActive(false);
            reload2.SetActive(true);
            StartCoroutine(Reload());
        }
    }
    public void decreaseAmmo()
    {
        ammo -= 1;
        magazine.text = ammo + "/" + maxAmmo;
    }
    public int getAmmo()
    {
        return ammo;
    }
    public bool getFullMag()
    {
        return fullMag;
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2);
        ammo = 5;
        magazine.text = ammo + "/" + maxAmmo;
        yield return new WaitForSeconds(2);
        reload1.SetActive(true);
        reload2.SetActive(false);
        fullMag=true;
    }
}
