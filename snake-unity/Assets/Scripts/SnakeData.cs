using UnityEngine;

namespace Snake
{
	public class SnakeData {
		public int x;
		public int y;
		public SType type; 

		public SnakeData(SType type,int x,int y){
			this.type = type;
			this.x = x;
			this.y = y;
		}
		public SnakeData(){
			this.type = SType.SNAKE;
			this.x = 0;
			this.y = 0;
		}

		public enum SType{
			SNAKE,
			FOOD,
		}
	}
}

