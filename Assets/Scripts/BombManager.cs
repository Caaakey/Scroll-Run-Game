using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static BombManager Get { get; set; } = null;

    public static void AddBomb()
        => Get.list.Add(Get.UpdateBomb());

    public GameObject prefab = null;
    public int firstCount = 4;

    private float range = 0f;
    private List<IEnumerator> list = new List<IEnumerator>();

    private void Awake()
        => Get = this;

    private void Start()
    {
        range = Camera.main.aspect;

        for (int i = 0; i < firstCount; ++i)
            list.Add(UpdateBomb());
    }

    private void Update()
    {
        for (int i = 0; i < list.Count; ++i)
            list[i].MoveNext();
    }

    private void CreateBomb(float gravity)
    {
        Vector2 pos = new Vector2(
                Random.Range(-range, range),
                transform.localPosition.y);

        var go = Instantiate(prefab, pos, Quaternion.identity, null);
        go.GetComponent<Rigidbody2D>().gravityScale = gravity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector3(-range, transform.position.y),
                        new Vector3(range, transform.position.y));
    }

    private IEnumerator UpdateBomb()
    {
        float gravityScale = Random.Range(0.2f, 1f);
        float waitTime = Random.Range(0.25f, 1f);
        float fixedTime = Time.time;
        while (true)
        {
            if (Time.time >= fixedTime + waitTime)
            {
                CreateBomb(gravityScale);
                fixedTime = Time.time;
            }

            yield return null;
        }
    }
}
