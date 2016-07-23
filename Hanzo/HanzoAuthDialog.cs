using Microsoft.Azure;
using Microsoft.Azure.Management.Compute;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hanzo
{
    public class HanzoAuthDialog
    {
        public static IDialog<AzureAuthProcess> MakeDialog()
        {
            return Chain.From(() => FormDialog.FromForm(AzureAuthProcess.BuildForm))
                                .Do(async (context, order) =>
                                {
                                    try
                                    {
                                        var completed = await order;
                                        // Actually process the sandwich order...
                                        await context.PostAsync("your authentication parameters are valid !");
                                    }
                                    catch (FormCanceledException<AzureAuthProcess> e)
                                    {
                                        string reply;
                                        if (e.InnerException == null)
                                        {
                                            reply = $"Your inputs are something wrong.";
                                        }
                                        else
                                        {
                                            reply = $"message is \"{e.Message}\". Please try again.";
                                        }
                                        await context.PostAsync(reply);
                                    }
                                });
        }
    }

    [Serializable]
    public class AzureAuthProcess
    {
        [Pattern("^\\w{8}-\\w{4}-\\w{4}-\\w{4}-\\w{12}$")]
        public string SubscriptionId;

        // you have to input more than one letter to pass this validate.
        [Pattern("\\w+")]
        public string AccessToken;

        public static IForm<AzureAuthProcess> BuildForm()
        {
            return new FormBuilder<AzureAuthProcess>()
                    //TODO: You should change this message to access your HanzoAuthWeb app to let your users getting AccessToken
                    .Message("Please authrize via Azure Active Directory")
                    .OnCompletion(async (context, state) =>
                    {
                        using (var azure = new ComputeManagementClient(new TokenCloudCredentials(state.SubscriptionId, state.AccessToken)))
                        {
                            var vms = azure.VirtualMachines.ListAll(null);
                        }
                        context.UserData.SetValue("SubscriptionId", state.SubscriptionId);
                        context.UserData.SetValue("AccessToken", state.AccessToken);
                    })
                    .Build();
        }
    }
}