using Unity.VisualScripting;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    const float Lanewidth = 6.0f; //���[����

    public GameObject[] dangerPrefab; //���������댯�Ԃ̃v���n�u

    public float minIntervalTime = 0.1f; //�C���^�[�o���̍ŏ�
    public float maxIntervalTime = 3.0f; //�C���^�[�o���̍ő�

    float timer; //���Ԍo�߂��ϑ�
    float posX; //�댯�Ԃ̏o��X���W

    GameObject cam; //�J�����I�u�W�F�N�g

    //�����ʒu
    public Vector3 defaultPos = new Vector3(0, 10, -60);

    Vector3 diff;
    public float followSpeed = 8; //�W�F�l���[�^�[�̕�ԃX�s�[�h

    int isSky;

    void Start()
    {
        transform.position = defaultPos;
        cam = Camera.main.gameObject;
        diff = transform.position - cam.transform.position;
    }

    void Update()
    {
        if (GameManager.gameState != GameState.playing) return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            DangerCreated();
            maxIntervalTime -= 0.1f;
            maxIntervalTime = Mathf.Clamp(maxIntervalTime, 0.1f, 3.0f);
            timer = Random.Range(minIntervalTime, maxIntervalTime + 1);
        }

        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, cam.transform.position + diff, followSpeed * Time.deltaTime);
        }

        void DangerCreated()
        {
            isSky = Random.Range(0, 2);
            int rand = Random.Range(-2, 3);
            posX = rand * Lanewidth;
        }

        Vector3 v = new Vector3(posX, transform.position.y, transform.position.z);

        if (isSky == 0) v.y = 1;

        Instantiate(dangerPrefab[isSky], v, dangerPrefab[isSky].transform.rotation);
    }
}
