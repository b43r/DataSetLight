# DataSetLight
A VisualStudio extension that adds a lightweight alternative to ADO.NET DataSets.

## Install
Download from [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=SimonBaer.DataSetLight) or search for *DataSet Light* within the VisualStudio dialog "Extensions and Updates".

## Getting started
For this example you should have the *DataSet Light* extension installed in VisualStudio 2015 or VisualStudio 2017. Also you need access to an existing Microsoft SQL Server database.

Open an existing project or create a new one. Add a new item to your project and select *DataSet Light* under *Data*.

![new item](https://github.com/b43r/DataSetLight/raw/master/img/newitem.png "new item")

A new file "MyDataSet.dslt" will be added to your project. Double-click this file to open the DataSet editor. Initially your DataSet will be empty. Add a new entity by clicking on "Click to add new entity..." or right-click and choose "Create new entity..." from the context menu:

![empty DataSet](https://github.com/b43r/DataSetLight/raw/master/img/emptydataset.png "empty DataSet")

Enter a name for the new entity. An empty entity will be added to your DataSet. Now click on "Add..." to enter a new database query. If your project already contains a database connection string, it can be selected from a dialog. Otherwise click on "New connection..." to build a new connection string.


