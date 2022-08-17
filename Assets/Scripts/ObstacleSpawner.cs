using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {

	[SerializeField] private float waitTime;
	[SerializeField] private GameObject[] obstaclePrefabs;
	private float tempTime;
    private int prePipe = -1;
    List<int> difficults = new List<int>() { 0,0,0,1,1};
    int currentDifficult = 0;
    public int difRange = 3;
	void Start(){
		tempTime = waitTime - Time.deltaTime;
	}

	void LateUpdate () {
		if(GameManager.Instance.GameState()){
			tempTime += Time.deltaTime;
			if(tempTime > waitTime){
				// Wait for some time, create an obstacle, then set wait time to 0 and start again
				tempTime = 0;
				GameObject pipeClone = Instantiate(GetPipe(), transform.position, transform.rotation);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.transform.parent != null){
			Destroy(col.gameObject.transform.parent.gameObject);
		}else{
			Destroy(col.gameObject);
		}
	}
    GameObject GetPipe()
    {

        if (prePipe == -1)
        {
            int rand = Random.Range(0, obstaclePrefabs.Length);
            prePipe = rand;
            InscreaseIndexDifficult();
            return obstaclePrefabs[rand];
        }
        else
        {
            int tempIndex = prePipe;
            if (prePipe > (difficults.Count / 2 - 1))
            {
                tempIndex = difficults.Count - 1 - prePipe;
            }
            int range = 1;
            if (difRange > obstaclePrefabs.Length / 2)
            {
                difRange = obstaclePrefabs.Length / 2;
            }
            if (difficults[currentDifficult] == 0)
            {
                range = Random.Range(1, difRange);
            }
            else
            {                
                range = Random.Range(difRange, difficults.Count - tempIndex);
            }
            if (prePipe - range < 0)
            {
                prePipe += range;
                
            }
            else if (prePipe + range >= obstaclePrefabs.Length)
            {
                prePipe -= range;
            }
            else
            {
                int rand = (Random.Range(0, 2) == 0) ? -1 : 1;
                prePipe += rand * range;
            }
            InscreaseIndexDifficult();
            return obstaclePrefabs[prePipe];
        }
    }
    void InscreaseIndexDifficult()
    {
        currentDifficult++;
        if (currentDifficult >= difficults.Count)
        {
            currentDifficult = 0;
        }
    }
}
