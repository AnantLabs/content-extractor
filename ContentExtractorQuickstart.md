# Content Extractor Quickstart #

Content Extractor is a desktop tool that allows you easily extract data from web-pages and save them into structured data format.

There are examples to help you to start working with Content Extrator 1.0

## Example 1 : Simple extraction ##

Here is the table describing climate in Death Valley, California:

|  | Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec | **Year** |
|:-|:----|:----|:----|:----|:----|:----|:----|:----|:----|:----|:----|:----|:---------|
| **Mean daily maximum temperature** | 64 °F| 71 °F| 80 °F| 89 °F| 98 °F| 107 °F| 114 °F| 113 °F| 105 °F| 91 °F| 75 °F| 64 °F| **89 °F** |
| **Mean daily maximum temperature** | 18 °C| 22 °C| 27 °C| 32 °C| 37 °C| 42 °C| 46 °C| 45 °C| 41 °C| 33 °C| 24 °C| 18 °C| **32 °C** |
| **Mean daily minimum temperature** | 37 °F| 44 °F| 51 °F| 60 °F| 69 °F| 78 °F| 86 °F| 84 °F| 73 °F| 59 °F| 46 °F| 37 °F| **60 °F** |
| **Mean daily minimum temperature** | 3 °C| 7 °C| 11 °C| 16 °C| 21 °C| 26 °C| 30 °C| 29 °C| 23 °C| 15 °C| 8 °C| 3 °C| **16 °C**|
| **Mean monthly rainfall** | 0.3" | 0.4" | 0.3" | 0.1" | 0.1" | 0.0" | 0.1" | 0.1" | 0.2" | 0.1"| 0.2" | 0.2" | **2.1"** |
| **Mean monthly rainfall** | 7.6mm | 10.2mm | 7.6mm| 2.5mm | 2.5mm | 0.0mm | 2.5mm | 2.5mm | 5.1mm | 2.5mm | 5.1mm | 5.1mm | **53.3mm** |



### Extract data from table into xml ###

  1. At first you need to add this page in list of working pages in Content Extractor. Look at _URLs panel_ (left-bottom of Content Extractor window, marked as "Drag file or URL here") and check if the URL of this page is in it. If you are looking at this page in Content Extractor you have it in this list already. If your looking at this page from your browser just drag'n'drop the URL from browser window or [just this link](ContentExtractorQuickstart.md) into _URLs panel_.
  1. Wait for several seconds until Content Extractor loads this page and shows it in the _Browser panel_ (right-bottom of your Content Extractor window)
  1. You can notice that the _Browser panel_ highlights HTML elements under your mouse cursor.
  1. Click on "**Mean daily maximum temperature**" cell in the second table row. When you do it Content Extractor will highlight it with light-red color. You'll also may notice that the _HTML Tree panel_ (in the top-left corner) will be expanded and the corresponding HTML node will be selected.
  1. Expand nodes in _HTML Tree panel_ inside selected "TD" node.
  1. Select "text()" inside "STRONG" node. Press right mouse button on "text()" and choose "Add node" item in opened context menu.
  1. After this you'll see that _result panel_ (top-right corner of Content Extractor) contains only one cell with "Mean daily maximum" text.
  1. Content Extractor template consists of rule defining the rows and a set of rules defining the cell values for each column. When you add the first HTML node to the template the row rule becomes to be the path to the node added (so there is only one row) and one column is added. The rule for this column is "." (identity rule). Content Extractor needs at least two nodes to suggest sensible rules.
  1. So we need to add one more cell from the same table row to the template. So click on "64 °F" cell. You'll notice that another TD node is selected in _HTML Tree panel_. Expand it, select the "text()" node inside it. Press the right mouse button on this node and select "Add node" menu item.
  1. You can see that Content Extractor recognized the row pattern and now you have 7 rows and 2 columns in your _result panel_. Now we need to add other columns (from Feb to **Year**) into our template.
  1. So you just click on "71 °F" cell, select "text()" subnode in _HTML Tree panel_ and add it to the template.
  1. Repeat this steps for the rest of columns. If you need you can reorder columns by dragging its headers. You can also delete columns. Just select one of them (press on any cell inside the column) and click "Delete column" button in _result panel_.

## Example 2: Google results parsing ##
Starting from version 1.0b.2847 Content Extractor is shipped with example files. One of them contains template to extract google search results. You can open this template in Content Extractor Gui and have a look at it.

You also can use ContentExtractor Console application to apply template to any query you want. Open "Content Extractor Command Prompt" from "Content Extractor" folder in your "All programs" menu. Then execute the following command:

`ContentExtractor.Console.exe Examples\google.cet output.xml http://www.google.com/search?q=foo+bar`







