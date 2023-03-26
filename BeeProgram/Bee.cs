using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeProgram
{
    abstract class Bee 
    {
        public abstract float CostPerShift
        {
            get;
        }
        public string Job 
        { 
            get; private set; 
        }
        public Bee(string job) 
        {
            Job = job;
        }
        public void WorkTheNextShift()
        {
            if(HoneyVault.ConsumeHoney(CostPerShift) == true)
            {
                DoJob();
            }
            
        }

        protected abstract void DoJob();
    }

    class Queen : Bee
    {
        private float eggs = 0;
        private float unassignedWorkers = 3;
        private Bee[] workers = new Bee[0];
        public override float CostPerShift { get { return 2.15f; } }
        public const float EGGS_PER_SHIFT = 0.45f;
        public const float HONEY_PER_UNASSIGNED_WORKER = 0.5f;
        public string StatusReport { get; private set;
        }
        public Queen() : base("Królowa")
        {
            AssignBee("Opiekunka Jaj");
            AssignBee("Zbieraczka nektaru");
            AssignBee("Producentka miodu");
        }
        public void AssignBee(string job)
        {
            switch (job)
            {
                case "Opiekunka Jaj":
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

        protected override void DoJob()
        {
            eggs += EGGS_PER_SHIFT;

            foreach (Bee worker in workers)
            {
                worker.WorkTheNextShift();
            }
            HoneyVault.ConsumeHoney(HONEY_PER_UNASSIGNED_WORKER * unassignedWorkers);
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
                eggs -= eggsToConvert;
                unassignedWorkers += eggsToConvert;
            }
        }

        // Tworze metode do generowania raportów
        private void UpdateStatusReport()
        {
            StatusReport = $"Raport o stanie skarbca:\n" +
                $"{HoneyVault.StatusReport}\n" +
                $"\n" +
                $"Liczba jaj: {eggs:0.0}\n" +
                $"Nieprzydzielone robotnice:{unassignedWorkers:0.0}\n" +
                $"Zbieraczka nektaru: {WorkerStatus("Zbieraczka nektaru")}\n" +
                $"Producentka miodu: {WorkerStatus("Producentka miodu")}\n" +
                $"Opiekunka jaj: {WorkerStatus("Opiekunka jaj")} \n" +
                $"ROBOTNICE W SUMIE: {workers.Length}";
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
        public const float NECTAR_PROCESSED_PER_SHIFT = 31.15f;
        public HoneyManufacturer() : base("Producentka miodu")
        {

        }
        protected override void DoJob()
        {
            HoneyVault.ConvertNectarToHoney(NECTAR_PROCESSED_PER_SHIFT);
        }

        public override float CostPerShift { get { return 1.7f; } }
    }

    class NectarCollector : Bee
    {
        public const float NECTAR_COLLECTED_PER_SHIFT = 33.25f;
        public NectarCollector() : base("Zbieraczka nektaru")
        {

        }
        protected override void DoJob()
        {
            HoneyVault.CollectNectar(NECTAR_COLLECTED_PER_SHIFT);
        }
        public override float CostPerShift { get { return 1.95f; } }

    }
    
    class EggCare : Bee
    {
        private Queen queen;
        public const float CARE_PROGRESS_PER_SHIFT = 0.15f;
        public EggCare(Queen queen) : base("Opiekunka jaj")
        {
            this.queen = queen;
        }
        protected override void DoJob()
        {
            queen.CareForEggs(CARE_PROGRESS_PER_SHIFT);
        }
        public override float CostPerShift { get { return 1.35f; } }
    }
}
