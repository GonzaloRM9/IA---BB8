using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetectorScript : MonoBehaviour
{

    public float angleOfSensors = 10f;
    public float rangeOfSensors = 10f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float angleToClosestObj;
    public int numObjects;
    public bool debugMode;
    public float multiplier = 1f;
    public float exponent = 1f;
    public float add = 0f;
    // Start is called before the first frame update
    void Start()
    {

        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ObjectInfo anObject;
        anObject = GetClosestBlock();
        if (anObject != null)
        {
            angleToClosestObj = anObject.angle;
            strength = -1/(anObject.distance+1);
        }
        else
        { // no object detected
            strength = 0;
            angleToClosestObj = 0;
        }

    }

    public float GetAngleToClosestObstacle()
    {
        return angleToClosestObj;
    }

    public float GetLinearOuput(float exponent, float multiplier, float add, float shift, float dilate)
    {

        return (float)Math.Pow(strength * dilate + shift, exponent) * Math.Abs(multiplier) + Math.Abs(add);
    }
    public virtual float GetLogaritmicOutput(float b, float shift)
    {
        // YOUR CODE HERE

        return (float)Math.Log(strength + shift) / (float)Math.Log(b);
    }
    public virtual float GetGaussianOutput(float mu, float sigma, float shift)
    {

        return (float)Math.Exp(-Math.Pow((strength + shift - mu), 2) / (2 * Math.Pow(sigma, 2)));
    }
    public virtual float GetTrigonometricOutput(float shift, float dilate) //apeteceu-me
    {
        // YOUR CODE HERE

        return (float)Math.Cos(strength * dilate + shift);
    }
    
    public ObjectInfo GetClosestBlock()
    {
        ObjectInfo[] a = (ObjectInfo[])GetVisibleObjects("Wall").ToArray();
        if (a.Length == 0)
        {
            return null;
        }
        return a[a.Length - 1];
    }
    public List<ObjectInfo> GetVisibleObjects(string objectTag)
    {
        RaycastHit hit;
        List<ObjectInfo> objectsInformation = new List<ObjectInfo>();

        for (int i = 0; i * angleOfSensors < 360f; i++)
        {
            if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors))
            {

                if (hit.transform.gameObject.CompareTag(objectTag))
                {
                    if (debugMode)
                    {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.red);
                    }
                    ObjectInfo info = new ObjectInfo(hit.distance, angleOfSensors * i + 90);
                    objectsInformation.Add(info);
                }
            }
        }

        objectsInformation.Sort();

        return objectsInformation;
    }
}
