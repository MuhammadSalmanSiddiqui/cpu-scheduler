using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace OperatingSystems_Scheduler
{
    public partial class CPU_Scheduler : Form
    {
        int numberOfProcesses;
        string type;
        double q;
        Button btn_st;
        TextBox quantum;
        TextBox interrupt;
        TextBox[] names;
        TextBox[] arrivals;
        TextBox[] bursts;
        TextBox[] priorities;
        TextBox newname;
        TextBox newburst;
        TextBox newpriority;
        Chart timeline;
        int counter;
        Label waiting;
        Scheduler cpu = new Scheduler();
        Stack<Control> objects = new Stack<Control>();
        Scheduler newProcesses = new Scheduler();
        public CPU_Scheduler()
        {

            InitializeComponent();
            txt_num.Text = "4";
            cmb_select.SelectedIndex = 0;
            timeline = new Chart();
            waiting = new Label();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                numberOfProcesses = Int32.Parse(txt_num.Text);
                if (numberOfProcesses <= 0)
                    throw new System.Exception("Number of Processes must be positive integer");
                type = cmb_select.SelectedItem.ToString();
                names = new TextBox[numberOfProcesses];
                arrivals = new TextBox[numberOfProcesses];
                bursts = new TextBox[numberOfProcesses];
                priorities = new TextBox[numberOfProcesses];
                quantum = new TextBox();
                interrupt = new TextBox();
                TextBox newname = new TextBox();
                TextBox newburst = new TextBox();
                TextBox newpriority = new TextBox();
                AddNewProcess(numberOfProcesses, type);
                btn_OK.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ex is Exception)
                {
                    string message = "Invalid Input Data \nPlease select a Scheduler and Enter a positive Integer";
                    string title = "ERROR";
                    // Show message box
                    MessageBox.Show(ex.Message + message, title);
                }
            }
        }

        void AddNewProcess(int n, string t)
        {
            if (n <= 0) throw new System.Exception("Number of Processes must be Positive integer");
            if (t == "First Come First Served" || t == "Shortest Job first (P)" || t == "Shortest Job first (NP)" || t == "Priority (P)" || t == "Priority (NP)" || t == "Round Robin")
            {
                System.Windows.Forms.Label l1 = new System.Windows.Forms.Label();
                this.Controls.Add(l1);
                l1.Top = 80;
                l1.Left = 10;
                l1.Text = "Name ";
                System.Windows.Forms.Label l2 = new System.Windows.Forms.Label();
                this.Controls.Add(l2);
                l2.Top = 80;
                l2.Left = 160;
                l2.Text = "Arrival Time";
                System.Windows.Forms.Label l3 = new System.Windows.Forms.Label();
                this.Controls.Add(l3);
                l3.Top = 80;
                l3.Left = 310;
                l3.Text = "Burst Time";
                if (t == "Priority (P)" || t == "Priority (NP)")
                {
                    System.Windows.Forms.Label l4 = new System.Windows.Forms.Label();
                    this.Controls.Add(l4);
                    l4.Top = 80;
                    l4.Left = 460;
                    l4.Text = "Priority";
                }
                for (int i = 0; i < n; i++)
                {
                    System.Windows.Forms.TextBox txt1 = new System.Windows.Forms.TextBox();
                    names[i] = txt1;
                    this.Controls.Add(txt1);
                    txt1.Top = (i * 30) + 105;
                    txt1.Left = 10;
                    txt1.Text = "P" + (i).ToString();
                    System.Windows.Forms.TextBox txt2 = new System.Windows.Forms.TextBox();
                    arrivals[i] = txt2;
                    this.Controls.Add(txt2);
                    txt2.Top = (i * 30) + 105;
                    txt2.Left = 160;
                    txt2.Text = (i + 1).ToString();
                    System.Windows.Forms.TextBox txt3 = new System.Windows.Forms.TextBox();
                    bursts[i] = txt3;
                    this.Controls.Add(txt3);
                    txt3.Top = (i * 30) + 105;
                    txt3.Left = 310;
                    txt3.Text = (i + 1).ToString();
                    if (t == "Priority (P)" || t == "Priority (NP)")
                    {
                        System.Windows.Forms.TextBox txt4 = new System.Windows.Forms.TextBox();
                        priorities[i] = txt4;
                        this.Controls.Add(txt4);
                        txt4.Top = (i * 30) + 105;
                        txt4.Left = 460;
                        txt4.Text = (i + 1).ToString();
                    }

                }
                if (t == "Round Robin")
                {
                    System.Windows.Forms.Label l5 = new System.Windows.Forms.Label();
                    this.Controls.Add(l5);
                    l5.Top = 80;
                    l5.Left = 460;
                    l5.Text = "Quantum Time ";
                    System.Windows.Forms.TextBox txt5 = new System.Windows.Forms.TextBox();
                    quantum = txt5;
                    this.Controls.Add(txt5);
                    txt5.Top = 105;

                    txt5.Left = 460;
                    txt5.Text = "1";
                }



                System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
                btn.Name = "btn_start";
                btn.Font = new System.Drawing.Font("Century", 15, System.Drawing.FontStyle.Regular);
                btn.ForeColor = System.Drawing.Color.Maroon;
                this.Controls.Add(btn);
                btn.Top = (numberOfProcesses * 30) + 120;

                btn.Left = 10;
                btn.Size = new System.Drawing.Size(400, 30);
                btn.Text = "START";
                btn_st = btn;
                btn.Click += new EventHandler(Button_Click_st);


                System.Windows.Forms.Button btn3 = new System.Windows.Forms.Button();
                btn3.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
                btn3.ForeColor = System.Drawing.Color.Maroon;
                this.Controls.Add(btn3);
                btn3.Top = (numberOfProcesses * 30) + 120;
                btn3.Left = 440;
                btn3.Size = new System.Drawing.Size(150, 30);
                btn3.Text = "INTERRUPT";
                btn3.Click += new EventHandler(Button_Click_add);

                System.Windows.Forms.Button b2 = new System.Windows.Forms.Button();
                b2.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
                b2.ForeColor = System.Drawing.Color.Maroon;
                b2.Size = new System.Drawing.Size(150, 30);
                this.Controls.Add(b2);
                b2.Top = (numberOfProcesses * 30) + 120;
                b2.Left = 620;
                b2.Text = "UNDO Interrupt";
                b2.Click += new EventHandler(Button_Click_con);

                /* System.Windows.Forms.Button howtouseInt = new System.Windows.Forms.Button();
                 howtouseInt.Font = new System.Drawing.Font("Century", 10, System.Drawing.FontStyle.Regular);
                 howtouseInt.ForeColor = System.Drawing.Color.Maroon;
                 howtouseInt.Size = new System.Drawing.Size(100, 30);
                 this.Controls.Add(howtouseInt);
                 howtouseInt.Top = (numberOfProcesses * 30) + 120;
                 howtouseInt.Left = 690;
                 howtouseInt.Text = "How to Use";
                 howtouseInt.Click += new EventHandler(Button_Click_howtouse);*/



            }
            else throw new System.Exception("Type of Scheduler is invalid");
        }

        private void Button_Click_st(object sender, EventArgs e)
        {
            try
            {
                if (type == "Round Robin")
                {
                    q = Convert.ToDouble(quantum.Text);
                    if (q <= 0)
                        throw new System.Exception("Quantum must be positive integer");
                }
                fillScheduler();
                scheduleCPU();
                btn_st.Enabled = false;

                /*System.Windows.Forms.Button btn3 = new System.Windows.Forms.Button();                
                btn3.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
                btn3.ForeColor = System.Drawing.Color.Maroon;
                this.Controls.Add(btn3);
                btn3.Top = (numberOfProcesses * 30) + 120;
                btn3.Left = 620;
                btn3.Size = new System.Drawing.Size(130, 30);
                btn3.Text = "INTERRUPT";
                btn3.Click += new EventHandler(Button_Click_add);
                System.Windows.Forms.Button b2 = new System.Windows.Forms.Button();
                b2.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
                b2.ForeColor = System.Drawing.Color.Maroon;
                b2.Size = new System.Drawing.Size(160, 30);
                this.Controls.Add(b2);                
                b2.Top = (numberOfProcesses * 30) + 120;
                b2.Left = 440;
                b2.Text = "UNDO Interrupt";
                b2.Click += new EventHandler(Button_Click_con);*/
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void FCFS(Scheduler s)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            temp = s.sort("arrival");
            node p = temp.getProcesses().getChain();
            double i = p.getData().getArrival();
            int counter = 0;
            double average = 0;
            double increment = p.getData().getArrival(), AvgTime = 0, TurnAround = 0;
            Series idle = new Series();
            createSeries(idle, i);
            while (p != null)
            {
                Series barSeries = new Series();
                barSeries.ChartType = SeriesChartType.StackedBar;
                barSeries.XValueType = ChartValueType.String;
                barSeries.Points.AddXY("timeline", p.getData().getDuration());

                barSeries.Label = p.getData().getName();
                timeline.Series.Add(barSeries);
                increment += p.getData().getDuration();
                average = average + i - p.getData().getArrival();
                i = i + p.getData().getDuration();

                TurnAround += (increment - p.getData().getArrival());
                if (i < numberOfProcesses - 1)
                    AvgTime += (increment - p.getNext().getData().getArrival());
                counter++;
                p = p.getNext();
            }

            waiting.Top = (numberOfProcesses * 30) + 165;
            waiting.Left = 10;
            waiting.Size = new System.Drawing.Size(500, 50);
            waiting.Text = "Average Waiting Time = " + average.ToString() + " / " + counter.ToString() + " = " + (average / counter).ToString();
            waiting.Text += "\nAverage TurnAround Time = " + TurnAround.ToString() + " / " + counter.ToString() + " = " + (TurnAround / counter).ToString();

            cpu.clear();
        }



        void SJF_NPrem(Scheduler s)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            temp = s.sort("arrival");
            double first = temp.getProcesses().getChain().getData().getArrival();
            double i = temp.getProcesses().getChain().getData().getArrival();
            int counter = 0;
            double average = 0;
            int flag = 0;
            Series idle = new Series();
            createSeries(idle, i);
            double bursttimes = 0;
            node pro = temp.getProcesses().getChain();
            while (pro != null)
            {
                bursttimes += pro.getData().getDuration();
                pro = pro.getNext();

            }
            while (temp.getProcesses().getChain() != null)
            {
                Scheduler temp2 = new Scheduler();
                while (temp.getProcesses().getChain() != null && temp.getProcesses().getChain().getData().getArrival() <= first)
                {
                    temp2.add(temp.getProcesses().getChain().getData());
                    temp.remove(temp.getProcesses().getChain().getData().getName());
                }
                temp2 = temp2.sort("duration");
                node p2 = temp2.getProcesses().getChain();
                while (p2 != null)
                {
                    Series barSeries = new Series();
                    barSeries.ChartType = SeriesChartType.StackedBar;
                    barSeries.XValueType = ChartValueType.String;
                    barSeries.Points.AddXY("timeline", p2.getData().getDuration());
                    barSeries.Label = p2.getData().getName();
                    timeline.Series.Add(barSeries);
                    average = average + i - p2.getData().getArrival();
                    i = i + p2.getData().getDuration();
                    counter++;
                    if (temp.getProcesses().getChain() != null)
                    {
                        temp2.remove(p2.getData().getName());
                        flag = 1;
                        p2 = p2.getNext();

                        break;

                    }
                    p2 = p2.getNext();
                }
                first = i;
                if (flag == 1)
                {
                    while (p2 != null)
                    {
                        temp.add(p2.getData());
                        p2 = p2.getNext();
                    }
                    flag = 0;

                }
                temp2.clear();
            }
            waiting.Top = (numberOfProcesses * 30) + 165;
            waiting.Left = 10;
            waiting.Size = new System.Drawing.Size(500, 50);
            waiting.Text = "Average Waiting Time = " + average.ToString() + " / " + counter.ToString() + " = " + (average / counter).ToString();
            waiting.Text += "\nAverage TurnAround Time = " + (average + bursttimes).ToString() + " / " + counter.ToString() + " = " + ((average + bursttimes) / counter).ToString();
            temp.clear();
            cpu.clear();
        }

        void Priority_NPrem(Scheduler s)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            temp = s.sort("arrival");
            double first = temp.getProcesses().getChain().getData().getArrival();
            double i = temp.getProcesses().getChain().getData().getArrival();
            int counter = 0;
            double average = 0;
            Series idle = new Series();
            createSeries(idle, i);
            double bursttimes = 0;
            int flag = 0;
            node pro = temp.getProcesses().getChain();
            while (pro != null)
            {
                bursttimes += pro.getData().getDuration();
                pro = pro.getNext();

            }
            while (temp.getProcesses().getChain() != null)
            {
                Scheduler temp2 = new Scheduler();
                while (temp.getProcesses().getChain() != null && temp.getProcesses().getChain().getData().getArrival() <= first)
                {
                    temp2.add(temp.getProcesses().getChain().getData());
                    temp.remove(temp.getProcesses().getChain().getData().getName());
                }
                temp2 = temp2.sort("priority");
                node p3 = temp2.getProcesses().getChain();
                while (p3 != null)
                {
                    Console.Write(p3.getData().getName() + "has piority" + p3.getData().getPriority() + "\n");
                    p3 = p3.getNext();

                }
                Console.Write("first is" + first + "\n");
                Console.Write("***************\n");
                node p2 = temp2.getProcesses().getChain();
                while (p2 != null)
                {
                    Series barSeries = new Series();
                    barSeries.ChartType = SeriesChartType.StackedBar;
                    barSeries.XValueType = ChartValueType.String;
                    barSeries.Points.AddXY("timeline", p2.getData().getDuration());
                    barSeries.Label = p2.getData().getName();
                    timeline.Series.Add(barSeries);
                    average = average + i - p2.getData().getArrival();
                    i = i + p2.getData().getDuration();
                    counter++;
                    if (temp.getProcesses().getChain() != null)
                    {
                        temp2.remove(p2.getData().getName());
                        flag = 1;
                        p2 = p2.getNext();

                        break;

                    }
                    p2 = p2.getNext();
                }
                first = i;
                if (flag == 1)
                {
                    while (p2 != null)
                    {
                        temp.add(p2.getData());
                        p2 = p2.getNext();
                    }
                    flag = 0;

                }
                temp2.clear();
            }
            waiting.Top = (numberOfProcesses * 30) + 165;
            waiting.Left = 10;
            waiting.Size = new System.Drawing.Size(500, 50);
            waiting.Text = "Average Waiting Time = " + average.ToString() + " / " + counter.ToString() + " = " + (average / counter).ToString();
            waiting.Text += "\nAverage TurnAround Time = " + (average + bursttimes).ToString() + " / " + counter.ToString() + " = " + ((average + bursttimes) / counter).ToString();
            temp.clear();
            cpu.clear();
        }


        void SJF_Prem(Scheduler s)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            temp = s.sort("arrival");
            double i = temp.getProcesses().getChain().getData().getArrival();
            int counter = 0;
            int flag = 0;
            double average = 0;
            double working = 0;
            Series idle = new Series();
            createSeries(idle, i);
            List<Process> pro = new List<Process>();
            List<Process> departure = new List<Process>();
            List<double> depart = new List<double>();
            Dictionary<string, int> flags = new Dictionary<string, int>();
            node pp = temp.getProcesses().getChain();
            double bursttimes = 0;
            while (pp != null)
            {
                Process t = pp.getData();
                pro.Add(t);
                bursttimes += t.getDuration();
                pp = pp.getNext();
            }
            for (var k = 0; k < pro.Count(); k++)
            {
                flags.Add(pro[k].getName(), 0);

            }

            while (pro.Count() != 0)
            {

                Scheduler temp2 = new Scheduler();

                node p1 = temp.getProcesses().getChain();
                List<Process> pro2 = new List<Process>();




                double first = temp.getProcesses().getChain().getData().getArrival();
                double first2 = pro[0].getArrival();
                for (var j = 0; j < pro.Count(); j++)
                {
                    if (pro[j].getArrival() <= i)
                    {
                        pro2.Add(pro[j]);
                        pro.Remove(pro[j]);
                        j--;

                    }
                }
                Console.Write("before\n");
                for (var k = 0; k < pro2.Count(); k++)
                {
                    Console.Write(pro2[k].getName() + " Duaration=" + pro2[k].getDuration() + "\n");
                }
                Console.Write("*************\n");
                for (var k = 0; k < pro2.Count() - 1; k++)
                {
                    for (var j = 0; j < pro2.Count() - 1 - k; j++)
                    {
                        if (pro2[j].getDuration() > pro2[j + 1].getDuration() || (pro2[j].getDuration() == pro2[j + 1].getDuration() && pro2[j].getArrival() > pro2[j + 1].getArrival()))
                        {
                            Process tmp = pro2[j];
                            pro2[j] = pro2[j + 1];
                            pro2[j + 1] = tmp;

                        }
                    }
                }
                Console.Write("after\n");
                for (var k = 0; k < pro2.Count(); k++)
                {
                    Console.Write(pro2[k].getName() + " Duaration=" + pro2[k].getDuration() + "\n");
                }
                Console.Write("*************\n");

                for (var k = 0; k < pro2.Count(); k++)
                {
                    if (pro.Count() != 0 && pro2[k].getDuration() + pro2[k].getArrival() > pro[0].getArrival()
                        && true)
                    {

                        flags[pro2[k].getName()] = 1;
                        Series barSeries = new Series();
                        barSeries.ChartType = SeriesChartType.StackedBar;
                        barSeries.XValueType = ChartValueType.String;
                        if (pro[0].getArrival() - pro2[k].getArrival() < pro2[k].getDuration()) { barSeries.Points.AddXY("timeline", pro[0].getArrival() - pro2[k].getArrival()); }
                        else { barSeries.Points.AddXY("timeline", pro2[k].getDuration()); }


                        barSeries.Color = pro2[k].getColor();
                        barSeries.Label = pro2[k].getName();
                        timeline.Series.Add(barSeries);
                        double dur = pro2[k].getDuration();
                        if (pro[0].getArrival() - pro2[k].getArrival() < pro2[k].getDuration())
                        {
                            pro2[k].setDuration(pro2[k].getDuration() + pro2[k].getArrival() - pro[0].getArrival());
                        }
                        else
                        {
                            pro2[k].setDuration(pro2[k].getDuration() - pro2[k].getDuration());
                        }
                        pro.Add(pro2[k]);



                        if (pro[0].getArrival() - pro2[k].getArrival() < pro2[k].getDuration())
                        {
                            i = pro[0].getArrival();
                            working = working + pro[0].getArrival() - pro2[k].getArrival();
                            depart.Add(i);
                            departure.Add(pro2[k]);

                        }
                        else
                        {
                            working = working + pro2[k].getDuration();
                            i = i + pro2[k].getDuration();
                            depart.Add(i);
                            departure.Add(pro2[k]);

                        }

                        flag = 1;
                        break;
                    }
                    else
                    {
                        Series barSeries = new Series();
                        barSeries.ChartType = SeriesChartType.StackedBar;
                        barSeries.XValueType = ChartValueType.String;
                        barSeries.Points.AddXY("timeline", pro2[k].getDuration());
                        barSeries.Color = pro2[k].getColor();
                        barSeries.Label = pro2[k].getName();
                        timeline.Series.Add(barSeries);
                        average = average + i - pro2[k].getArrival();
                        i = i + pro2[k].getDuration();
                        depart.Add(i);
                        departure.Add(pro2[k]);
                        counter++;
                        flag = 1;
                        break;
                    }

                }

                if (pro2.Count() > 1 && flag == 1)
                {
                    for (var l = 1; l < pro2.Count(); l++)
                    {
                        if (pro2[l].getDuration() > 0)
                            pro.Add(pro2[l]);


                    }
                    flag = 0;
                }
                pro2.Clear();
            }
            waiting.Top = (numberOfProcesses * 30) + 165;
            waiting.Left = 10;
            waiting.Size = new System.Drawing.Size(500, 50);
            waiting.Text = "Average Waiting Time = " + (average - working).ToString() + " / " + counter.ToString() + " = " + ((average - working) / counter).ToString();
            waiting.Text += "\nAverage TurnAround Time = " + (average - working + bursttimes).ToString() + " / " + counter.ToString() + " = " + ((average - working + bursttimes) / counter).ToString();
            temp.clear();
            cpu.clear();
        }

        void Priority_Prem(Scheduler s)
        {
            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            temp = s.sort("arrival");
            double i = temp.getProcesses().getChain().getData().getArrival();
            int counter = 0;
            int flag = 0;
            double average = 0;
            double working = 0;
            Series idle = new Series();
            createSeries(idle, i);
            List<Process> pro = new List<Process>();
            List<Process> departure = new List<Process>();
            List<double> depart = new List<double>();
            Dictionary<string, int> flags = new Dictionary<string, int>();
            node pp = temp.getProcesses().getChain();
            double bursttimes = 0;
            while (pp != null)
            {
                Process t = pp.getData();
                pro.Add(t);
                pp = pp.getNext();
                bursttimes += t.getDuration();
            }
            for (var k = 0; k < pro.Count(); k++)
            {
                flags.Add(pro[k].getName(), 0);

            }

            while (pro.Count() != 0)
            {

                Scheduler temp2 = new Scheduler();

                node p1 = temp.getProcesses().getChain();
                List<Process> pro2 = new List<Process>();




                double first = temp.getProcesses().getChain().getData().getArrival();
                double first2 = pro[0].getArrival();
                for (var j = 0; j < pro.Count(); j++)
                {
                    if (pro[j].getArrival() <= i)
                    {
                        pro2.Add(pro[j]);
                        pro.Remove(pro[j]);
                        j--;

                    }
                }
                for (var k = 0; k < pro2.Count() - 1; k++)
                {
                    for (var j = 0; j < pro2.Count() - 1 - k; j++)
                    {
                        if (pro2[j].getPriority() > pro2[j + 1].getPriority() || (pro2[j].getPriority() == pro2[j + 1].getPriority() && pro2[j].getArrival() > pro2[j + 1].getArrival()))
                        {
                            Process tmp = pro2[j];
                            pro2[j] = pro2[j + 1];
                            pro2[j + 1] = tmp;

                        }
                    }
                }

                for (var k = 0; k < pro2.Count(); k++)
                {
                    if (pro.Count() != 0 && pro2[k].getDuration() + pro2[k].getArrival() > i
                        && true)
                    {

                        flags[pro2[k].getName()] = 1;
                        Series barSeries = new Series();
                        barSeries.ChartType = SeriesChartType.StackedBar;
                        barSeries.XValueType = ChartValueType.String;
                        if (pro[0].getArrival() - pro2[k].getArrival() < pro2[k].getDuration()) { barSeries.Points.AddXY("timeline", pro[0].getArrival() - pro2[k].getArrival()); }
                        else { barSeries.Points.AddXY("timeline", pro2[k].getDuration()); }


                        barSeries.Color = pro2[k].getColor();
                        barSeries.Label = pro2[k].getName();
                        timeline.Series.Add(barSeries);
                        double dur = pro2[k].getDuration();
                        if (pro[0].getArrival() - pro2[k].getArrival() < pro2[k].getDuration())
                        {
                            pro2[k].setDuration(pro2[k].getDuration() + pro2[k].getArrival() - pro[0].getArrival());
                        }
                        else
                        {
                            pro2[k].setDuration(pro2[k].getDuration() - pro2[k].getDuration());
                        }
                        pro.Add(pro2[k]);



                        if (pro[0].getArrival() - pro2[k].getArrival() < pro2[k].getDuration())
                        {

                            i = pro[0].getArrival();
                            working = working + pro[0].getArrival() - pro2[k].getArrival();
                            depart.Add(i);
                            departure.Add(pro2[k]);

                        }
                        else
                        {
                            working = working + pro2[k].getDuration();
                            i = i + pro2[k].getDuration();
                            depart.Add(i);
                            departure.Add(pro2[k]);

                        }

                        flag = 1;
                        break;
                    }
                    else
                    {

                        Series barSeries = new Series();
                        barSeries.ChartType = SeriesChartType.StackedBar;
                        barSeries.XValueType = ChartValueType.String;
                        barSeries.Points.AddXY("timeline", pro2[k].getDuration());
                        barSeries.Color = pro2[k].getColor();
                        barSeries.Label = pro2[k].getName();
                        timeline.Series.Add(barSeries);
                        average = average + i - pro2[k].getArrival();

                        i = i + pro2[k].getDuration();

                        depart.Add(i);
                        departure.Add(pro2[k]);
                        counter++;
                        flag = 1;
                        break;
                    }

                }

                if (pro2.Count() > 1 && flag == 1)
                {
                    for (var l = 1; l < pro2.Count(); l++)
                    {
                        if (pro2[l].getDuration() > 0)
                            pro.Add(pro2[l]);


                    }
                    flag = 0;
                }
                pro2.Clear();
            }

            waiting.Top = (numberOfProcesses * 30) + 165;
            waiting.Left = 10;
            waiting.Size = new System.Drawing.Size(500, 50);
            waiting.Text = "Average Waiting Time = " + (average - working).ToString() + " / " + counter.ToString() + " = " + ((average - working) / counter).ToString();
            waiting.Text += "\nAverage TurnAround Time = " + (average - working + bursttimes).ToString() + " / " + counter.ToString() + " = " + ((average - working + bursttimes) / counter).ToString();
            temp.clear();
            cpu.clear();

        }

        void RoundRobin(Scheduler s, double q)
        {

            createWaiting();
            createTimeline();
            Scheduler temp = new Scheduler();
            temp = s.sort("arrival");
            node p = temp.getProcesses().getChain();
            double first = p.getData().getArrival();
            double i = p.getData().getArrival();
            double j = 0;
            int counter = 0;
            double average = 0;
            double working = 0;
            double arrival = 0;
            Series idle = new Series();
            createSeries(idle, i);
            node p1 = temp.getProcesses().getChain();
            List<Process> pro = new List<Process>();
            Dictionary<string, int> flags = new Dictionary<string, int>();
            double bursts = 0;
            while (p1 != null)
            {
                Process t = p1.getData();
                pro.Add(t);
                arrival = arrival + p1.getData().getArrival();
                bursts += p1.getData().getDuration();
                p1 = p1.getNext();
            }
            for (var k = 0; k < pro.Count(); k++)
            {
                flags.Add(pro[k].getName(), 0);
            }
            while (pro.Count() != 0)
            {
                flags[pro[0].getName()] = 1;
                j = (pro[0].getDuration() <= q) ? pro[0].getDuration() : q;
                Series barSeries = new Series();
                barSeries.ChartType = SeriesChartType.StackedBar;
                barSeries.XValueType = ChartValueType.String;
                barSeries.Points.AddXY("timeline", j);
                barSeries.Color = pro[0].getColor();
                barSeries.Label = pro[0].getName();
                timeline.Series.Add(barSeries);
                if (pro[0].getDuration() <= q)
                {
                    average = average + i;
                    i = i + pro[0].getDuration();
                    counter++;
                }
                else
                {
                    i = i + q;
                    working = working + q;
                    pro[0].setDuration(pro[0].getDuration() - q);
                    pro[0].setArrival(i);
                    pro.Add(pro[0]);
                }

                pro.Remove(pro[0]);


                for (var k = 0; k < pro.Count() - 1; k++)
                {
                    for (var l = 0; l < pro.Count() - 1 - k; l++)
                    {
                        if (pro[l].getArrival() > pro[l + 1].getArrival())
                        {
                            Process tmp = pro[l];
                            pro[l] = pro[l + 1];
                            pro[l + 1] = tmp;

                        }
                        /* else if(pro[l].getArrival() <= i && flags[pro[l].getName()] == 0)
                         {
                             if((flags[pro[l+1].getName()] == 0&& pro[l].getArrival() > pro[l + 1].getArrival())|| (flags[pro[l + 1].getName()] == 1)) {
                                 Process tmp = pro[l];
                                 pro[l] = pro[l + 1];
                                 pro[l + 1] = tmp;

                             }
                         }*/
                    }
                }


                List<Process> t = new List<Process>();
                int index = 0;
                int flag = 0;
                for (var k = 0; k < pro.Count(); k++)
                {
                    if (!(pro[k].getArrival() <= i && flags[pro[k].getName()] == 0))
                    {
                        t.Add(pro[k]);
                        pro.Remove(pro[k]);
                        k--;
                        index = k;
                        flag = 1;
                    }
                }
                for (var k = 0; k < pro.Count() - 1; k++)
                {
                    for (var l = 0; l < pro.Count() - 1 - k; l++)
                    {
                        if (pro[l].getArrival() > pro[l + 1].getArrival())
                        {
                            Process tmp = pro[l];
                            pro[l] = pro[l + 1];
                            pro[l + 1] = tmp;

                        }
                    }
                }
                pro.AddRange(t);

                /* if (flag == 1)
         {
             for(var k = index; k >= 1; k--)
             {
                 pro[k] = pro[k - 1];
             }
             pro[0] = t;
         }*/

            }
            waiting.Top = (numberOfProcesses * 30) + 165;
            waiting.Left = 10;
            waiting.Size = new System.Drawing.Size(500, 50);
            waiting.Text = "Average Waiting Time = ( " + (average - arrival).ToString() + " - " + working.ToString() + " ) / " + counter.ToString() + " = " + ((average - arrival - working) / counter).ToString();
            waiting.Text += "\nAverage TurnAround Time = " + (average - arrival - working + bursts).ToString() + " / " + counter.ToString() + " = " + ((average - arrival - working + bursts) / counter).ToString();
            cpu.clear();
        }

        private void Button_Click_res(object sender, EventArgs e)
        {
            this.Controls.Clear();
            cpu.clear();
            newProcesses.clear();
            timeline = new Chart();
            waiting = new Label();
            this.InitializeComponent();
            txt_num.Text = "4";
            cmb_select.SelectedIndex = 0;
        }

        void createTimeline()
        {
            timeline.Size = new System.Drawing.Size(820, 100);
            ChartArea area = new ChartArea(counter.ToString());
            counter++;
            timeline.ChartAreas.Add(area);
            timeline.Top = 220 + (30 * numberOfProcesses);
            this.Controls.Add(timeline);
        }

        void createWaiting()
        {
            waiting.Font = new System.Drawing.Font("Century", 12, System.Drawing.FontStyle.Regular);
            waiting.ForeColor = System.Drawing.Color.Maroon;
            waiting.Top = 275 + (30 * numberOfProcesses);
            waiting.Size = new System.Drawing.Size(500, 20);
            this.Controls.Add(waiting);
        }

        void createSeries(Series idle, double i)
        {
            idle.ChartType = SeriesChartType.StackedBar;
            idle.XValueType = ChartValueType.String;
            idle.Points.AddXY("timeline", i);
            idle.Label = "Idle";
            idle.Color = System.Drawing.Color.White;
            timeline.Series.Add(idle);
        }

        private void Button_Click_int(object sender, EventArgs e)
        {




        }

        private void Button_Click_add(object sender, EventArgs e)
        {

            this.Controls.Remove(timeline);
            this.Controls.Remove(waiting);

            System.Windows.Forms.Label processLabel = new System.Windows.Forms.Label();
            this.Controls.Add(processLabel);
            objects.Push(processLabel);
            processLabel.Top = (numberOfProcesses * 30) + 175;
            processLabel.Left = 10;
            processLabel.Text = "Process Name";

            System.Windows.Forms.TextBox processText = new System.Windows.Forms.TextBox();
            this.Controls.Add(processText);
            objects.Push(processText);
            processText.Top = (numberOfProcesses * 30) + 200;
            processText.Left = 10;
            processText.Text = "New ";
            newname = processText;

            System.Windows.Forms.Label arrivalLabel = new System.Windows.Forms.Label();
            this.Controls.Add(arrivalLabel);
            objects.Push(arrivalLabel);
            arrivalLabel.Top = 175 + (30 * numberOfProcesses); //500
            arrivalLabel.Left = 160;            //100
            arrivalLabel.Text = "Arrival Time";

            System.Windows.Forms.TextBox t2 = new System.Windows.Forms.TextBox();
            this.Controls.Add(t2);
            objects.Push(t2);
            t2.Top = (numberOfProcesses * 30) + 200;
            t2.Left = 160;
            t2.Text = "3";
            interrupt = t2;
            
            System.Windows.Forms.Label burstLabel = new System.Windows.Forms.Label();
            this.Controls.Add(burstLabel);
            objects.Push(burstLabel);
            burstLabel.Top = 175 + (30 * numberOfProcesses); //500
            burstLabel.Left = 310;            //100
            burstLabel.Text = "Burst Time";

            System.Windows.Forms.TextBox t3 = new System.Windows.Forms.TextBox();
            this.Controls.Add(t3);
            objects.Push(t3);
            t3.Top = (numberOfProcesses * 30) + 200;
            t3.Left = 310;
            t3.Text = "5";
            newburst = t3;

            System.Windows.Forms.Button bt = new System.Windows.Forms.Button();


            if (type == "Priority (P)" || type == "Priority (NP)")
            {
                System.Windows.Forms.Label priorityLabel = new System.Windows.Forms.Label();
                this.Controls.Add(priorityLabel);
                objects.Push(priorityLabel);
                priorityLabel.Top = 175 + (30 * numberOfProcesses); //500
                priorityLabel.Left = 460;            //100
                priorityLabel.Text = "Priority";

                System.Windows.Forms.TextBox t4 = new System.Windows.Forms.TextBox();
                this.Controls.Add(t4);
                objects.Push(t4);
                t4.Top = (numberOfProcesses * 30) + 200;
                t4.Left = 460;
                t4.Text = "1";
                newpriority = t4;


                bt.ForeColor = System.Drawing.Color.Maroon;
                bt.Size = new System.Drawing.Size(150, 25);
                this.Controls.Add(bt);
                objects.Push(bt);
                bt.Top = (numberOfProcesses * 30) + 200;
                bt.Left = 610;
                bt.Text = "ADD Process";
                bt.Click += new EventHandler(Button_Click_new);

            }

            else
            {
                bt.ForeColor = System.Drawing.Color.Maroon;
                bt.Size = new System.Drawing.Size(150, 25);
                this.Controls.Add(bt);
                objects.Push(bt);
                bt.Top = (numberOfProcesses * 30) + 200;
                bt.Left = 460;
                bt.Text = "ADD Process";
                bt.Click += new EventHandler(Button_Click_new);
            }
        }

        private void Button_Click_howtouse(object sender, EventArgs e)
        {
            Tips t = new Tips();
            t.Show();
        }
        private void Button_Click_con(object sender, EventArgs e)
        {
            this.Controls.Remove(timeline);
            this.Controls.Remove(waiting);
            timeline = new Chart();
            waiting = new Label();
            cpu.clear();
            fillScheduler();
            while (objects.Count > 0)
                this.Controls.Remove(objects.Pop());
            objects.Clear();
            scheduleCPU();
        }

        private void Button_Click_new(object sender, EventArgs e)
        {
            try
            {
                timeline = new Chart();
                waiting = new Label();
                Process Pn = new Process();
                Pn.setName(newname.Text);
                Pn.setArrival(Convert.ToDouble(interrupt.Text));
                if (Pn.getArrival() <= 0)
                    throw new System.Exception("Process " + "[ " + Pn.getName().ToString() + " ]" + " Arrival time must be positive integer");
                Pn.setDuration(Convert.ToDouble(newburst.Text));
                if (Pn.getDuration() <= 0)
                    throw new System.Exception("Process " + "[ " + Pn.getName().ToString() + " ]" + " Burst time must be positive integer");
                Random rnd = new Random(18 * System.Environment.TickCount);
                Pn.setColor(System.Drawing.Color.FromArgb(255, rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                if (type == "Priority (P)" || type == "Priority (NP)")
                {
                    Pn.setPriority(Int32.Parse(newpriority.Text));
                    if (Pn.getPriority() <= 0)
                        throw new System.Exception("Process " + "[ " + Pn.getName().ToString() + " ]" + " Priority must be positive integer");
                }
                fillScheduler();
                node p = new node();
                p = newProcesses.getProcesses().getChain();
                while (p != null)
                {
                    cpu.add(p.getData());
                    p = p.getNext();
                }
                cpu.add(Pn);
                newProcesses.add(Pn);
                while (objects.Count > 0)
                    this.Controls.Remove(objects.Pop());
                objects.Clear();
                scheduleCPU();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void scheduleCPU()
        {
            if (type == "First Come First Served")
                FCFS(cpu);
            else if (type == "Shortest Job first (NP)")
                SJF_NPrem(cpu);
            else if (type == "Priority (NP)")
                Priority_NPrem(cpu);
            else if (type == "Shortest Job first (P)")
                SJF_Prem(cpu);
            else if (type == "Priority (P)")
                Priority_Prem(cpu);
            else if (type == "Round Robin")
            {
                RoundRobin(cpu, q);
            }
        }

        void fillScheduler()
        {
            for (int i = 0; i < numberOfProcesses; i++)
            {
                Process p = new Process();
                p.setName(names[i].Text);
                p.setArrival(Convert.ToDouble(arrivals[i].Text));
                if (p.getArrival() <= 0)
                    throw new System.Exception("Process " + "[ " + p.getName().ToString()  + " ]" + " Arrival time must be positive integer");
                p.setDuration(Convert.ToDouble(bursts[i].Text));
                if (p.getDuration() <= 0)
                    throw new System.Exception("Process " + "[ " + p.getName().ToString()  + " ]" + " Burst time must be positive integer");
                Random rnd = new Random(i * (System.Environment.TickCount));
                p.setColor(System.Drawing.Color.FromArgb(255, rnd.Next(255), rnd.Next(255), rnd.Next(255)));
                if (type == "Priority (P)" || type == "Priority (NP)")
                {
                    p.setPriority(Int32.Parse(priorities[i].Text));
                    if (p.getPriority() <= 0)
                        throw new System.Exception("Process " + "[ " + p.getName().ToString()  + " ]" + " Priority must be positive integer");
                }
                cpu.add(p);
            }
        }

        private void cmb_select_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            cpu.clear();
            newProcesses.clear();
            timeline = new Chart();
            waiting = new Label();
            this.InitializeComponent();
            txt_num.Text = "4";
            cmb_select.SelectedIndex = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Tips t = new Tips();
            t.Show();
        }
    }
}
