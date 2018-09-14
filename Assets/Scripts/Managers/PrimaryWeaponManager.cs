using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryWeaponManager : MonoBehaviour {
    public List<IShootable> primaryWeaponsPowerUp;
    public static PrimaryWeaponManager instance=null;

	void Awake () {
        if (instance == null) {
            instance = this;
        }
	}


    public IShootable GetNextPowerUp(IShootable gun) {
        int index = primaryWeaponsPowerUp.FindIndex(x => x == gun);
        if (index!=-1 && index < primaryWeaponsPowerUp.Count) {
             return primaryWeaponsPowerUp[++index];
        }
        return gun;
    }

    public IShootable GetPreviousPowerUp(IShootable gun)
    {
        int index = primaryWeaponsPowerUp.FindIndex(x => x == gun);
        if (index != -1 && index < primaryWeaponsPowerUp.Count)
        {
            return primaryWeaponsPowerUp[--index];
        }
        return gun;
    }
    public int Index(IShootable gun)
    {
        
        return primaryWeaponsPowerUp.FindIndex(x => x == gun); 
    }
}
