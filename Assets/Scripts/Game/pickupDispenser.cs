﻿/*
Copyright 2018 Cycronix
 
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pickupDispenser : MonoBehaviour, IPointerDownHandler  {
	private CTunity ctunity;

	private static int nobject = 0;
	public int pickupsPerClick = 5;
	public int maxPickups = 100;
	public Vector3 groundPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		ctunity = GameObject.Find("CTunity").GetComponent<CTunity>();           // reference CTunity script
	}

	//----------------------------------------------------------------------------------------------------------------
	public void OnPointerDown(PointerEventData eventData)
	{
		if (Input.GetMouseButton(0))
		{
			if (ctunity.showMenu) return;          // notta if changing settings...
//			Debug.Log("onpointerdown dispense pickups...");
			PlayerObjects ctplayer = GameObject.Find(ctunity.Player).GetComponent<PlayerObjects>();     // reference Player gameobject spawner
			ctplayer.dispensePickups();
		}
	}
    
}
