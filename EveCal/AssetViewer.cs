using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveCal
{
    public partial class AssetViewer : Form
    {
        Dictionary<string, string> LocationName;
        List<string> syncFacilityListId;
        Dictionary<string, CharInfo> chars;

        string indy_path = "https://esi.evetech.net/latest/characters/{character_id}/industry/jobs";
        string asset_path = "https://esi.evetech.net/latest/characters/{character_id}/assets";
        public AssetViewer()
        {
            InitializeComponent();
            syncFacilityListId = new List<string>();
            chars = CharacterManager.GetCharList();

            foreach (FacilityType facilityType in Enum.GetValues(typeof(FacilityType)))
            {
                FacilityListBox.Items.Add(facilityType.ToString());
            }

            LoadFacilityList();
        }

        void LoadFacilityList()
        {
            LocationName = Storage.GetFacilityList();
            syncFacilityListId.Clear();
            FacilityList.Items.Clear();
            foreach(string id in LocationName.Keys)
            {
                syncFacilityListId.Add(id);
                FacilityList.Items.Add(new ListViewItem(LocationName[id]));
            }
        }

        private void reload_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(reloadAsset);
            coverLabel.Text = "Loading asset...";
            coverLabel.Visible = true;
            thread.Start();
        }

        void reloadAsset()
        {
            bool will_update_running = true;
            bool will_update_asset = true;
            List<Dictionary<string, string>> all_jobs = new List<Dictionary<string, string>>();
            Dictionary<string, int> map = new Dictionary<string, int>();
            List<string> ids = new List<string>();
            Dictionary<string, Dictionary<string, int>> assets = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> BPC = new Dictionary<string, int>();

            List<Dictionary<string, string>> AllLoadedAsset = new List<Dictionary<string, string>>();

            foreach (string cid in chars.Keys)
            {
                Invoke(AppendCover, "\n" + cid + " - " + chars[cid].Name + ": Indy jobs ");
                CharacterManager.GetRefreshToken(chars[cid].code, chars[cid].refresh);
                HttpClient client = new HttpClient();
                /*if (chars[cid].IndyEtag.Trim() != "")
                {
                    client.DefaultRequestHeaders.Add("If-None-Match", chars[cid].IndyEtag);
                }*/
                
                var response = client.GetAsync(indy_path.Replace("{character_id}", cid) + "?datasource=tranquility&include_completed=false&token=" + chars[cid].token)
                    .GetAwaiter().GetResult();
                string res_str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<Dictionary<string, string>> jobs = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(res_str);

                    foreach (Dictionary<string, string> job in jobs)
                    {
                        all_jobs.Add(job);
                        if (!map.ContainsKey(job["blueprint_type_id"]))
                        {
                            map.Add(job["blueprint_type_id"], 1);
                            ids.Add(job["blueprint_type_id"]);
                        }
                    }
                    Invoke(AppendCover, "success, ");
                } else if(response.StatusCode == System.Net.HttpStatusCode.NotModified)
                {
                    Invoke(AppendCover, "no modify, ");
                    will_update_running = false;
                } else
                {
                    Invoke(AppendCover, "failed, ");
                    will_update_running = false;
                }
                if (response.Headers.Contains("ETag"))
                {
                    chars[cid].IndyEtag = response.Headers.GetValues("ETag").First();
                }
                Invoke(AppendCover, "Assets load ");
                int assetPage = 1;
                int success = 0;
                int not_modified = 0;
                int failed = 0;

                //============ UPDATE ASSET===============


                for (int currPage = 1; currPage <= assetPage; currPage++)
                {
                    while (chars[cid].AssetEtag.Count < currPage)
                    {
                        chars[cid].AssetEtag.Add("");
                    }
                    /*
                    if (client.DefaultRequestHeaders.Contains("If-None-Match")) client.DefaultRequestHeaders.Remove("If-None-Match");
                    if (chars[cid].AssetEtag[currPage - 1].Trim() != "") client.DefaultRequestHeaders.Add("If-None-Match", chars[cid].AssetEtag[currPage - 1]);
                    */
                    response = client.GetAsync(asset_path.Replace("{character_id}", cid) + "?page=" + currPage + "&token=" + chars[cid].token).GetAwaiter().GetResult();
                    res_str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<Dictionary<string, string>> items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(res_str);
                        AllLoadedAsset.AddRange(items);
                        foreach (Dictionary<string, string> item in items)
                        {
                            if (item["location_flag"] != "Hangar") continue;
                            string location = item["location_id"];
                            string itemType = item["type_id"];
                            if (!map.ContainsKey(itemType))
                            {
                                map.Add(itemType, 1);
                                ids.Add(itemType);
                            }
                            int quantity = int.Parse(item["quantity"]);
                            if(item.ContainsKey("is_blueprint_copy"))
                            {
                                if (item["is_blueprint_copy"] == "true")
                                {
                                    if(!BPC.ContainsKey(itemType)) BPC.Add(itemType, quantity);
                                    else BPC[itemType] += quantity;
                                }
                            } 
                            else
                            {
                                if (!assets.ContainsKey(location))
                                {
                                    assets.Add(location, new Dictionary<string, int>());
                                }
                                if (!assets[location].ContainsKey(itemType))
                                {
                                    assets[location].Add(itemType, 0);
                                }
                                assets[location][itemType] += quantity;
                            }
                            
                        }
                        if (response.Headers.Contains("x-pages"))
                        {
                            string xpage = response.Headers.GetValues("x-pages").First();
                            assetPage = int.Parse(xpage);
                        }
                        success++;
                    } else if(response.StatusCode == System.Net.HttpStatusCode.NotModified)
                    {
                        not_modified++;
                        will_update_asset = false;
                    } else
                    {
                        failed++;
                        will_update_asset = false;
                    }
                    if (response.Headers.Contains("ETag"))
                    {
                        chars[cid].AssetEtag[currPage - 1] = response.Headers.GetValues("ETag").First();
                    }
                }
                Invoke(AppendCover, "success: " + success + ", not modified: " + not_modified + ", faield:" + failed);
            }
            Invoke(AppendCover, "\n Update ids");
            Cache.AddIds(ids);

            if(will_update_asset)
            {
                Invoke(AppendCover, "\n Update assets");

                Storage.UpadateAsset(AllLoadedAsset);

                Invoke(LoadFacilityList, null);
            }
            if (will_update_running)
            {
                Invoke(AppendCover, "\n Update running jobs");
                Storage.UpdateRunningJob(all_jobs);
            }

            Invoke(delegate
            {
                coverLabel.Visible = false;
            });
        }

        void AppendCover(string text)
        {
            coverLabel.Text = coverLabel.Text.ToString() + text;
        }
        private void FacilityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(FacilityList.SelectedItems.Count > 0 && FacilityListBox.SelectedItems.Count > 0)
            {
                FacilityType type = (FacilityType)FacilityListBox.Items.IndexOf(FacilityListBox.SelectedItems[0]);
                int selectedIndex = FacilityList.Items.IndexOf(FacilityList.SelectedItems[0]);
                string id = syncFacilityListId[selectedIndex];
                Storage.UpdateFacilityMapping(type, id);
            }
        }

        private void FacilityListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(FacilityListBox.SelectedItems.Count > 0)
            {
                FacilityType facilityType = (FacilityType)FacilityListBox.Items.IndexOf(FacilityListBox.SelectedItems[0]);

                Dictionary<string, int> map = Storage.GetAssetAt(facilityType);
                AssetList.Items.Clear();
                foreach (string key in map.Keys)
                {
                    ListViewItem item = new ListViewItem(key);
                    item.SubItems.Add("" + map[key]);
                    AssetList.Items.Add(item);
                }

                string facid = Storage.GetFacilityIdByType(facilityType);
                if(facid != "")
                {
                    int index = syncFacilityListId.IndexOf(facid);
                    if(index != -1)
                    {
                        FacilityList.Focus();
                        FacilityList.Items[index].Selected = true;
                    } else
                    {
                        FacilityList.SelectedItems.Clear();
                    }
                } else
                {
                    FacilityList.SelectedItems.Clear();
                }
            }
        }

        private void RunningBtn_Click(object sender, EventArgs e)
        {
            AssetList.Items.Clear();
            FacilityList.SelectedItems.Clear();
            FacilityListBox.SelectedItems.Clear();
            Dictionary<string, int> Running = Storage.GetRunningJob(ActivityType.Manufacturing);
            foreach(string key in Running.Keys)
            {
                ListViewItem item = new ListViewItem("Manufacturing - " + key);
                item.SubItems.Add(Running[key].ToString());
                AssetList.Items.Add(item);
            }

            Running = Storage.GetRunningJob(ActivityType.Reaction);
            foreach (string key in Running.Keys)
            {
                ListViewItem item = new ListViewItem("Reaction - " + key);
                item.SubItems.Add(Running[key].ToString());
                AssetList.Items.Add(item);
            }

            Running = Storage.GetRunningJob(ActivityType.Copying);
            foreach (string key in Running.Keys)
            {
                ListViewItem item = new ListViewItem("Copying - " + key);
                item.SubItems.Add(Running[key].ToString());
                AssetList.Items.Add(item);
            }
        }

        private void CopyBtn_Click(object sender, EventArgs e)
        {
            string clip = "";
            foreach(ListViewItem item in AssetList.SelectedItems)
            {
                clip += item.SubItems[0].Text + "\t" + item.SubItems[1].Text + "\n";
            }
            Clipboard.SetText(clip);
        }
    }
}