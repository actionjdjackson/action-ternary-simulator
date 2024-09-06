using System;
namespace ActionTernarySimulator
{
    public class TernaryNumGeneric
    {

        protected string fullTernaryString;

        public TernaryNumGeneric()
        {
            
        }

        public string FullTernaryString { get => fullTernaryString; set => ChangeValue(); }

        public virtual void ChangeValue()
        {

        }

        public virtual void Invert()
        {

        }
    }
}
