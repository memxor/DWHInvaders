using System.Collections;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyEnumerator());
    }

    private IEnumerator DestroyEnumerator()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}