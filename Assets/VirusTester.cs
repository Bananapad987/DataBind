using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirusTester : MonoBehaviour
{
    Scene scene;
    public GameObject vb_prefab;

    HashSet<VirusBlock> binded = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scene = gameObject.scene;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] objects = scene.GetRootGameObjects();

        VirusBlock t_1 = null;
        VirusBlock t_2 = null;

        foreach (GameObject obj in objects)
        {
            VirusBlock b = obj.GetComponent<VirusBlock>();

            if (b == null || binded.Contains(b))
            {
                continue;
            }

            Debug.Log(binded.Count);

            if (t_1 == null)
            {
                t_1 = b;
            }
            else
            {
                t_2 = b;
            }

            if (t_1 != null && t_2 != null)
            {
                VirusBinder vb = Instantiate(vb_prefab).GetComponent<VirusBinder>();
                vb.Bind(t_1, t_2, VirusBinder.TYPE.red);

                binded.Add(t_1);
                binded.Add(t_2);

                t_1 = null;
                t_2 = null;
            }
        }
    }
}
