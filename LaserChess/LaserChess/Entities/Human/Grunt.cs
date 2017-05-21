using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.Entities.Human
{
	public class Grunt : Entity
	{
		public const string _name = "Grunt";
		public Guid _id = Guid.NewGuid();
		public const string _icon = "G";
		public const int _hitPoints = 2;
		public const int _attackPower = 1;
		public const int _movementPoint = 1;
		public const int _attackRange = 0;
		public const EntityType _type = EntityType.Grunt;
		public const EntityControlType _controlType = EntityControlType.Human;
		public const EntityMovementType _movementType = EntityMovementType.Orthogonally;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.None;
		public const EntityAttackType _attackType = EntityAttackType.Diagonally;

		public Grunt()
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
