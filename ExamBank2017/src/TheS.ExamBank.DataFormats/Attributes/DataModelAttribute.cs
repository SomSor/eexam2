using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheS.ExamBank.DataFormats.Attributes
{
    /// <summary>
    /// Marker attribute for DataModel, won't use anywhere!
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class DataModelAttribute : Attribute
    {
        public DataModelAttribute()
        {
        }

        public bool Root { get; set; }
    }
}
