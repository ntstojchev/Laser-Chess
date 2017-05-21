using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.Entities.Human
{
	public class Tank : Entity
	{
		public const string _name = "Tank";
		public const string _icon = "T";
		public const int _hitPoints = 4;
		public const int _attackPower = 2;
		public const int _movementPoint = 3;
		public const int _attackRange = 0;
		public const EntityType _type = EntityType.Tank;
		public const EntityControlType _controlType = EntityControlType.Human;
		public const EntityMovementType _movementType = EntityMovementType.OmniDirection;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.Queen;
		public const EntityAttackType _attackType = EntityAttackType.Orthogonally;

		public Tank()
		{
			Name = _name;
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
