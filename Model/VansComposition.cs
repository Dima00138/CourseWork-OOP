using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    [OracleCustomTypeMapping("MANAGER.VANS_COMPOSITION)")]
    public class VansComposition : IOracleCustomType, INullable
    {
        private bool isNull = false;
        private decimal[] values;

        [OracleArrayMapping]
        public decimal[] Values
        {
            get { return values; }
            set
            {
                if (value != null && value.Length > 15)
                {
                    throw new ArgumentException("The VANS_COMPOSITION array can have at most 15 elements.");
                }
                values = value;
            }
        }

        public virtual void FromCustomObject(OracleConnection con, object pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, Values);
        }

        public virtual void ToCustomObject(OracleConnection con, object pUdt)
        {
            Values = (decimal[])OracleUdt.GetValue(con, pUdt, 0);
        }

        public virtual bool IsNull => isNull;

        public static VansComposition Null => new VansComposition { isNull = true };
    }
}
