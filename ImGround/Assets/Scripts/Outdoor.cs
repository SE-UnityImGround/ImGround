using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outdoor : MonoBehaviour
{

    public AudioSource[] effectSound;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("IsOpen", true);
            effectSound[0].Play();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("IsOpen", false);
            effectSound[1].Play();
        }
    }
}



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outdoor : MonoBehaviour
{
    public AudioSource[] effectSound; // [0]: Open sound, [1]: Close sound
    private Animator animator;
    private bool isOpen = false; // �� ���¸� ����

    void Start()
    {
        animator = GetComponent<Animator>();

        // Animator �ʱ�ȭ: ���� �� IsOpen�� false�� �����ǵ��� ����
        animator.SetBool("IsOpen", false);

        // AudioSource�� Play On Awake ��Ȱ��ȭ (�����ϰ� ����)
        foreach (var sound in effectSound)
        {
            sound.playOnAwake = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = true; // �� ���� ���·� ����
            animator.SetBool("IsOpen", true);
            effectSound[0].Play(); // Open sound ���
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            isOpen = false; // �� ���� ���·� ����
            animator.SetBool("IsOpen", false);
            effectSound[1].Play(); // Close sound ���
        }
    }
}*/
