using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeProgram
{
    internal class Bee 
    {
        virtual public float CostPerShift
        {
            get; private set;
        }
        public Bee(string Job) 
        {
            
        }
        void WorkTheNextShift()
        {

        }

        protected virtual void DoJob()
        {

        }
    }

    class Queen : Bee
    {
        public Queen() : base("Królowa")
        {
            
        }
    }
}
