using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserChess.Entities
{
	public abstract class Entity
	{
		public string Name { get; set; }

		public Guid ID { get; set; }

		public string Icon { get; set; }

		public int HitPoints { get; set; }

		public int AttackPower { get; set; }

		public int MovementPoint { get; set; }

		public int AttackRange { get; set; }

		public EntityType Type { get; set; }

		public EntityControlType ControlType { get; set; }

		public EntityMovementType MovementType { get; set; }

		public EntitySpecialMovementPattern MovementPattern { get; set; }

		public EntityAttackType AttackType { get; set; }

		public virtual void Attack()
		{
			throw new NotImplementedException();
		}

		public virtual void Move()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return Icon;
		}
	}
}
