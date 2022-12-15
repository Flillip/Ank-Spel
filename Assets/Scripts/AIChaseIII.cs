using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIChaseIII : MonoBehaviour
{
    #region Variables
    public Transform target;
    public float speed = 1f;
    public GameObject EnemyExplosionSpawner;
    public GameObject EnemyExplosionClone;
    private Rigidbody2D rb;
    #endregion
    #region Basic Functions
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        float step = speed * Time.deltaTime;

        Vector3 forceVector = target.position;
        forceVector.Normalize();
        forceVector = forceVector * step;
    }
    #endregion
    #region Collision Function
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GameObject Clone_Enemy_Explode_Handler;
            Clone_Enemy_Explode_Handler = Instantiate(EnemyExplosionClone, EnemyExplosionSpawner.transform.position, EnemyExplosionSpawner.transform.rotation) as GameObject;
            Destroy(Clone_Enemy_Explode_Handler, 5f);
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Death")
        {
            GameObject Clone_Enemy_Explode_Handler;
            Clone_Enemy_Explode_Handler = Instantiate(EnemyExplosionClone, EnemyExplosionSpawner.transform.position, EnemyExplosionSpawner.transform.rotation) as GameObject;
            Destroy(Clone_Enemy_Explode_Handler, 5f);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
