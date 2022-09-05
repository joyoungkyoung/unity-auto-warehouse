using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : BaseMonoSingleton<SystemManager>
{
    [SerializeField] ObjectPoolContainer objectPoolContainer;
    private List<Container> curContainerList = null;

    public Container GetContainerObj()
    {
        if(curContainerList == null)
        {
            curContainerList = new List<Container>();
        }
        Container container = objectPoolContainer.GetObject();
        curContainerList.Add(container);
        return container;
    }
    
    public Container FindContainerById(int id)
    {
        Container find = curContainerList.Find((obj) => obj.GetId() == id);
        if(find == null)
        {
            return null;
        }
        return find;
    }

    public void RefreshContainer()
    {
        if(curContainerList != null && curContainerList.Count > 0)
        {
            foreach (Container obj in curContainerList)
            {
                obj.InitializeColor();
            }
        }
        
    }

    public void ResetContainerPool(Transform root)
    {
        if(curContainerList == null || curContainerList.Count <= 0)
        {
            return;
        }

        curContainerList.Clear();
        foreach (Container obj in curContainerList) {
            obj.Initialize();
            objectPoolContainer.ReturnObject(obj);
        }
    }


}