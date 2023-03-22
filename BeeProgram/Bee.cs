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
        public string Job 
        { 
            get; private set; 
        }
        public Bee(string Job) 
        {
            
        }
        public void WorkTheNextShift()
        {
            if(HoneyVault.ConsumeHoney(CostPerShift) == true)
            {
                DoJob();
            }
            
        }

        protected virtual void DoJob()
        {

        }
    }

    class Queen : Bee
    {
        private float eggs;
        private float unassignedWorkers;
        private Bee[] workers;
        public const float EGGS_PER_SHIFT = 0.45f;
        public const float HONEY_PER_UNASSIGNED_WORKER = 0.5f;
        public Queen() : base("Królowa")
        {
            AssignBee("Opiekunta jaj");
            AssignBee("Zbieraczka nektaru");
            AssignBee("Producentka miodu");
        }
        void AssignBee(string job)
        {
            switch (job)
            {
                case "Opiekunka jaj":
                    AddWorker(new EggCare(this));
                    break;
                case "Zbieraczka nektaru":
                    AddWorker(new NectarCollector());
                    break;
                case "Producentka miodu":
                    AddWorker(new HoneyManufacturer());
                    break;
            }
            UpdateStatusReport();
        }
        public new float CostPerShift { get { return 2.15f; } }

        protected override void DoJob()
        {
            eggs += EGGS_PER_SHIFT;

            foreach (Bee worker in workers)
            {
                WorkTheNextShift();
                HoneyVault.ConsumeHoney(HONEY_PER_UNASSIGNED_WORKER * unassignedWorkers);
            }
            UpdateStatusReport();
        }
        /// <summary>
        /// Zwiększanie tablicy workers o jedno miejsce i dodanie referencji typu Bee
        /// </summary>
        /// <param name="worker">Robotnica dodawana do tablicy</param>
        private void AddWorker(Bee worker)
        {
            if (unassignedWorkers >= 1)
            {
                unassignedWorkers--;
                Array.Resize(ref workers, workers.Length + 1);
                workers[workers.Length - 1] = worker;
            }
        }

        public void CareForEggs(float eggsToConvert)
        {
            if(eggs >= eggsToConvert)
            {
                // Tworze pomocniczą wartość helpEggs, aby uzyskać różnicę z eggs i eggsToConvert
                float helpEggs = eggs - eggsToConvert;
                eggs -= eggsToConvert;
                unassignedWorkers += helpEggs;
            }
        }

        // Tworze metode do generowania raportów
        private void UpdateStatusReport()
        {
            //HoneyVault.StatusReport;
           // WorkerStatus();

        }
        private string WorkerStatus(string job)
        {
            int count = 0;
            foreach(Bee worker in workers)
            {
                if(worker.Job == job)
                {
                    count++;
                }
            }
            return $"{job}: {count} ";
        }
    }

    class HoneyManufacturer : Bee
    {
        const float NECTAR_PROCESSED_PER_SHIFT = 31.15f;
        public HoneyManufacturer() : base("Producentka miodu")
        {

        }
        protected override void DoJob()
        {
            HoneyVault.ConvertNectarToHoney(NECTAR_PROCESSED_PER_SHIFT);
        }

        public new float CostPerShift { get { return 1.7f; } }
    }

    class NectarCollector : Bee
    {
        const float NECTAR_COLLECTED_PER_SHIFT = 33.25f;
        public NectarCollector() : base("Zbieraczka nektaru")
        {

        }
        protected override void DoJob()
        {
            HoneyVault.CollectNectar(NECTAR_COLLECTED_PER_SHIFT);
        }
        public new float CostPerShift { get { return 1.95f; } }

    }
    
    class EggCare : Bee
    {
        private Queen queen;
        const float CARE_PROGRESS_PER_SHIFT = 0.15f;
        public EggCare(Queen queen) : base("Opiekunka jaj")
        {
            this.queen = queen;
        }
        protected override void DoJob()
        {
            queen.CareForEggs(CARE_PROGRESS_PER_SHIFT);
        }
        public new float CostPerShift { get { return 1.35f; } }
    }
}
