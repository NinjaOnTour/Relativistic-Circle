using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Circle : MonoBehaviour
{
    private Vector3[] RealPos;
    private Vector3[] ObservedPos;
    
    private LineRenderer lineR;
    private LineRenderer lineO;

    public Transform Observer;
    public int N;
    public float InitialRadius;
    public float SpeedOfLight;
    public float SpeedOfExpansion;
    public float LineWidth = 0.2f;

    public TextMeshProUGUI ABRatio;

    private void Start()
    {
        RealPos = new Vector3[N];
        lineR = Instantiate(new GameObject("RealLineRenderer"), transform.position, Quaternion.identity).AddComponent<LineRenderer>();
        lineR.material.color = Color.black;
        lineR.positionCount = N+1;
        lineR.startWidth = LineWidth;
        lineR.endWidth = LineWidth;
        

        for (int i = 0; i < N; i++)
        {
            float angle = 2 * Mathf.PI / N * i;
            RealPos[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * InitialRadius;
            lineR.SetPosition(i, RealPos[i]);
        }
        lineR.SetPosition(N, RealPos[0]);

        ObservedPos = new Vector3[N];
        lineO = Instantiate(new GameObject("ObservedLineRenderer"), transform.position, Quaternion.identity).AddComponent<LineRenderer>();
        lineO.positionCount = N + 1;
        lineO.startWidth = LineWidth;
        lineO.endWidth = LineWidth;


        for (int i = 0; i < N; i++)
        {
            float angle = 2 * Mathf.PI / N * i;
            ObservedPos[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * InitialRadius;
            lineO.SetPosition(i, ObservedPos[i]);
        }
        lineO.SetPosition(N, ObservedPos[0]);
    }

    private void Update()
    {
        for (int i = 0; i < N; i++)
        {
            float angle = 2 * Mathf.PI / N * i;
            RealPos[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * (InitialRadius + SpeedOfExpansion * Time.time);
            float delay = Vector3.Distance(RealPos[i], Observer.position) / SpeedOfLight;
            float time = Time.time - delay;
            if (time < 0f)
                ObservedPos[i] = new Vector3(0, 0, -100);
            else
            {
                ObservedPos[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * (InitialRadius + SpeedOfExpansion * time);
            }
            lineR.SetPosition(i, RealPos[i]);
            lineO.SetPosition(i, ObservedPos[i]);
        }
        lineR.SetPosition(N, RealPos[0]);
        lineO.SetPosition(N, ObservedPos[0]);

        float A = ObservedPos[N/2+1].x;
        float B = ObservedPos[0].x;
        ABRatio.text = "A = " + System.MathF.Round(Mathf.Abs(A), 4) + "\nB = " + System.MathF.Round(Mathf.Abs(B), 4) + "\nA/B = " + System.MathF.Round(Mathf.Abs(A / B), 4);
    }
}
