using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OperatingSystems_Scheduler
{
    class Process
    {
	        private string name;
	        private double arrival;
	        private double duration;
	        private int priority;            
            private Color color = new Color();

            public Process() 
            {
                name = ""; 
                arrival = 0.0; 
                duration = 0.0; priority = 0;
                color = System.Drawing.Color.Black;
            }

	        public Process(string n,double a,double d,int p){name =n;arrival=a;duration =d;priority =p;}
            public void setName(string n) { name = n; }
            public void setColor(Color c) { color = c; }
            public void setArrival(double a) { arrival = a; }
            public void setDuration(double d) { duration = d; }
            public void setPriority(int p) { priority = p; }
            public string getName() { return name; }
            public Color getColor() { return color; }
            public double getArrival() { return arrival; }
            public double getDuration() { return duration; }
            public int getPriority() { return priority; }
    }
}
