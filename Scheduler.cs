using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingSystems_Scheduler
{
    class Scheduler
    {
        private linkedlist processes = new linkedlist();

        public Scheduler(){}

        public linkedlist getProcesses() { return processes; }

        public Scheduler sort(string type)
        {
            if (processes.getChain() == processes.getTail())
            {
                return this;
            }
            Scheduler left = new Scheduler();
            Scheduler right = new Scheduler();
            Scheduler result = new Scheduler();
            node p = processes.getChain();
            node t = processes.getTail();
            while (p != t && p.getNext() != t)
            {
                left.processes.addtoEnd(p.getData());
                right.processes.add(t.getData());
                p = p.getNext();
                t = t.getPrevious();
            }
            if (p == t)
                right.processes.add(t.getData());
            else if (p.getNext() == t)
            {
                left.processes.addtoEnd(p.getData());
                right.processes.add(t.getData());
            }
            return merge(left.sort(type), right.sort(type), type);
        }

        public void add(Process p)
        {
            processes.addtoEnd(p); 
        }

        public Process searchByName(string name)
        {
            if (processes.getChain() == processes.getTail())
            {
                if (processes.getChain().getData().getName() == name)
                    return processes.getChain().getData();
                else
                {
                    Process empty = new Process();
                    empty.setDuration(-1);
                    return empty;
                }
            }
            Scheduler p1 = new Scheduler();
            p1 = this.sort("name");
            Scheduler left = new Scheduler();
            Scheduler right = new Scheduler();
            node p = p1.processes.getChain();
            node t = p1.processes.getTail();
            while (p != t && p.getNext() != t)
            {
                left.processes.addtoEnd(p.getData());
                right.processes.add(t.getData());
                p = p.getNext();
                t = t.getPrevious();
            }
            if (p == t)
                right.processes.add(t.getData());
            else if (p.getNext() == t)
            {
                left.processes.addtoEnd(p.getData());
                right.add(t.getData());
            }
            if (right.processes.getChain().getData().getName() == name)
                return right.processes.getChain().getData();
            else if (string.Compare(right.processes.getChain().getData().getName(), name) > 0)
                return left.searchByName(name);
            else
                return right.searchByName(name); 
        }

        public void remove(string name)
        {
            if (processes.getChain() != null && processes.getChain().getData().getName() == name)
            {
                node  temp = processes.getChain();
                processes.setChain(processes.getChain().getNext());
                temp = new node();
                if (processes.getChain() == null) processes.setTail(null);
                return;
            }
            node p1 = processes.getChain();
            node p2 = processes.getChain().getNext();
            node p3 = new node();
            if (p2.getNext() != null)
                p3 = p2.getNext();
            while (p2 != null)
            {
                if (p2.getData().getName() == name)
                {
                    p1.setNext(p2.getNext());
                    p3.setPrevious(p2.getPrevious());
                    p2 = new node();
                    break;
                }
                p1 = p1.getNext();
                p2 = p2.getNext();
            }
        }

	    
        public Scheduler merge(Scheduler l1,Scheduler l2,string type)
        {
            Scheduler merged = new Scheduler();
	        node p1 =l1.processes.getChain();
	        node p2 = l2.processes.getChain();
	        if (type=="arrival")
		        while(p1 !=null && p2 != null)
		        {
			        if(p1.getData().getArrival() <= p2.getData().getArrival())
			        {
				        merged.processes.addtoEnd(p1.getData());
				        p1 =p1.getNext();
			        }
			        else
			        {
				        merged.processes.addtoEnd(p2.getData());
				        p2 = p2.getNext();
			        }
		        }
	        else if (type == "duration")
		        while(p1 !=null && p2 != null)
		        {
			        if(p1.getData().getDuration() <= p2.getData().getDuration())
			        {
			        merged.processes.addtoEnd(p1.getData());
			        p1 =p1.getNext();
			        }
		        else
			        {
			        merged.processes.addtoEnd(p2.getData());
			        p2 = p2.getNext();
			        }
		        }
	        else if (type == "priority")
		        while(p1 !=null && p2 != null)
		        {
			        if(p1.getData().getPriority() <= p2.getData().getPriority())
			        {
			        merged.processes.addtoEnd(p1.getData());
			        p1 =p1.getNext();
			        }
		        else
			        {
			        merged.processes.addtoEnd(p2.getData());
			        p2 = p2.getNext();
			        }
		        }
	        if(p1 != null)
		        while(p1 !=null)
		        {
			        merged.processes.addtoEnd(p1.getData());
			        p1 =p1.getNext();
		        }
	        if(p2 != null)
		        while(p2 != null)
		        {
			        merged.processes.addtoEnd(p2.getData());
			        p2 = p2.getNext();
		        }
		        return merged;
        }

        public void clear()
        {
            while (processes.getChain() != null)
                remove(processes.getChain().getData().getName());

        }
    }
}
