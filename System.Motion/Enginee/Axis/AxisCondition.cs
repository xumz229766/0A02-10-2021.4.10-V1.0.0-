using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Enginee
{
    class AxisCondition
    {
        private readonly Func<bool> _offcondition;
        private readonly Func<bool> _oncondition;
        public AxisCondition(Func<bool> offCondition, Func<bool> onCondition)
        {
            _offcondition = offCondition;
            _oncondition = onCondition;
            External = new External();
        }

        /// <summary>
        /// 外部信号
        /// </summary>
        public External External { get; set; }
        /// <summary>
        /// 为OFF条件
        /// </summary>
        public bool IsOffCondition
        {
            get
            {
                try
                {
                    return _offcondition();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 为ON条件
        /// </summary>
        public bool IsOnCondition
        {
            get
            {
                try
                {
                    return _oncondition();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
