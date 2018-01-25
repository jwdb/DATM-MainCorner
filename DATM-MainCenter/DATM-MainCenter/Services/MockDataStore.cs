using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Threading;
using DATM_MainCenter.Models;
using System.Net.NetworkInformation;

[assembly: Xamarin.Forms.Dependency(typeof(DATM_MainCenter.Services.MockDataStore))]
namespace DATM_MainCenter.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;

        public MockDataStore()
        {

            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "Main site address", Description="Hosting a blog on datm.nl" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Minecraft Server", Description="Currently Running FTB Relevations on chibi.hunter2.nl:25565" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Plex Server", Description="Chibi and Micro are online" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Teamspeak", Description="ts.datm.nl" },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}