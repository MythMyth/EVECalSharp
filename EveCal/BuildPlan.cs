using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveCal.BPs;

namespace EveCal
{
    using HaulDetail = Dictionary<Tuple<FacilityType, FacilityType>, Dictionary<string, int>>;
    public class ItemWorkDetail
    {
        public string name;
        public int amount;
        public int jobRun;

        public ItemWorkDetail()
        {
            name = "";
            amount = 0;
            jobRun = 0;
        }
    }
    internal class BuildPlan
    {
        Dictionary<string, int> outputItems;
        Dictionary<FacilityType, Dictionary<string, int>> demand;
        Dictionary<FacilityType, Dictionary<string, int>> available;
        Dictionary<FacilityType, Dictionary<string, int>> running;
        public BuildPlan()
        {
            outputItems = new Dictionary<string, int>();
            demand = new Dictionary<FacilityType, Dictionary<string, int>>();
            available = Storage.GetAllAsset();
            running = Storage.GetOuptutRunningJobInFacility();
        }

        public bool Add(string item, int amount)
        {
            if(Loader.Have(item))
            {
                outputItems.Add(item, amount);
                return true;
            }
            return false;
        }

        void pushToPlan(HaulDetail haulPlan, Tuple<FacilityType, FacilityType> road, string item, int amount)
        {
            if (!haulPlan.ContainsKey(road)) haulPlan.Add(road, new Dictionary<string, int>());
            if (!haulPlan[road].ContainsKey(item)) haulPlan[road].Add(item, amount);
            else haulPlan[road][item] += amount;
        }

        int FindInDemand(string item, HaulDetail haulPlan)
        {
            int result = 0;
            foreach(FacilityType facility in demand.Keys)
            {
                if(demand[facility].ContainsKey(item))
                {
                    int demandInThisFacility = demand[facility][item];
                    //Available
                    if(available[facility].ContainsKey(item) && demandInThisFacility >= available[facility][item])
                    {
                        demandInThisFacility -= available[facility][item];
                        available[facility].Remove(item);
                    } else if(available[facility].ContainsKey(item))
                    {
                        available[facility][item] -= demandInThisFacility;
                        demandInThisFacility = 0;
                    }
                    if (demandInThisFacility > 0 && Loader.Have(item))
                    {
                        FacilityType makeAt = Loader.Get(item).MakeAt();
                        Tuple<FacilityType, FacilityType> road = new Tuple<FacilityType, FacilityType>(makeAt, facility);
                        //Can move
                        if (available[makeAt].ContainsKey(item) && demandInThisFacility >= available[makeAt][item])
                        {
                            demandInThisFacility -= available[makeAt][item];
                            pushToPlan(haulPlan, road, item, available[makeAt][item]);
                            available[makeAt].Remove(item);
                        }
                        else if (available[makeAt].ContainsKey(item))
                        {
                            available[makeAt][item] -= demandInThisFacility;
                            pushToPlan(haulPlan, road, item, demandInThisFacility);
                            demandInThisFacility = 0;
                        }
                    }
                    //Making
                    if (demandInThisFacility > 0 && Loader.Have(item))
                    {
                        FacilityType makeAt = Loader.Get(item).MakeAt();
                        Tuple<FacilityType, FacilityType> road = new Tuple<FacilityType, FacilityType>(makeAt, facility);
                        if (running[makeAt].ContainsKey(item) && demandInThisFacility >= running[makeAt][item])
                        {
                            demandInThisFacility -= running[makeAt][item];
                            pushToPlan(haulPlan, road, item, running[makeAt][item]);
                            running[makeAt].Remove(item);
                        } else if(running[makeAt].ContainsKey(item))
                        {
                            running[makeAt][item] -= demandInThisFacility;
                            pushToPlan(haulPlan, road, item, demandInThisFacility);
                            demandInThisFacility = 0;
                        }
                    }

                    result += demandInThisFacility;
                }
            }
            return result;
        }

        public Tuple<List<ItemWorkDetail>, HaulDetail> MakePlan(bool BPRunWithMax)
        {
            List<ItemWorkDetail> plan = new List<ItemWorkDetail>();
            Dictionary<string, int> allNode = new Dictionary<string, int>();
            HaulDetail haulPlan = new HaulDetail();
            demand.Clear();
            foreach (string key in outputItems.Keys)
            {
                GetAllNode(key.Trim(), allNode);
            }
            demand.Add(FacilityType.SOURCE, new Dictionary<string, int>());
            foreach (var pair in outputItems)
            {
                demand[FacilityType.SOURCE].Add(pair.Key.Trim(), pair.Value);
            }

            List<string> reverseBuildOrder = TopologicalSort(allNode);
            foreach (string item in reverseBuildOrder)
            {
                ItemWorkDetail workDetail = new ItemWorkDetail();
                workDetail.name = item.Trim();
                workDetail.amount = FindInDemand(workDetail.name, haulPlan);
                BP bp = Loader.Get(item.Trim());
                if(bp != null)
                {
                    Dictionary<string, int> run_material = bp.Cal(workDetail.amount, ref workDetail.jobRun, BPRunWithMax);
                    if(!demand.ContainsKey(bp.MakeAt()))
                    {
                        demand.Add(bp.MakeAt(), new Dictionary<string, int>());
                    }
                    foreach(var pair in run_material)
                    {
                        if (!demand[bp.MakeAt()].ContainsKey(pair.Key.Trim())) demand[bp.MakeAt()].Add(pair.Key.Trim(), 0);
                        demand[bp.MakeAt()][pair.Key.Trim()] += pair.Value;
                    }
                }
                plan.Add(workDetail);
            }
            plan.Reverse();
            return new Tuple<List<ItemWorkDetail>, HaulDetail>(plan, haulPlan);
        }

        void GetAllNode(string node, Dictionary<string, int> allNode)
        {
            if (allNode.ContainsKey(node)) return;
            allNode.Add(node, 0);
            BP bp = Loader.Get(node);
            if (bp != null)
            {
                foreach(string key in bp.material.Keys)
                {
                    GetAllNode(key.Trim(), allNode);
                }
            }
        }

        List<string> TopologicalSort(Dictionary<string, int> allNode)
        {
            List<string> result = new List<string>();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach( string key in allNode.Keys )
            {
                visited.Add(key.Trim(), false);
            }
            Stack<string> stack = new Stack<string>();
            foreach(string key in allNode.Keys)
            {
                TopologicalSortUtil(key.Trim(), stack, visited);
            }
            while(stack.Count > 0)
            {
                result.Add(stack.Pop());
            }
            return result;
        }

        void TopologicalSortUtil(string node, Stack<string> sortStack, Dictionary<string, bool> visited) 
        {
            if (visited[node]) return;
            if(!Loader.Have(node))
            {
                visited[node] = true;
                sortStack.Push(node);
                return;
            }

            BP bp = Loader.Get(node);
            visited[node] = true;
            foreach(string Key in bp.material.Keys)
            {
                TopologicalSortUtil(Key.Trim(), sortStack, visited);
            }
            sortStack.Push(node);
        }

    }
}
