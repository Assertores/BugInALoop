using System.Collections;
using System.Collections.Generic;
using BIAL.Entities;
using BIAL.Runtime.Interfaces;
using UnityEngine;

namespace BIAL.Runtime.DataStorage
{
	public class EntityConfig : ScriptableObject
	{
		public float DirectorCreditCost = 1;
		public GameObject EntityView;
		public EntityFactory Factory;
	}
}
