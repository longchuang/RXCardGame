// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class RX_CardManager
	{
		#region
		private RX_CardManager ()
		{
		}

		private static RX_CardManager s_RX_CardManager = null;

		public static RX_CardManager DefaultManager()
		{
			
			if(s_RX_CardManager == null)
			{
				s_RX_CardManager = new RX_CardManager();
			}
			return s_RX_CardManager;
			
		} 

		#endregion


		/// <summary>
		/// Get A New CardL
		/// </summary>
	
		private List<int> GetNewCardList()
		{
			List<int> cl = new List<int> ();

			for (int i = 0; i < RX_Define.RX_CARD_NUMBER; i++) 
			{
				cl.Add(i);
			}

			return cl;
		}

		public List<RX_Card> Reshuffle()
		{
			//get a new card list
			List<int> the_list = GetNewCardList ();

			//create a shuffle card list
			List<RX_Card> res_list = new List<RX_Card> ();


			Random random = new Random ();
			for (int i = 0; i < RX_Define.RX_CARD_NUMBER; i++) 
			{
				//create a random index within the_list

				int random_index = random.Next(0,the_list.Count);

				//take the random index value into res_list
				res_list.Add(new RX_Card(the_list[random_index]));

				//remove the random index value from the_list
				the_list.RemoveAt(random_index);
			}

			return res_list;
		}


		#region CareSpriteList

		/// <summary>
		/// 精灵池...
		/// </summary>
		private static List<UISprite> bottom_sprite_pool = null;
		private static List<UISprite> BottomPool{
			get{
				if (bottom_sprite_pool == null) {
					bottom_sprite_pool = new List<UISprite> ();
				}
				return bottom_sprite_pool;
			}
		}
		private static List<UISprite> left_sprite_pool = null;
		private static List<UISprite> LeftPool{
			get{
				if (left_sprite_pool == null) {
					left_sprite_pool = new List<UISprite> ();
				}
				return left_sprite_pool;
			}
		}
		private static List<UISprite> right_sprite_pool = null;
		private static List<UISprite> RightPool{
			get{
				if (right_sprite_pool == null) {
					right_sprite_pool = new List<UISprite> ();
				}
				return right_sprite_pool;
			}
		}	
		private static List<UISprite> top_sprite_pool = null;
		private static List<UISprite> TopPool{
			get{
				if (top_sprite_pool == null) {
					top_sprite_pool = new List<UISprite> ();
				}
				return top_sprite_pool;
			}
		}
		private static List<UISprite> GetPool(RX_SEAT_POSITION pos)
		{
			List<UISprite> pool = null;
			switch (pos) {
			case RX_SEAT_POSITION.RX_SEAT_LEFT:
				{
					pool = LeftPool;
					break;
				}
			case RX_SEAT_POSITION.RX_SEAT_BOTTOM:
				{
					pool = BottomPool;
					break;
				}
			case RX_SEAT_POSITION.RX_SEAT_RIGHT:
				{
					pool = RightPool;
					break;
				}
			case RX_SEAT_POSITION.RX_SEAT_TOP:
				{
					pool = TopPool;
					break;
				}
			}

			return pool;
		}
		/// <summary>
		/// 通过一张牌对象和容器对象还有坐标x创建一张图片
		/// </summary>
		/// <returns>The sprite by.</returns>
		/// <param name="card">Card.</param>
		/// <param name="parent">Parent.</param>
	
		public struct Rect
		{
			public float x;
			public float y;
			public float width;
			public float heitht;
		}

		static Rect GetRect(int p1,float p2,RX_SEAT_POSITION pos)
		{
			Rect rect = new Rect ();

			if (pos == RX_SEAT_POSITION.RX_SEAT_LEFT || pos == RX_SEAT_POSITION.RX_SEAT_RIGHT) {
				rect.y = (float)p1;
				rect.x = (float)p2;
			} else {
				rect.x = (float)p1;
				rect.y = (float)p2;
			}

			rect.width = 42f;
			rect.heitht = 60f;

			return rect;
		}

		public static UISprite CreateSpriteBy(RX_Card card,UISprite parent,int pp,RX_SEAT_POSITION pos)
		{			
			//创建精灵对象,通过NGUITools.AddChild函数
			UISprite sprite = NGUITools.AddChild<UISprite> (parent.gameObject);

			//为图片对象添加碰撞器...
			NGUITools.AddWidgetCollider (sprite.gameObject);

			//为一个NGUI的游戏控件,添加缺失的组件
			UIButton button = NGUITools.AddMissingComponent<UIButton> (sprite.gameObject);
	
			sprite.depth = pp + 300;
			sprite.atlas = RX_Resources.DefaultResources.CardAtlas;
			sprite.spriteName = card.ToString ();

			Rect sprite_rect = GetRect (pp,card.PositionY, pos);
			//设置NGUI控件在屏幕上的显示位置和大小
			sprite.SetRect (sprite_rect.x, sprite_rect.y, sprite_rect.width, sprite_rect.heitht);

			button.onClick.Add (new EventDelegate (() => {

				card.IsPop = !card.IsPop;
				Rect sr = GetRect (pp,card.PositionY, pos);
				sprite.SetRect(sr.x,sr.y,sr.width,sr.heitht);
				
			}));
				
			//在CreateSpriteBy函数实现的最后
			//将创建的精灵对象添加到sprite池内
			GetPool(pos).Add(sprite);

			return sprite;
		}

		/// <summary>
		/// 将数组内的所有游戏对象全部销毁,并且移除数组...
		/// </summary>
		public static void RefreshPool(RX_SEAT_POSITION pos)
		{
			List<UISprite> pool = GetPool (pos);
			for (int i = 0; i < pool.Count; i++) 
			{
				//销毁游戏对象
				UnityEngine.GameObject.Destroy (pool [i].gameObject);
			}
			//从数组内移除所有的游戏对象
			pool.RemoveRange (0, pool.Count);

		}

		#endregion

	}
}

