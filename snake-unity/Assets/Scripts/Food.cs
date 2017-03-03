using System;
using UnityEngine;

namespace Snake
{
	public class Food :MonoBehaviour
	{
		public int x;
		public int y;

		public Food(int x,int y){
			this.x = x;
			this.y = y;
		}
	}

	public class Pos{
		public int x;
		public int y;

		public Pos(int x,int y){
			this.x = x;
			this.y = y;
		}
	}
}

