using HappyTravel.Hiroshima.Common.Models.Enums;

namespace HappyTravel.Hiroshima.Common.Models
{
    public class ManagerServiceSupplierRelation
    {
        public int ManagerId { get; set; }
        public ManagerPermissions ManagerPermissions { get; set; }
        public int ServiceSupplierId { get; set; }
        public bool IsMaster { get; set; }
        public bool IsActive { get; set; }


        public override bool Equals(object obj) => obj is ManagerServiceSupplierRelation other && Equals(other);


        public bool Equals(ManagerServiceSupplierRelation other)
            => Equals((ManagerId, ManagerPermissions, ServiceSupplierId, IsMaster),
                (other.ManagerId, other.ManagerPermissions, other.ServiceSupplierId, other.IsMaster));


        public override int GetHashCode() => (ManagerId, ManagerPermissions, ServiceSupplierId, IsMaster).GetHashCode();
    }
}