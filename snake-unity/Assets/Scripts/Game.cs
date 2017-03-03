using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snake
{
	public class Game : MonoBehaviour
	{
		[SerializeField]GameObject mSnakeGroup,mFoodGroup;
		List<SnakeData> SnakeDataList = new List<SnakeData>();

		private Direction MoveDirection;
		bool gameStart = false;
		void Start(){

			// 初始化数据;
			SnakeDataList.Clear ();
			SnakeDataList.Add (new SnakeData(SnakeData.SType.SNAKE,0,0));
			SnakeDataList.Add (new SnakeData(SnakeData.SType.SNAKE,1,0));
			SnakeDataList.Add (new SnakeData(SnakeData.SType.SNAKE,2,0));

			// 载入数据;
			UpdateSnake(SnakeDataList);

			// 初始移动;
			Move (Direction.DOWN);

			// 随机食物;
			UpdateFood (SnakeDataList);

			gameStart = true;
		}

		SnakeData GetNextSnakeData(Direction moveDir){
			var snakeData = new SnakeData ();
			switch(moveDir){
			case Direction.UP:
				snakeData.x = SnakeDataList [0].x;
				snakeData.y = SnakeDataList [0].y + 1;
				break;
			case Direction.DOWN:
				snakeData.x = SnakeDataList [0].x;
				snakeData.y = SnakeDataList [0].y - 1;
				break;
			case Direction.LEFT:
				snakeData.x = SnakeDataList [0].x - 1;
				snakeData.y = SnakeDataList [0].y;
				break;
			case Direction.RIGHT:
				snakeData.x = SnakeDataList [0].x + 1;
				snakeData.y = SnakeDataList [0].y;
				break;
			default:
				snakeData = null;
				break;
			}
			return snakeData;
		}

		Pos GetRandomFood(List<SnakeData> snakeDataList){
			int min = -GlobalVar.LENGTH/GlobalVar.EACH_LENGTH / 2+1;
			int max=GlobalVar.WIDTH/GlobalVar.EACH_LENGTH/2;
			int randomX = 0,randomY=0;
			do{
				randomX = UnityEngine.Random.Range(min,max);
				Debug.Log("randomX: "+randomX);
			}while(snakeDataList.FirstOrDefault(s=>s.x==randomX)!=null);
				
			do{
				randomY = UnityEngine.Random.Range(min,max);
				Debug.Log("randomY: "+randomY);
			
			}while(snakeDataList.FirstOrDefault(s=>s.y==randomY)!=null);

			return new Pos (randomX,randomY);
		}



		float moveTime = 0f;
		void Update(){

			if(!gameStart){
				return;
			}

			moveTime += Time.deltaTime;
			if(moveTime>1f){
				Move (MoveDirection);
			}

			if(Input.GetKeyDown(KeyCode.W)){
				Move (Direction.UP);
			}

			if(Input.GetKeyDown(KeyCode.A)){
				Move (Direction.LEFT);
			}

			if(Input.GetKeyDown(KeyCode.S)){
				Move (Direction.DOWN);
			}

			if(Input.GetKeyDown(KeyCode.D)){
			 	Move (Direction.RIGHT);
			}
		}

		void UpdateSnake(List<SnakeData> snakeDataList){

			if(CheckGameOver(snakeDataList)){
				Debug.LogError ("game over!");
				return;
			}

			// 载入数据;
			NGUITools.DestroyChildren (mSnakeGroup.transform);
			for(int i=0;i<snakeDataList.Count;++i){
				var x = snakeDataList [i].x;
				var y = snakeDataList [i].y;

				var snake = Instantiate(Resources.Load (GlobalVar.SNAKE_PATH)) as GameObject;
				snake.SetActive (true);

				snake.name = string.Format ("{0},{1},{2}",i,x,y);
				snake.transform.parent = mSnakeGroup.transform;
				snake.transform.localScale = Vector3.one;
				snake.transform.localPosition = new Vector3 (x*GlobalVar.EACH_LENGTH,y*GlobalVar.EACH_LENGTH);
			}
		}

		Food mFood = null;
		void UpdateFood(List<SnakeData> snakeDataList){
			
			var Vec = GetRandomFood (snakeDataList);

			var snake = Instantiate(Resources.Load (GlobalVar.SNAKE_PATH)) as GameObject;
			snake.SetActive (true);

			snake.name = string.Format ("Food: {0},{1}",Vec.x,Vec.y);
			snake.transform.parent = mFoodGroup.transform;
			snake.transform.localScale = Vector3.one;
			snake.transform.localPosition = new Vector3 (Vec.x*GlobalVar.EACH_LENGTH,Vec.y*GlobalVar.EACH_LENGTH);
			mFood = snake.AddComponent<Food> ();
			mFood.x = Vec.x;
			mFood.y = Vec.y;
		}

		void Move(Direction direction){
			  
			if(!CheckMoveDirectionCorrect(direction)){
				return;
			}

			this.MoveDirection = direction;
			this.moveTime = 0f;

			var nextData = GetNextSnakeData (direction);
			if(nextData!=null){
				var eatFood = (mFood != null && nextData.x == mFood.x && nextData.y == mFood.y);
				Debug.Log ("nextData: "+eatFood+ nextData.x+","+nextData.y);	
				if (!eatFood) {
					SnakeDataList.RemoveAt (SnakeDataList.Count - 1);
				} else {
					GameObject.Destroy (mFood.gameObject);
				}
				SnakeDataList.Insert (0,nextData);

				UpdateSnake (SnakeDataList);

				if(eatFood){
					UpdateFood (SnakeDataList);
				}
			}
		}

		bool CheckMoveDirectionCorrect(Direction direction){
			if(direction == Direction.UP && MoveDirection == Direction.DOWN){
				return false;
			}

			if(direction == Direction.DOWN && MoveDirection == Direction.UP){
				return false;
			}

			if(direction == Direction.LEFT && MoveDirection == Direction.RIGHT){
				return false;
			}

			if(direction == Direction.RIGHT && MoveDirection == Direction.LEFT){
				return false;
			}

			return true;
		}

		bool CheckGameOver(List<SnakeData> snakeDataList){ 

			if(snakeDataList[0].x>=GlobalVar.LENGTH/2/GlobalVar.EACH_LENGTH || snakeDataList[0].x<=-GlobalVar.LENGTH/2/GlobalVar.EACH_LENGTH){
				return true;
			}

			if(snakeDataList[0].y>=GlobalVar.WIDTH/2/GlobalVar.EACH_LENGTH || snakeDataList[0].y<=-GlobalVar.WIDTH/2/GlobalVar.EACH_LENGTH){
				return true;
			}

			return false;
		}


		const int GUI_BUTTON_DISTANCE = 60;//10;
		void OnGUI(){
			#if UNITY_EDITOR
			//15,5;
			if(GUI.Button(new Rect(GUI_BUTTON_DISTANCE+GUI_BUTTON_DISTANCE/2,GUI_BUTTON_DISTANCE/2,GUI_BUTTON_DISTANCE,GUI_BUTTON_DISTANCE),"W")){
				Move (Direction.UP);
			}
			//5,15
			if(GUI.Button(new Rect(GUI_BUTTON_DISTANCE/2,GUI_BUTTON_DISTANCE+GUI_BUTTON_DISTANCE/2,GUI_BUTTON_DISTANCE,GUI_BUTTON_DISTANCE),"A")){
				Move (Direction.LEFT);
			}
			//15,25
			if(GUI.Button(new Rect(GUI_BUTTON_DISTANCE+GUI_BUTTON_DISTANCE/2,GUI_BUTTON_DISTANCE*2+GUI_BUTTON_DISTANCE/2,GUI_BUTTON_DISTANCE,GUI_BUTTON_DISTANCE),"S")){
				Move (Direction.DOWN);
			}
			//25,15
			if(GUI.Button(new Rect(GUI_BUTTON_DISTANCE*2+GUI_BUTTON_DISTANCE/2,GUI_BUTTON_DISTANCE+GUI_BUTTON_DISTANCE/2,GUI_BUTTON_DISTANCE,GUI_BUTTON_DISTANCE),"D")){
				Move (Direction.RIGHT);
			}
			#endif
		}
	}

	public enum Direction{
		NONE,
		UP,
		DOWN,
		LEFT,
		RIGHT,
	}


}

