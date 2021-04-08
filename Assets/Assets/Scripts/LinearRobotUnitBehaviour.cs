using System;
using System.Collections;
using UnityEngine;

public class LinearRobotUnitBehaviour : RobotUnit
{
    public float resourceValue;
    public float blockValue;
    public float resouceAngle;
    public float blockAngle;
    public int modeR = 1, modeB = 1; //1-> linear, 2-> gauss, 3-> log
    public float weightPercentageResources = 100f;
    public float b_resources = 0.25f, b_blocks = 0.25f;
    public float limiar_min_resources = -2f, limiar_max_resources = 2f, limiar_min_blocks = -2f, limiar_max_blocks = 2f;
    public float lim_min_blocks = -2f, lim_max_blocks = 2f, lim_min_resources = -2f, lim_max_resources = 2f;
    public float multiplier_resources = 1f, multiplier_blocks = 1f;
    public float exponent_resources = 1f, exponent_blocks = 1f;
    public float add_resources = 0f, add_blocks = 0f;
    public float shift_resources = 0f, shift_blocks = 0f;
    public float mu_resources = 0.5f, mu_blocks = 0.5f;
    public float sigma_resources = 0.12f, sigma_blocks = 0.12f;
    public float dilate_resources = 1f, dilate_blocks = 1f;
    void Update()
    {
        weightPercentageResources = Math.Abs(weightPercentageResources);
        if (weightPercentageResources > 200)
        {
            weightPercentageResources = 200;
        }

        if (limiar_min_blocks > limiar_max_blocks)
        {
            float temp = limiar_min_blocks;
            limiar_min_blocks = limiar_max_blocks;
            limiar_max_blocks = temp;
        }
        if (lim_min_resources > lim_max_resources)
        {
            float temp = lim_min_blocks;
            lim_min_blocks = lim_max_blocks;
            lim_max_blocks = temp;
        }
        if (lim_min_resources < -2)
        {
            lim_min_resources = -2;
        }
        if (lim_max_resources > 2)
        {
            lim_max_resources = 2;
        }
        if (lim_min_blocks < -2)
        {
            lim_min_blocks = -2;
        }
        if (lim_max_blocks > 2)
        {
            lim_max_blocks = 2;
        }
        b_resources = Math.Abs(b_resources); b_blocks = Math.Abs(b_blocks);
        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();
        blockAngle = blockDetector.GetAngleToClosestObstacle();
        if (modeR == 1)
        {
            resourceValue = resourcesDetector.GetLinearOuput(exponent_resources, multiplier_resources, add_resources, shift_resources, dilate_resources);
        }
        else if (modeR == 2)
        {
            resourceValue = multiplier_resources * (float)Math.Pow(resourcesDetector.GetGaussianOutput(mu_resources, sigma_resources, shift_resources), exponent_resources) + add_resources;
        }
        else if (modeR == 3)
        {
            resourceValue = multiplier_resources * (float)Math.Pow(resourcesDetector.GetLogaritmicOutput(b_resources, shift_resources), exponent_resources) + add_resources;
        }
        else if (modeR == 4)
        {
            resourceValue = multiplier_resources * (float)Math.Pow(resourcesDetector.GetTrigonometricOutput(shift_resources, dilate_resources), exponent_resources) + add_resources;
        }
        if (modeB == 1)
        {
            blockValue = blockDetector.GetLinearOuput(exponent_blocks, multiplier_blocks, add_blocks, shift_blocks, dilate_blocks);
        }
        else if (modeB == 2)
        {
            blockValue = multiplier_blocks * (float)Math.Pow(blockDetector.GetGaussianOutput(mu_blocks, sigma_blocks, shift_blocks), exponent_blocks) + add_blocks;
        }
        else if (modeB == 3)
        {
            blockValue = multiplier_blocks * (float)Math.Pow(blockDetector.GetLogaritmicOutput(b_blocks, shift_blocks), exponent_blocks) + add_blocks;
        }
        else if (modeB == 4)
        {
            blockValue = multiplier_blocks * (float)Math.Pow(blockDetector.GetTrigonometricOutput(shift_blocks, dilate_blocks), exponent_blocks) + add_blocks;
        }
        // apply to the ball
        if (resourceValue < lim_min_resources || resourcesDetector.strength < limiar_min_resources || resourcesDetector.strength > limiar_max_resources)
        {
            resourceValue = lim_min_resources;
        }
        else if (resourceValue > lim_max_resources)
        {
            resourceValue = lim_max_resources;
        }
        
        if (blockValue < lim_min_blocks || blockDetector.strength < limiar_min_blocks || blockDetector.strength > limiar_max_blocks)
        {
            blockValue = lim_min_blocks;
        }
        else if (blockValue > lim_max_blocks)
        {
            blockValue = lim_max_blocks;
        }
        if(resourcesDetector.strength < limiar_min_resources || resourcesDetector.strength > limiar_max_resources)
        {
            if(lim_min_resources < 0)
            {
                resourceValue = 0;
            }
        }
        if (blockDetector.strength < limiar_min_blocks || blockDetector.strength > limiar_max_blocks)
        {
            if (lim_min_blocks > 0)
            {
                blockValue = 0;
            }
        }
        print("x =" + resourcesDetector.strength + "\ty= " + resourceValue);
        applyForce(resouceAngle, resourceValue * (weightPercentageResources / 100)); // go towards
        applyForce(blockAngle, blockValue * ((200 - weightPercentageResources) / 100)); // go towards


    }


}






