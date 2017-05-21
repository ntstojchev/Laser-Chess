﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.Entities.Human
{
	public class Jumpship : Entity
	{
		public const string _name = "Jumpship";
		public const string _icon = "J";
		public const int _hitPoints = 2;
		public const int _attackPower = 2;
		public const int _movementPoint = 0;
		public const int _attackRange = 1;
		public const EntityType _type = EntityType.Jumpship;
		public const EntityControlType _controlType = EntityControlType.Human;
		public const EntityMovementType _movementType = EntityMovementType.Orthogonally;
		public const EntitySpecialMovementPattern _movementPattern = EntitySpecialMovementPattern.Knight;
		public const EntityAttackType _attackType = EntityAttackType.Orthogonally;

		public Jumpship()
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
