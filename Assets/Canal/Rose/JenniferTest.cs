using UnityEngine;
using System.Collections;

public class JenniferTest : MonoBehaviour {
    public float walkSpeed = 2f;
    public Animator animator;

	void Update () {
        float move = Input.GetAxisRaw("Horizontal");

        transform.position += Vector3.right * move * walkSpeed * Time.deltaTime;
        animator.SetFloat("hSpeed", move * walkSpeed);
        animator.SetBool("Walking", Mathf.Abs(move) > 0.1f);

        if(move > 0)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        else if(move < 0)
        {
            transform.localEulerAngles = Vector3.up * 180f;
        }
	}
}
