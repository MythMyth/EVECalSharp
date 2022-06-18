using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveCal.BPs;

namespace EveCal
{

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
        public BuildPlan()
        {
            outputItems = new Dictionary<string, int>();
            demand = new Dictionary<FacilityType, Dictionary<string, int>>();  
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

        int FindInDemand(string item, Dictionary<string, int> haulable, Dictionary<Tuple<FacilityType, FacilityType>, Dictionary<string, int>> haulPlan)
        {
            int result = 0;
            foreach(FacilityType facility in demand.Keys)
            {
                if(demand[facility].ContainsKey(item))
                {
                    int demandInThisFacility = demand[facility][item] - Storage.Get(facility, item);
                    if(haulable.ContainsKey(item) && demandInThisFacility > 0 && haulable[item] > 0 && ((!Loader.Have(item)) || (Loader.Get(item).MakeAt() != facility))) {
                        int haul = Math.Min(demandInThisFacility, haulable[item]);
                        demandInThisFacility -= haul;
                        haulable[item] -= haul;
                        if(haul > 0) { 
                            BP bp = Loader.Get(item);
                            Tuple < FacilityType, FacilityType > road;
                            if (bp == null)
                            {
                                //If null mean it a raw material
                                road = new Tuple<FacilityType, FacilityType>(FacilityType.SOURCE, facility);
                            } else
                            {
                                road = new Tuple<FacilityType, FacilityType>(bp.MakeAt(), facility);
                            }
                            if(!haulPlan.ContainsKey(road))
                            {
                                haulPlan.Add(road, new Dictionary<string, int>());
                            }
                            if (!haulPlan[road].ContainsKey(item))
                            {
                                haulPlan[road].Add(item, 0);
                            }
                            haulPlan[road][item] += haul;
                        }
                    }
                    result += ((demandInThisFacility < 0) ? 0 : demandInThisFacility);
                }
            }
            return result;
        }

        public List<ItemWorkDetail> MakePlan(bool BPRunWithMax)
        {
            List<ItemWorkDetail> plan = new List<ItemWorkDetail>();
            Dictionary<string, int> allNode = new Dictionary<string, int>();
            Dictionary<string, int> haulable = Storage.GetHaulable();
            Dictionary<Tuple<FacilityType, FacilityType>, Dictionary<string, int>> haulPlan = new Dictionary<Tuple<FacilityType, FacilityType>, Dictionary<string, int>>();
            demand.Clear();
            foreach (string key in outputItems.Keys)
            {
                GetAllNode(key, allNode);
            }

            foreach (var pair in outputItems)
            {
                allNode[pair.Key] = pair.Value;
            }

            List<string> reverseBuildOrder = TopologicalSort(allNode);
            foreach (string item in reverseBuildOrder)
            {
                ItemWorkDetail workDetail = new ItemWorkDetail();
                workDetail.name = item;
                workDetail.amount = allNode[item] + FindInDemand(workDetail.name, haulable, haulPlan);
                BP bp = Loader.Get(item);
                if(bp != null)
                {
                    Dictionary<string, int> run_material = bp.Cal(workDetail.amount, ref workDetail.jobRun, BPRunWithMax);
                    if(!demand.ContainsKey(bp.MakeAt()))
                    {
                        demand.Add(bp.MakeAt(), new Dictionary<string, int>());
                    }
                    foreach(var pair in run_material)
                    {
                        if (!demand[bp.MakeAt()].ContainsKey(pair.Key)) demand[bp.MakeAt()].Add(pair.Key, 0);
                        demand[bp.MakeAt()][pair.Key] += pair.Value;
                    }
                }
                plan.Add(workDetail);
            }
            plan.Reverse();
            return plan;
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
                    GetAllNode(key, allNode);
                }
            }
        }

        List<string> TopologicalSort(Dictionary<string, int> allNode)
        {
            List<string> result = new List<string>();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach( string key in allNode.Keys )
            {
                visited.Add(key, false);
            }
            Stack<string> stack = new Stack<string>();
            foreach(string key in allNode.Keys)
            {
                TopologicalSortUtil(key, stack, visited);
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
                TopologicalSortUtil(Key, sortStack, visited);
            }
            sortStack.Push(node);
        }

    }
}
