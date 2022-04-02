using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public GameObject poolObject = null;

    Stack<GameObject> stack = new Stack<GameObject>();
    
    public GameObject Spawn()
    {
        GameObject entity = null;

        if (stack.Count == 0)
        {
            entity = (GameObject)Instantiate(poolObject, this.transform);
            entity.GetComponent<PoolObject>().pool = this;
        }
        else
            entity = stack.Pop();

        entity.SetActive(true);

        Debug.Log("Stack count: " + stack.Count.ToString());
        return entity;
    }

    public void DeSpawn(GameObject entity)
    {
        entity.SetActive(false);
        stack.Push(entity);
        Debug.Log("Stack count: " + stack.Count.ToString());
    }
}
