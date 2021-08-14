using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public void SetDontDestroyed()
    {
        instance = GetInstance();
    }
    public static T Instance
    {
        get
        {
            return GetInstance();
        }
    }
    public static T GetInstance()
    {
        instance = (T)FindObjectOfType(typeof(T));
        if (instance == null)
        {
            T[] objs = FindObjectsOfType<T>();

            if (objs.Length > 1)
            {
                Debug.LogError("There are Singleotones more than 1" + typeof(T).Name);
            }
            if (objs.Length >= 1)
            {
                instance = objs[0];
            }
            else
            {
                //싱글톤 클래스 프리팹 있을경우 생성.
                GameObject gObj = Resources.Load<GameObject>("Prefab/"+ typeof(T).Name);

                if (gObj != null)
                {
                    instance = Instantiate(gObj).GetComponent<T>();
                }

                //없으면 새로운 오브젝트로 생성.
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<T>();
                    instance.gameObject.name = typeof(T).Name;

                }

            }
            instance = (T)FindObjectOfType(typeof(T));
        }

        DontDestroyOnLoad(instance);

        return instance;
    }
}

