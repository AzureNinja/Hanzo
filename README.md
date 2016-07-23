# Hanzo Bot
This bot will manage Azure resources of your subscription. Please enjoy this. This solution contains below three projects.
- Hanzo: Bot application to talk with users
- HanzoAuthWeb: Web application to authrize you via Azure Active Directory and get AccessToken
- Proxy: Procedures to manage your Azure resources

## Hanzo
Now under writing...

## HanzoAuthWeb
You can get AccessToken without makeing service principals using PowerShell commands

### Input and Output
Input
- tenantId:your tenantid. e.g. xxxxxxx.onmicrosoft.com
- client_id: your applicationid. e.g. xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
- client_secret: your key via Azure AD of portal
- redirect_uri: http://xxxxxxxxxxxxxxxxxxxxxxx.azurewebsites.net/Home/Authorized

Output
- Your tenant AccessToken

### How to use?

1. Access to management portal and select Azure AD tenant (such as xxxxxxx.onmicrosoft.com) which you want to get AccessToken
2. Add a "Web application" to your Azure AD
3. Add "Windows Azure Service Management" into "permissions to other applications" section. And setup "Delegated Permissions" of "Windows Azure Service Management"
4. Setup "REPLY URL" as http://"your deplpoyment URL"/Home/Authorized at CONFIGURE tab of your added web application.
5. Setup "Keys" to take "client_secret" at CONFIGURE tab of your added web application. This key info is shown only one time, and you can use this as "client_secret".
6. Now you can take tenantId, client_id, client_secret and redirect_uri at CONFIGURE tab of your added web application.

## Proxy
Now under writing...
