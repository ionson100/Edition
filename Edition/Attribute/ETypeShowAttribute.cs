using System;

namespace Edition.Attribute
{


     /// <summary>
     ///Show type edition
     /// </summary>
     [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ETypeShowAttribute:System.Attribute
    {
         /// <summary>
         /// tabs or accordion
         /// </summary>
         public TypeShow Type { get; set; }

         /// <summary>
         /// ctor.
         /// </summary>
         /// <param name="type">Тип показа</param>
         public ETypeShowAttribute(TypeShow type)
        {
            Type = type;
        }
    }
}
