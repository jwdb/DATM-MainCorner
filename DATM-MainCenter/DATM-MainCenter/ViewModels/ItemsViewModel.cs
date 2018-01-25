using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using Xamarin.Forms;

using DATM_MainCenter.Models;
using DATM_MainCenter.Services;
using DATM_MainCenter.Views;

namespace DATM_MainCenter.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "News";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Item;
                Items.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
        }



        public async Task<bool> PingIPAsync(string ip)
        {
            try
            {
                Ping pingSender = new Ping();

                var response = await pingSender.SendPingAsync(ip);

                return response.Status == IPStatus.Success;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }

                // Server statusses
                Items.Add(new Item()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Chibi Server Status",
                    Description = $"Status {(await PingIPAsync("chibi.hunter2.nl") ? "Online" : "Offline")}"
                });

                Items.Add(new Item()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "TS Server Status",
                    Description = $"Status {(await PingIPAsync("ts.datm.nl") ? "Online" : "Offline")}"
                });

                Items.Add(new Item()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Blog Server Status",
                    Description = $"Status {(await PingIPAsync("datm.nl") ? "Online" : "Offline")}"
                });

                MineStat stat = new MineStat("chibi.hunter2.nl",25565);

                await stat.Check();

                Items.Add(new Item()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "MC Server Status",
                    Description = $"Status: Up: {stat.ServerUp} Players: {stat.CurrentPlayers}/{stat.MaximumPlayers}"
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}