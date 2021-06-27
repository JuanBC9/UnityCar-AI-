﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSphereScript : MonoBehaviour
{
    public BotController controller;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Wall"))
        {
            controller.gen.addFitness(-1);
        }
    }
}
