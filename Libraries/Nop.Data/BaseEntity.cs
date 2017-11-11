using System;

namespace Nop.Data
{
    [Serializable]
    public abstract class BaseEntity<TId>
    {
        public virtual TId Id { get; set; }
        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity<TId>);
        }

        public virtual bool IsTransient()
        {
            return Id == null || Id.Equals(default(TId));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity<TId> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;
            return other != null
               && HasSameNonDefaultIdAs(other)
               && HasSameBusinessSignatureAs(other);
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(TId)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }
        private bool HasSameBusinessSignatureAs(BaseEntity<TId> compareTo)
        {
            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>  
        /// Returns true if self and the provided domain   
        /// object have the same ID values and the IDs   
        /// are not of the default ID value  
        /// </summary>  
        private bool HasSameNonDefaultIdAs(BaseEntity<TId> compareTo)
        {
            return IsTransient() || compareTo.IsTransient() || Id.Equals(compareTo.Id);
        }
        public static bool operator ==(BaseEntity<TId> x, BaseEntity<TId> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity<TId> x, BaseEntity<TId> y)
        {
            return !(x == y);
        }


    }
    [Serializable]
    public abstract class BaseEntity : BaseEntity<int>
    {

    } 
}
