using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.Entities.AI
{
	public class CommandUnit : Entity
	{
		public const string _name = "CommandUnit";
		public Guid _id = Guid.NewGuid();
		public const string _icon = "C";
		public const int _hitPoints = 5;
		public const int _attackPower = 0;
		public const int _movementPoint = 1;
		public const int _attackRange = 0;
		public const EntityType _type = EntityType.CommandUnit;
		public const EntityControlType _controlType = EntityControlType.Ai;
		public const EntityMovementType _movementType = EntityMovementType.Orthogonally;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.None;
		public const EntityAttackType _attackType = EntityAttackType.None;

		public CommandUnit()
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
