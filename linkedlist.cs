using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingSystems_Scheduler
{
    class linkedlist
    {
        private node chain = new node();
	    private node tail = new node();
        public linkedlist() { chain = null; tail = null; }
	    public node  getChain() {return chain;}
	    public void setChain( node n) {chain=n;}
	    public node getTail() {return tail;}
	    public void setTail( node n) {tail=n;}
	    public void add(Process data1)
        {
             node temp= new node(data1);
             temp.setNext(chain);
             if(chain != null) chain.setPrevious(temp);
             chain =temp;
             if(tail==null) tail=chain;
        }

	    public void addtoEnd(Process data1)
        {
            node temp = new node(data1);
	        if(tail != null)
	        {
		        tail.setNext(temp);
		        temp.setPrevious(tail);
	        }
	        tail=temp;
	        if(chain==null) 
		        chain=tail;
        }

        
    }
   
   
}


