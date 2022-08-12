
### Create Google Sheet Reader ###

#### 1. Select "GoogleSheetReader" project under "urmobi.co" organization. ####

<img src="./imgs/google-sheets-project-selection.png">

#### 2. Create credential ####
Go to Create Credential->Service Account

<img src="./imgs/create-service-account-1.png">

Grant **Owner** role

<img src="./imgs/create-service-account-2.png">

#### 3. Get service account key ####
  - Click on just created service account under **Service accounts** tab.
  - Go to **Keys** tab.
  - Click **Add key->Create New key**.
  - Select **Json** (default).
  - Click **Create**.
  - Rename downloaded file to _"client_secret.json"_
  - Place it in your project in Resources folder

#### 4. Add Service Account to Google Sheets ####
  Copy service account mail under Service Accounts tab.
  Add created service account to your GoogleSheets table

  <img src="./imgs/add-service-account.png">

#### 5. Import GoogleSheetReader package ####

  Just import package to unity.
  If you already using Newtonsoft in your project ignore folder "Newtonsoft.Json <version>" while importing

  >___I recomended to upgrade unity builtin Newtonsoft package by adding "com.unity.nuget.newtonsoft-json": "3.0.2" to you manifest file under Packages folder.
  Version must be 3.+___

#### 6.  Fill Package variables

  Get google sheet id from link
  https:/docs.google.com/spreadsheets/d/**1WLEmjKoIO5I683ZopKTtvoHDKmomAjxQ47n1gz3s8zY**/edit#gid=1162944446
  Add early downloaded client_secret.json

  <img src="./imgs/package-configuration.png">

  Create minimum one table script using data from google sheet


  <img src="./imgs/table.png">


  ```C#
    [SpreadSheet("Squad")]
    [Serializable] public class SquadData : ISheetData
    {
        public string sID;
        public string uID;
        public int level;
        public float exp;
        public int unitCount;
    }
  ```
>_"Squad" it is name of table in google sheets_

  Click "Create sheets SO" and "Load Data" in package settings

#### 7. Create provider script

   To access google sheets data required to create provider script using just autocreated ConfigDatabase.cs

```C#
    public class ConfigDataProvider : SpreadSheetsDataProvider<ConfigDatabase>
    {

    }
```
#### 8. All done ####
***
### Functionality ###

<img src="./imgs/package-configuration.png">

* **Create sheets SO** - need to be click after adding each new data table. Add new data table to ConfigDatabase

* **Load data** - manual load latest data from google sheets base on created data tables. After success loading automatically save data into binary file;

* **Save to file** - manually save ConfigDatabase to binary file. Can be used after manual changing some values in ConfigDatabase to apply changes into binary

* **Clear all saves** - yes your right it just clear all saved binary files

* **Json** - #TODO remove from screenshots

### Data Access ###
```C#
  private ConfigDataProvider _configDataProvider;
  ...
  var values=_configDataProvider.Database.GetSpreadSheetData<SquadData>();
  foreach (var squadData in values)
  {
      Debug.Log(squadData.ToString());
  }
```
#### Output ####
> s1 u1 0 100 3<br>s2 u1 1 200 5
