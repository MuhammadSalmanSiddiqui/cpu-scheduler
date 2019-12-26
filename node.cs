using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingSystems_Scheduler
{
    class node
    {
        private Process data;
        private node next;
        private node previous;
        public node() {next = null;}
        public node(Process d) { data = d; next = null; } 
	    public Process getData() {return data;}
        public void setData(Process d) { data = d; }
        public node getNext() { return next; }
        public void setNext(node n) { next = n; }
        public node getPrevious() { return previous; }
        public void setPrevious(node n) { previous = n; }
    }
}
