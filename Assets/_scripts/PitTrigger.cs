using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameObject trigger = GetNearestActiveCheckpoint();

            if (trigger != null)
            {
                collider.transform.position = trigger.transform.position;
            }
            else
            {
                Debug.LogError("No Valid checkpoint was found!");
            }
        }
        else
        {
            Destroy(collider.gameObject);
        }
    }

    GameObject GetNearestActiveCheckpoint()
    {
        GameObject[] checkpoints =
            GameObject.FindGameObjectsWithTag("Checkpint");
        GameObject nearestCheckpoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach(GameObject checkpoint in checkpoints)
        {
            Vector3 checkpointPosition = checkpoint.transform.position;
            float distance =
                (checkpointPosition - transform.position).sqrMagnitude;


            CheckpointTrigger trigger =
                checkpoint.GetComponent<CheckpointTrigger>();

            if (distance < shortestDistance && trigger.isTriggered == true)
            {
                nearestCheckpoint = checkpoint;
                shortestDistance = distance;
            }
        }
        return nearestCheckpoint;
    }
}
