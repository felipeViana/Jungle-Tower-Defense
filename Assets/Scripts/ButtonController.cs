using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void SetIsBuildingSimpleTower()
    {
        GameManager.Instance.SetIsBuilding(0);
    }

    public void SetIsBuildingFastTower()
    {
        GameManager.Instance.SetIsBuilding(1);
    }

    public void SetIsBuildingBigTower()
    {
        GameManager.Instance.SetIsBuilding(2);
    }
}
