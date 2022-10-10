using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject[] pickup;
    public TextMeshProUGUI closestText;
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI positionText;
    private int closestIndex;
    private LineRenderer lineRenderer;
    private Vector3 lastPos;

    private Mode mode;
    // Start is called before the first frame update

    enum Mode
    {
        Normal,
        Distance,
        Vision
    }

    void Start()
    {
        mode = Mode.Normal;
        closestIndex = 0;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lastPos = player.transform.position;
    }

    void OnDebug()
    {
        switch (mode)
        {
            case Mode.Normal:
                mode = Mode.Distance;
                break;
            case Mode.Distance:
                mode = Mode.Vision;
                break;
            case Mode.Vision:
                mode = Mode.Normal;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mode != Mode.Distance)
        {
            positionText.text = "";
            velocityText.text = "";
            closestText.text = "";
        }

        if (mode == Mode.Normal)
        {
            lineRenderer.startWidth = 0;
            lineRenderer.endWidth = 0;
            for (int i = 0; i < pickup.Length; i++)
            {
                pickup[i].GetComponent<Renderer>().material.color = Color.white;
            }
        }
        if (mode == Mode.Distance)
        {

            positionText.text = "Position: " + player.transform.position;
            velocityText.text = "Velocity: " + (player.transform.position - lastPos)
                                             + " " + (player.transform.position - lastPos).magnitude;
            for (int i = 0; i < pickup.Length; i++)
            {
                if (!pickup[closestIndex].activeSelf && pickup[i].gameObject.activeSelf)
                {
                    closestIndex = i;
                }

                if (Vector3.Distance(player.transform.position, pickup[i].transform.position) <
                    (Vector3.Distance(player.transform.position, pickup[closestIndex].transform.position))
                    && pickup[i].gameObject.activeSelf)
                {
                    closestIndex = i;
                }
            }

            for (int i = 0; i < pickup.Length; i++)
            {
                if (i == closestIndex)
                {
                    pickup[i].GetComponent<Renderer>().material.color = Color.blue;
                }
                else
                {
                    pickup[i].GetComponent<Renderer>().material.color = Color.white;
                }
            }

            closestText.text = "Closest: " +
                               Vector3.Distance(player.transform.position, pickup[closestIndex].transform.position);
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, pickup[closestIndex].transform.position);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }

        if (mode == Mode.Vision)
        {
            // Velocity Line
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, player.transform.position + (player.transform.position - lastPos) * 50);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;


            for (int i = 0; i < pickup.Length; i++)
            {
                if (!pickup[closestIndex].activeSelf && pickup[i].gameObject.activeSelf)
                {
                    closestIndex = i;
                }

                if (Vector3.Angle((player.transform.position - lastPos), pickup[i].transform.position) <
                    (Vector3.Angle((player.transform.position - lastPos), pickup[closestIndex].transform.position))
                    && pickup[i].gameObject.activeSelf)
                {
                    closestIndex = i;
                }
            }

            for (int i = 0; i < pickup.Length; i++)
            {
                if (i == closestIndex)
                {
                    pickup[i].GetComponent<Renderer>().material.color = Color.green;
                    pickup[i].transform.LookAt(player.transform);
                }
                else
                {
                    pickup[i].GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }

        lastPos = player.transform.position;
    }
}
