using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanzoProxyAPI.Models;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using Microsoft.Rest;

namespace HanzoProxyAPI.Core
{
    class VirtualMachineManager
    {
        private ComputeManagementClient _client;

        public VirtualMachineManager(TokenCredentials cred, string subid)
        {
            this._client = new ComputeManagementClient(cred)
            { SubscriptionId = subid };
        }

        public VirtualMachineObject GetVirtualMachine(
                string resourceGroupName,
                string vmName)
        {
            VirtualMachineObject vmo = new VirtualMachineObject();
            var vmResult = this._client.VirtualMachines.Get(
                                            resourceGroupName,
                                            vmName,
                                            InstanceViewTypes.InstanceView);
            foreach (InstanceViewStatus istat in vmResult.InstanceView.Statuses)
            {
                vmo.name = vmName;
                vmo.resourcegroup = resourceGroupName;
                vmo.status = istat.DisplayStatus;
            }
            return vmo;
        }

        public List<VirtualMachineObject> GetAllVirtualMachines()
        {
            List<VirtualMachineObject> vmos = new List<VirtualMachineObject>();
            foreach (var vm in this._client.VirtualMachines.ListAll())
            {
                // ID= /subscriptions/87c7c7f9-0c9f-47d1-a856-1305a0cbfd7a/resourceGroups/RG-YOICHIKA-DEV/providers/Microsoft.Compute/virtualMachines/yoichika-win1
                Console.WriteLine("{0}:ID{1} ", vm.Name, vm.Id);
                VirtualMachineObject vmo = GetVirtualMachine("RG-YOICHIKA-DEV", vm.Name);
                vmos.Add(vmo);
            }
            return vmos;
        }

        public List<VirtualMachineObject> GetVirtualMachinesByResource(string resourceGroup)
        {
            List<VirtualMachineObject> vmos = new List<VirtualMachineObject>();
            foreach (var vm in this._client.VirtualMachines.List(resourceGroup))
            {
                VirtualMachineObject vmo = GetVirtualMachine("RG-YOICHIKA-DEV", vm.Name);
                vmos.Add(vmo);
            }
            return vmos;
        }

        public async void StartVirtualMachineAsync(string groupName, string vmName)
        {
            await this._client.VirtualMachines.StartAsync(groupName, vmName);
        }
        public void StartVirtualMachine(string groupName, string vmName)
        {
            this._client.VirtualMachines.Start(groupName, vmName);
        }

        public async void StopVirtualMachineAsync(string groupName, string vmName)
        {
            await this._client.VirtualMachines.PowerOffAsync(groupName, vmName);
        }
        public void StopVirtualMachine(string groupName, string vmName)
        {
            this._client.VirtualMachines.PowerOff(groupName, vmName);
        }

        public async void RestartVirtualMachineAsync(string groupName, string vmName)
        {
            await this._client.VirtualMachines.RestartAsync(groupName, vmName);
        }
        public void RestartVirtualMachine(string groupName, string vmName)
        {
            this._client.VirtualMachines.Restart(groupName, vmName);
        }
    }
}
