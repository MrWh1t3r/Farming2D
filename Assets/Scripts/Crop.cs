using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    private CropData _curCrop;
    private int _plantDay;
    private int _daysSinceLastWatered;

    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCrop;
    public static event UnityAction<CropData> onHarvestCrop;

    public void Plant(CropData crop)
    {
        _curCrop = crop;
        _plantDay = GameManager.instance.curDay;
        _daysSinceLastWatered = 1;
        UpdateCropSprite();
        
        onPlantCrop?.Invoke(crop);
    }

    public void NewDayCheck()
    {
        _daysSinceLastWatered++;
        if (_daysSinceLastWatered > 3)
        {
            Destroy(gameObject);
        }
        UpdateCropSprite();
    }

    void UpdateCropSprite()
    {
        int cropProg = CropProgress();

        if (cropProg < _curCrop.daysToGrow)
        {
            sr.sprite = _curCrop.growProgressSprites[cropProg];
        }
        else
        {
            sr.sprite = _curCrop.readyToHarvestSprite;
        }
    }

    public void Water()
    {
        _daysSinceLastWatered = 0;
    }

    public void Harvest()
    {
        if (CanHarvest())
        {
            onHarvestCrop?.Invoke(_curCrop);
            Destroy(gameObject);
        }
    }

    public int CropProgress()
    {
        return GameManager.instance.curDay - _plantDay;
    }

    public bool CanHarvest()
    {
        return CropProgress() >= _curCrop.daysToGrow;
    }
}
