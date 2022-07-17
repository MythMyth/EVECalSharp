﻿using Newtonsoft.Json;
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
        Dictionary<FacilityType, Button> ButtonMap = new Dictionary<FacilityType, Button>();
        Dictionary<string, string> LocationName;
        Dictionary<FacilityType, string> FacilityMatch;
        List<string> syncFacilityListId;

        Button currBtn;
        string indy_path = "https://esi.evetech.net/latest/characters/{character_id}/industry/jobs";
        string asset_path = "https://esi.evetech.net/latest/characters/{character_id}/assets";
        public AssetViewer()
        {
            InitializeComponent();
            syncFacilityListId = new List<string>();
            ButtonMap.Add(FacilityType.SOURCE, button1);
            ButtonMap.Add(FacilityType.ADV_COMPONENT, button2);
            ButtonMap.Add(FacilityType.ADV_LARGE_SHIP, button3);
            ButtonMap.Add(FacilityType.ADV_MED_SHIP, button4);
            ButtonMap.Add(FacilityType.ADV_SMALL_SHIP, button5);
            ButtonMap.Add(FacilityType.FUEL_COMP, button6);
            ButtonMap.Add(FacilityType.LARGE_SHIP, button7);
            ButtonMap.Add(FacilityType.MEDIUM_SHIP, button8);
            ButtonMap.Add(FacilityType.SMALL_SHIP, button9);
            ButtonMap.Add(FacilityType.REACTION, button10);
            ButtonMap.Add(FacilityType.MODULE, button11);
            SetButtonTitle();
            LoadFacilityList();
        }

        void SetButtonTitle()
        {
            ButtonMap[FacilityType.SOURCE].Tag = FacilityType.SOURCE;
            ButtonMap[FacilityType.SOURCE].Text = "Home station";

            ButtonMap[FacilityType.ADV_COMPONENT].Tag = FacilityType.ADV_COMPONENT;
            ButtonMap[FacilityType.ADV_COMPONENT].Text = "Advance Component";

            ButtonMap[FacilityType.ADV_LARGE_SHIP].Tag = FacilityType.ADV_LARGE_SHIP;
            ButtonMap[FacilityType.ADV_LARGE_SHIP].Text = "T2 Large Ship";

            ButtonMap[FacilityType.ADV_MED_SHIP].Tag = FacilityType.ADV_MED_SHIP;
            ButtonMap[FacilityType.ADV_MED_SHIP].Text = "T2 Medium Ship";

            ButtonMap[FacilityType.ADV_SMALL_SHIP].Tag = FacilityType.ADV_SMALL_SHIP;
            ButtonMap[FacilityType.ADV_SMALL_SHIP].Text = "T2 Small Ship";

            ButtonMap[FacilityType.FUEL_COMP].Tag = FacilityType.FUEL_COMP;
            ButtonMap[FacilityType.FUEL_COMP].Text = "Fuel and Component";

            ButtonMap[FacilityType.LARGE_SHIP].Tag = FacilityType.LARGE_SHIP;
            ButtonMap[FacilityType.LARGE_SHIP].Text = "Large Ship";

            ButtonMap[FacilityType.MEDIUM_SHIP].Tag = FacilityType.MEDIUM_SHIP;
            ButtonMap[FacilityType.MEDIUM_SHIP].Text = "Medium Ship";

            ButtonMap[FacilityType.SMALL_SHIP].Tag = FacilityType.SMALL_SHIP;
            ButtonMap[FacilityType.SMALL_SHIP].Text = "Small Ship";

            ButtonMap[FacilityType.REACTION].Tag = FacilityType.REACTION;
            ButtonMap[FacilityType.REACTION].Text = "Reaction";

            ButtonMap[FacilityType.MODULE].Tag = FacilityType.MODULE;
            ButtonMap[FacilityType.MODULE].Text = "Module";
        }

        void LoadFacilityList()
        {
            LocationName = Storage.GetFacilityNames();
            FacilityMatch = Storage.GetFacilityMatch();
            syncFacilityListId.Clear();
            FacilityList.Items.Clear();
            foreach(string id in LocationName.Keys)
            {
                syncFacilityListId.Add(id);
                FacilityList.Items.Add(new ListViewItem(LocationName[id]));
            }
            if (currBtn != null) currBtn.BackColor = Color.White;
            currBtn = null;
        }
        private void button_Click(object sender, EventArgs e)
        {
            if(currBtn != null) currBtn.BackColor = Color.White;
            currBtn = (Button)sender;
            currBtn.BackColor = Color.Aqua;
            Dictionary<string, int> map = Storage.GetFacilityAsset((FacilityType)currBtn.Tag);
            string text = "";
            foreach(string key in map.Keys)
            {
                text += key + "\t" + map[key] + "\r\n";
            }
            AssetTextBox.Text = text;
            facilityName.Text = Storage.GetName((FacilityType)currBtn.Tag);

            FacilityType type = (FacilityType)currBtn.Tag;
            if(FacilityMatch.ContainsKey(type))
            {
                string id = FacilityMatch[type];
                int index = syncFacilityListId.IndexOf(id);
                if(index > -1)
                {
                    FacilityList.Focus();
                    FacilityList.Items[index].Selected = true;
                }
                else
                {
                    FacilityList.SelectedItems.Clear();
                }
            } else
            {
                FacilityList.SelectedItems.Clear();
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            /*if (currBtn == RunningJob)
            {
                Storage.SetRunningJob(AssetTextBox.Text);
            }
            else if (currBtn != null)
            {
                Storage.UpdateAsset(AssetTextBox.Text, (FacilityType)currBtn.Tag, facilityName.Text);
            }*/
            Storage.SaveFacilityMatch();
        }

        private void RunningJob_Click(object sender, EventArgs e)
        {
            if (currBtn != null) currBtn.BackColor = Color.White;
            currBtn = (Button)sender;
            currBtn.BackColor = Color.Aqua;
            Dictionary<string, int> map = Storage.GetRunningJob();
            string text = "";
            foreach (string key in map.Keys)
            {
                text += "\t" + map[key] + "\t\t" + key + "\r\n";
            }
            AssetTextBox.Text = text;
            facilityName.Text = "";
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
            Dictionary<string, CharInfo> chars = CharacterManager.GetCharList();
            List<Dictionary<string, string>> all_jobs = new List<Dictionary<string, string>>();
            Dictionary<string, int> map = new Dictionary<string, int>();
            List<string> ids = new List<string>();
            Dictionary<string, Dictionary<string, int>> assets = new Dictionary<string, Dictionary<string, int>>();
            foreach (string cid in chars.Keys)
            {
                Invoke(AppendCover, "\n" + cid + " - " + chars[cid].Name + ": Indy jobs ");
                CharacterManager.GetRefreshToken(chars[cid].code, chars[cid].refresh);
                HttpClient client = new HttpClient();
                if (chars[cid].IndyEtag.Trim() != "")
                {
                    client.DefaultRequestHeaders.Add("If-None-Match", chars[cid].IndyEtag);
                }
                
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
                } else
                {
                    Invoke(AppendCover, "failed, ");
                }
                if (response.Headers.Contains("etag"))
                {
                    chars[cid].IndyEtag = response.Headers.GetValues("etag").First();
                }
                Invoke(AppendCover, "Assets load ");
                int assetPage = 1;
                int success = 0;
                int not_modified = 0;
                int failed = 0;
                for (int currPage = 1; currPage <= assetPage; currPage++)
                {
                    response = client.GetAsync(asset_path.Replace("{character_id}", cid) + "?page=" + currPage + "&token=" + chars[cid].token).GetAwaiter().GetResult();
                    if(client.DefaultRequestHeaders.Contains("If-None-Match")) client.DefaultRequestHeaders.Remove("If-None-Match");
                    while (chars[cid].AssetEtag.Count < currPage)
                    {
                        chars[cid].AssetEtag.Add("");
                    }
                    if(chars[cid].AssetEtag[currPage - 1].Trim() != "") client.DefaultRequestHeaders.Add("If-None-Match", chars[cid].AssetEtag[currPage - 1]);
                    res_str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<Dictionary<string, string>> items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(res_str);
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
                        if (response.Headers.Contains("x-pages"))
                        {
                            string xpage = response.Headers.GetValues("x-pages").First();
                            assetPage = int.Parse(xpage);
                        }
                        success++;
                    } else if(response.StatusCode == System.Net.HttpStatusCode.NotModified)
                    {
                        not_modified++;
                    } else
                    {
                        failed++;
                    }
                    if (response.Headers.Contains("etag"))
                    {
                        chars[cid].AssetEtag[currPage - 1] = response.Headers.GetValues("etag").First();
                    }
                    Invoke(AppendCover, "success: " + success + ", not modified: " + not_modified + ", faield:" + failed);
                }
            }
            Invoke(AppendCover, "\n Update ids");
            Cache.AddIds(ids);
            Invoke(AppendCover, "\n Update running jobs");
            Storage.SetRunningJob(all_jobs);
            Invoke(AppendCover, "\n Update assets");
            Storage.UpdateAsset(assets);
            Invoke(delegate
            {
                coverLabel.Visible = false;
            });
        }

        void AppendCover(string text)
        {
            coverLabel.Text = coverLabel.Text.ToString() + text;
        }

        void SetCover(string text)
        {
            coverLabel.Text = text;
        }
        private void FacilityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(FacilityList.SelectedItems.Count > 0 && currBtn != null && currBtn != RunningJob)
            {
                FacilityType type = (FacilityType)currBtn.Tag;
                int selectedIndex = FacilityList.Items.IndexOf(FacilityList.SelectedItems[0]);
                string id = syncFacilityListId[selectedIndex];
                if (FacilityMatch.ContainsKey(type))
                {
                    FacilityMatch[type] = id;
                }
                else
                {
                    FacilityMatch.Add(type, id);
                }
            }
        }
    }
}
