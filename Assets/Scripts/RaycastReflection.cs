﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class RaycastReflection : MonoBehaviour
{
    //this game object's Transform  
    private Transform goTransform;
    //the attached line renderer  
    private LineRenderer lineRenderer;

    //a ray  
    private Ray ray;
    //a RaycastHit variable, to gather informartion about the ray's collision  
    private RaycastHit hit;

    //reflection direction  
    private Vector3 inDirection;

    //the number of reflections  
    public int nReflections = 2;

    //the number of points at the line renderer  
    private int nPoints;

    void Awake()
    {
        //get the attached Transform component  
        goTransform = this.GetComponent<Transform>();
        //get the attached LineRenderer component  
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        //clamp the number of reflections between 1 and int capacity  
        nReflections = Mathf.Clamp(nReflections, 1, nReflections);
        //cast a new ray forward, from the current attached game object position  
        ray = new Ray(goTransform.position, goTransform.forward);

        //represent the ray using a line that can only be viewed at the scene tab  
        Debug.DrawRay(goTransform.position, goTransform.forward * 100, Color.magenta);

        //set the number of points to be the same as the number of reflections  
        nPoints = nReflections;
        //make the lineRenderer have nPoints  
        lineRenderer.SetVertexCount(nPoints);
        //Set the first point of the line at the current attached game object position  
        lineRenderer.SetPosition(0, goTransform.position);

        for (int i = 0; i <= nReflections; i++)
        {
            //If the ray hasn't reflected yet  
            if (i == 0)
            {
                //Check if the ray has hit something  
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100))//cast the ray 100 units at the specified direction  
                {
                    //the reflection direction is the reflection of the current ray direction flipped at the hit normal  
                    inDirection = Vector3.Reflect(ray.direction, hit.normal);
                    //cast the reflected ray, using the hit point as the origin and the reflected direction as the direction  
                    ray = new Ray(hit.point, inDirection);

                    //Draw the normal - can only be seen at the Scene tab, for debugging purposes  
                    Debug.DrawRay(hit.point, hit.normal * 3, Color.blue);
                    //represent the ray using a line that can only be viewed at the scene tab  
                    Debug.DrawRay(hit.point, inDirection * 100, Color.magenta);

                    //Print the name of the object the cast ray has hit, at the console  
                    Debug.Log("Object name: " + hit.transform.name);

                    //if the number of reflections is set to 1  
                    if (nReflections == 1)
                    {
                        //add a new vertex to the line renderer  
                        lineRenderer.SetVertexCount(++nPoints);
                    }

                    //set the position of the next vertex at the line renderer to be the same as the hit point  
                    lineRenderer.SetPosition(i + 1, hit.point);
                }
            }
            else // the ray has reflected at least once  
            {
                //Check if the ray has hit something  
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100))//cast the ray 100 units at the specified direction  
                {
                    //the refletion direction is the reflection of the ray's direction at the hit normal  
                    inDirection = Vector3.Reflect(inDirection, hit.normal);
                    //cast the reflected ray, using the hit point as the origin and the reflected direction as the direction  
                    ray = new Ray(hit.point, inDirection);

                    //Draw the normal - can only be seen at the Scene tab, for debugging purposes  
                    Debug.DrawRay(hit.point, hit.normal * 3, Color.blue);
                    //represent the ray using a line that can only be viewed at the scene tab  
                    Debug.DrawRay(hit.point, inDirection * 100, Color.magenta);

                    //Print the name of the object the cast ray has hit, at the console  
                    Debug.Log("Object name: " + hit.transform.name);

                    //add a new vertex to the line renderer  
                    lineRenderer.SetVertexCount(++nPoints);
                    //set the position of the next vertex at the line renderer to be the same as the hit point  
                    lineRenderer.SetPosition(i + 1, hit.point);
                }
            }
        }
    }

    public Vector3 getEstimatedPosition(float distanceTraveled, Vector3 direction)
    {
        bool found = false;
        Vector3 estimatedPosition = Vector3.zero;
        Vector3 position = goTransform.position;
        ray = new Ray(position, direction);

        while (! found) {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, distanceTraveled))//cast the ray 10 units at the specified direction  
            {
                distanceTraveled = distanceTraveled - Vector3.Distance(position, hit.point);
                position = hit.point;
            } else
            {
                found = true;
                estimatedPosition = position + position.normalized * distanceTraveled;
            }

        }
        return estimatedPosition;
    }
}