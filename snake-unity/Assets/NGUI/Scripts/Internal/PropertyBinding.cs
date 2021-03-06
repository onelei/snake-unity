/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Property binding lets you bind two fields or properties so that changing one will update the other.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Property Binding")]
public class PropertyBinding : MonoBehaviour
{
	public enum UpdateCondition
	{
		OnStart,
		OnUpdate,
		OnLateUpdate,
		OnFixedUpdate,
	}

	public enum Direction
	{
		SourceUpdatesTarget,
		TargetUpdatesSource,
		BiDirectional,
	}

	/// <summary>
	/// First property reference.
	/// </summary>

	public PropertyReference source;

	/// <summary>
	/// Second property reference.
	/// </summary>

	public PropertyReference target;

	/// <summary>
	/// Direction of updates.
	/// </summary>

	public Direction direction = Direction.SourceUpdatesTarget;

	/// <summary>
	/// When the property update will occur.
	/// </summary>

	public UpdateCondition update = UpdateCondition.OnUpdate;

	/// <summary>
	/// Whether the values will update while in edit mode.
	/// </summary>

	public bool editMode = true;

	// Cached value from the last update, used to see which property changes for bi-directional updates.
	object mLastValue = null;

	void Start ()
	{
		UpdateTarget();
		if (update == UpdateCondition.OnStart) enabled = false;
	}

	void Update ()
	{
#if UNITY_EDITOR
		if (!editMode && !Application.isPlaying) return;
#endif
		if (update == UpdateCondition.OnUpdate) UpdateTarget();
	}

	void LateUpdate ()
	{
#if UNITY_EDITOR
		if (!editMode && !Application.isPlaying) return;
#endif
		if (update == UpdateCondition.OnLateUpdate) UpdateTarget();
	}

	void FixedUpdate ()
	{
#if UNITY_EDITOR
		if (!editMode && !Application.isPlaying) return;
#endif
		if (update == UpdateCondition.OnFixedUpdate) UpdateTarget();
	}

	void OnValidate ()
	{
		if (source != null) source.Reset();
		if (target != null) target.Reset();
	}

	/// <summary>
	/// Immediately update the bound data.
	/// </summary>

	[ContextMenu("Update Now")]
	public void UpdateTarget ()
	{
		if (source != null && target != null && source.isValid && target.isValid)
		{
			if (direction == Direction.SourceUpdatesTarget)
			{
				target.Set(source.Get());
			}
			else if (direction == Direction.TargetUpdatesSource)
			{
				source.Set(target.Get());
			}
			else if (source.GetPropertyType() == target.GetPropertyType())
			{
				object current = source.Get();

				if (mLastValue == null || !mLastValue.Equals(current))
				{
					mLastValue = current;
					target.Set(current);
				}
				else
				{
					current = target.Get();

					if (!mLastValue.Equals(current))
					{
						mLastValue = current;
						source.Set(current);
					}
				}
			}
		}
	}
}
