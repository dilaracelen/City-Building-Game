using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private bool currentlyPlacing;
    private bool currentlyBulldozering;


    private BuildingPreset curBuildingPreset;

    private float indicatorUpdateTime = 0.05f;
    private float lastUpdateTime;
    private Vector3 curIndicatorPos;

    public GameObject placementIndicator;
    public GameObject bulldozerIndicator;

    public GameObject HousePanel;
    public GameObject TreePanel;

    public void BeginNewBuildingPlacement(BuildingPreset preset)
    {
        /*if(City.instance.money<preset.cost)
        {
            return;
        }*/

        currentlyPlacing = true;
        curBuildingPreset = preset;
        placementIndicator.SetActive(true);
    }

    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
    }

    public void ToggleBulldoze()
    {
        currentlyBulldozering = !currentlyBulldozering;
        bulldozerIndicator.SetActive(currentlyBulldozering);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildingPlacement();


        if (Time.time - lastUpdateTime > indicatorUpdateTime)
        {
            lastUpdateTime = Time.time;


            curIndicatorPos = Selector.instance.GetCurTilePosition();


            if (currentlyPlacing)
                placementIndicator.transform.position = curIndicatorPos;

            else if (currentlyBulldozering)
                bulldozerIndicator.transform.position = curIndicatorPos;

        }

        if (Input.GetMouseButtonDown(0) && currentlyPlacing)
        {
            PlaceBuilding();
        }

        else if (Input.GetMouseButtonDown(0) && currentlyBulldozering)
        {
            Bulldoze();
        }
    }

    void PlaceBuilding()
    {
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, new Vector3(curIndicatorPos.x, curBuildingPreset.prefab.transform.position.y, curIndicatorPos.z), Quaternion.identity);
        City.instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());
        CancelBuildingPlacement();
    }

    void Bulldoze()
    {
        Building buildingToDestroy = City.instance.buildings.Find(x => x.transform.position == curIndicatorPos);
        if (buildingToDestroy != null)
        {
            City.instance.OnRemoveBuilding(buildingToDestroy);
        }
    }

    public void HouseButton()
    {
        if (HousePanel.activeInHierarchy)
            HousePanel.SetActive(false);

        else if(!HousePanel.activeInHierarchy)
            HousePanel.SetActive(true);
    }

    public void TreeButton()
    {
        if (TreePanel.activeInHierarchy)
            TreePanel.SetActive(false);

        else if (!TreePanel.activeInHierarchy)
            TreePanel.SetActive(true);
    }
}




