using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;
    public Vector3 velocity;
    public float   smoothSpeed = 0.5f;

    public float   maxZoom = 100;
    public float   minZoom = 3.5f;
    public float   zoomLimiter = 200;
    private Camera cam;
    private GameController controller;

    PreviewManager pw;


    void Start() 
    {
        cam = GetComponent<Camera>();   
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
    }
    void LateUpdate() 
    {
        if (targets.Count == 0) return;

        MOVE();
        ZOOM();
    }

    void MOVE()
    {
        Vector3 centerPoint = GetCentrePoint();
        Vector3 newPoint    = centerPoint + offset;
        transform.position  = Vector3.SmoothDamp(transform.position, newPoint, ref velocity, smoothSpeed); 
    }

    void ZOOM()
    {
        float newZoom           = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistance() / zoomLimiter);
        cam.orthographicSize    = newZoom;
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position,Vector3.zero);
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            bounds.Encapsulate(targets[i].position);
        }

        if (bounds.size.x > bounds.size.y)
        {
            return bounds.size.x;
        }
        return bounds.size.y;
    }

    private Vector3 GetCentrePoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

}
