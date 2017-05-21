using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.Entities.AI
{
	public class Dreadnought : Entity
	{
		public const string _name = "Dreadnought";
		public Guid _id = Guid.NewGuid();
		public const string _icon = "N";
		public const int _hitPoints = 5;
		public const int _attackPower = 2;
		public const int _movementPoint = 1;
		public const int _attackRange = 1;
		public const EntityType _type = EntityType.Dreadnought;
		public const EntityControlType _controlType = EntityControlType.Ai;
		public const EntityMovementType _movementType = EntityMovementType.OmniDirection;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.None;
		public const EntityAttackType _attackType = EntityAttackType.OmniDirection;

		public Dreadnought()
		{
			Name = _name;
			ID = _id;
			Icon = _icon;
			HitPoints = _hitPoints;
			AttackPower = _attackPower;
			MovementPoint = _movementPoint;
			AttackRange = _attackRange;
			Type = _type;
			ControlType = _controlType;
			MovementType = _movementType;
			MovementPattern = _movementPattern;
			AttackType = _attackType;
		}

		public override void Attack()
		{
			base.Attack();
		}

		public override void Move()
		{
			base.Move();
		}
	}
}
