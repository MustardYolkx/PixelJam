using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public enum Level
    {
        level1,
        level2,
    }
    public Level currentLevel;

    public string sceneName;

    public List<GameObject> eliminateEnemyList ;

    public bool triggerStart;
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        eliminateEnemyList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLevel == Level.level1)
        {
            Level1Event();
        }
        if (currentLevel == Level.level2)
        {
            Level2Event();
        }
    }

    public void Level1Event()
    {
        if(count== 4)
        {
            if (eliminateEnemyList[0]==null&& eliminateEnemyList[1] == null&&eliminateEnemyList[2] == null&&eliminateEnemyList[3] == null)
            {
                SceneManager.LoadScene(sceneName);
            }
        }


    }
    public void Level2Event()
    {

        if (count == 4)
        {
            if (eliminateEnemyList[0] == null && eliminateEnemyList[1] == null && eliminateEnemyList[2] == null && eliminateEnemyList[3] == null)
            {
                SceneManager.LoadScene(sceneName);
            }
        }

    }

    public void AddObjToList(GameObject obj)
    {
        eliminateEnemyList.Add(obj);
        count++;
    }
}
