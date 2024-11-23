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
    private bool isOpen = false; // 문 상태를 추적

    void Start()
    {
        animator = GetComponent<Animator>();

        // Animator 초기화: 시작 시 IsOpen이 false로 설정되도록 보장
        animator.SetBool("IsOpen", false);

        // AudioSource의 Play On Awake 비활성화 (안전하게 설정)
        foreach (var sound in effectSound)
        {
            sound.playOnAwake = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = true; // 문 열림 상태로 변경
            animator.SetBool("IsOpen", true);
            effectSound[0].Play(); // Open sound 재생
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            isOpen = false; // 문 닫힘 상태로 변경
            animator.SetBool("IsOpen", false);
            effectSound[1].Play(); // Close sound 재생
        }
    }
}*/
