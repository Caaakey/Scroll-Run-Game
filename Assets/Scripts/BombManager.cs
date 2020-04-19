using UnityEngine;

public class BombManager : MonoBehaviour
{
    public GameObject prefab = null;
    public float range = 1.7f;

    public int createCount = 1;
    public float createTime = 2f;

    private float waitTime = 0f;
    private void Start()
    {
        waitTime = Time.time + createTime;
    }

    private void Update()
    {
        if (Time.time >= waitTime)
        {
            CreatePrefab();
            waitTime = Time.time + createTime;
        }
    }

    private void CreatePrefab()
    {
        for (int i = 0; i < createCount; ++i)
        {
            Vector2 pos = new Vector2(
                Random.Range(-range, range),
                transform.localPosition.y);

            Instantiate(prefab, pos, Quaternion.identity, null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector3(-range, transform.position.y),
                        new Vector3(range, transform.position.y));
    }

}
