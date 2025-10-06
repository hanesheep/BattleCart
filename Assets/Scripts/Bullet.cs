using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float deleteTime = 5.0f;
    public GameObject boms;


    void Start()
    {
        //deleteTime秒後に消える
        Destroy(gameObject,deleteTime);
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //相手がEnemyタグなら相手を削除
            Destroy(other.gameObject);
            //相手がEnemyタグならbomsを生成
            Instantiate(boms, transform.position, Quaternion.identity);

        }

        //いずれにしても自分は削除
        Destroy(gameObject);
    }
}
