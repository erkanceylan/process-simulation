using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Homework_1
{
    public class Process
    {
        //Process'in durumlarını tutan enum.
        public enum Status { New, Ready, Running, Waiting, Terminated };
        //Process'in actionlarını tutan enum.
        public enum Action {ProcessCreated,ProcessAdmitted,SchedulerPatcher,Interrupt,IOEvent,Exit,IOEventCompletion }
        
        private Status status;//Process nesnesinin o anki durumu.
        public Status getStatus { get { return status; } }
        private Action action;//Process nesnesinin o anki action'ı.
        public Action getAction { get { return action; } }

        //Constructor
        public Process()
        {
            //Process ilk oluşturulduğunda alması gereken değerler tanımlanır.
            status = Status.New;
            action = Action.ProcessCreated;
        }

        //Process'in o anki durumuna göre random hareketi sağlayan method.
        public void MoveOnRandomly()
        {
            switch (status)
            {
                case Status.New:
                    Move(GetRandomNumber(1));
                    break;
                case Status.Ready:
                    Move(GetRandomNumber(1));
                    break;
                case Status.Running:
                    Move(GetRandomNumber(3));
                    break;
                case Status.Waiting:
                    Move(GetRandomNumber(1));
                    break;
                case Status.Terminated:
                    break;
                default:
                    break;
            }
        }

        //Her durum için ayrı random hareket numarası döndüren method.
        private int GetRandomNumber(int count)
        {
            int number;
            Random rand = new Random();
            number = rand.Next(count);
            return number;
        }

        //Geri döndürülen harekete göre durum ve action değişkenini ayarlayan method.
        private void Move(int move)
        {
            switch (status)
            {
                case Status.New:
                    status = Status.Ready;
                    action = Action.ProcessAdmitted;
                    break;
                case Status.Ready:
                    status = Status.Running;
                    action = Action.SchedulerPatcher;
                    break;
                case Status.Running:
                    if(move==0)
                    {
                        status = Status.Waiting;
                        action = Action.IOEvent;
                    }
                    else if(move==1)
                    {
                        status = Status.Ready;
                        action = Action.Interrupt;
                    }
                    else if(move==2)
                    {
                        status = Status.Terminated;
                        action = Action.Exit;
                    }
                    break;
                case Status.Waiting:
                    status = Status.Ready;
                    action = Action.IOEventCompletion;
                    break;
                case Status.Terminated:
                    break;
            }
        }

    }

    
}
