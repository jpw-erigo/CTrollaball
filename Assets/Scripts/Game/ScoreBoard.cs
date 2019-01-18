﻿/*
Copyright 2019 Cycronix
 
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
 
    http://www.apache.org/licenses/LICENSE-2.0
 
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

// ScoreBoard:  maintain and display score (stats) for collision/interactions

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreBoard : MonoBehaviour
{
    private CTunity ctunity;
    private CTclient ctclient;
//    private CTledger ctledger;

    public int HP = 10;        // max hits before killed
    public int ATK = 1;         // amount of damage
    public int AC = 1;          // damage mitigation
    public Boolean showHP = true;

    // Use this for initialization
    void Start()
    {
        ctunity = GameObject.Find("CTunity").GetComponent<CTunity>();
        ctclient = GetComponent<CTclient>();

        //       if (showHP) int.TryParse(ctclient.getCustom("HP", HP + ""), out HP);
        if (showHP)
        {
            HP = ctclient.getCustom("HP", 0);
//            Debug.Log(CTunity.fullName(gameObject)+": startup Custom: " + ctclient.custom);
            if (HP == 0) showHP = false;        // enabled by startup JSON having HP custom field
        }
    }

    //----------------------------------------------------------------------------------------------------------------
    // show HP bar above object 
    void OnGUI()
    {
        if (showHP && ctunity.trackEnabled)
        {
            Vector2 targetPos = Camera.main.WorldToScreenPoint(transform.position);
            int w = 30;
            w = ctclient.custom.Length * 7 + 12;
            int h = 24;
   //         GUI.Box(new Rect(targetPos.x - w / 2, Screen.height - targetPos.y - 2 * h, w, h), HP + "");
            GUI.Box(new Rect(targetPos.x - w / 2, Screen.height - targetPos.y - 2*h, w, h), ctclient.custom);
   //         GUI.Box(new Rect(targetPos.x - w / 2, Screen.height - targetPos.y - 2 * h, w, h), new GUIContent(HP + "", ctclient.custom));
        }
    }

    private void Update()
    {
        //        if(showHP) int.TryParse(ctclient.getCustom("HP", HP + ""), out HP);
        if (showHP) HP = ctclient.getCustom("HP", HP);
    }

    //----------------------------------------------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
//        Debug.Log(name + ", Trigger with: " + other.name);
        doCollision(other);
    }

    void OnCollisionEnter(Collision collision)
    {
 //       Debug.Log(name + ", Collision with: " + collision.collider.name);
        doCollision(collision.collider);
    }

    //----------------------------------------------------------------------------------------------------------------
    void doCollision(Collider other)
    {
        if (!showHP) return;                                        // no game

        String myName = CTunity.fullName(gameObject);
        String otherName = CTunity.fullName(other.gameObject);
//        Debug.Log(myName + ", collide with: " + otherName);

        if(ctunity == null) ctunity = GameObject.Find("CTunity").GetComponent<CTunity>();
        if (other.gameObject == null || ctunity == null)
        {
            Debug.Log(name + ": OnTrigger null other object: "+other.name);
            return;
        }

        // compare hit levels to see who wins
        int otherATK = 0;
        ScoreBoard kso = other.gameObject.GetComponent<ScoreBoard>();      
        if (kso != null)                                                    // an opponent!
        {
            otherATK = kso.ATK;
            //            Debug.Log(myName+".ATK: " + ATK + ", "+ otherName + ".ATK: " + otherATK);
            if (ctunity.activePlayer(gameObject) && !ctunity.localPlayer(other.gameObject))
 //               if ((ATK < otherATK) && ctunity.activePlayer(gameObject) && !ctunity.localPlayer(other.gameObject))
            {
                HP = ctclient.getCustom("HP", HP);

                //              HP -= (otherATK - ATK);
                int damage = (int)Math.Ceiling((float)otherATK / (float)AC); 
                HP -= damage;
                Debug.Log(myName + ": HIT by: " + otherName + ", AC: "+ AC+", otherATK: "+otherATK+", damage: "+damage);

                if (HP < 0) HP = 0;
  //              ctclient.putCustom("HP",""+HP);
                ctclient.putCustom("HP", HP);
                if (HP <= 0)
                {
                    // Debug.Log(name + ": killed!");
                    ctunity.clearObject(gameObject, false);  // can't destroyImmediate inside collision callback
                }
            }
        }
    }
}