using System;
using System . Collections ;
using System . Collections . Generic ;
using TMPro;
using UnityEngine ;
using UnityEngine . InputSystem ;

public class PlayerController : MonoBehaviour
{

    public Vector2 moveValue;
    public float speed;
    private int count;
    private int numPickups = 5;

    private Vector3 lastPos;
    
    public TextMeshProUGUI winText;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        count = 0;
        winText.text = "";
        lastPos = gameObject.transform.position;
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
        SetCountText();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    private void SetCountText()
    {
        scoreText.text = "Score: " + count.ToString();
        if (count >= numPickups)
        {
            winText.text = "YOUR WINNER!";
        }
    }
}