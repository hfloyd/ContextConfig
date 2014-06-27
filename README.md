# Context Config C# Library#

This library allows you to set configuration values dependent upon which web server environment the code is currently running in. Environments are defined using the hostname (url) that the page is operating from. Multiple urls can be set for each environment, along with an optional "catch-all" indicating what environment should be assumed if the current domain doesn't match.

*Compiled against .Net v. 4.5*

## What is included? ##

* Releases : Pre-compiled code ready to use as-is, documentation
* Solution :	Visual Studio .sln file and build output folders
* Source :
	* Documentation : Sandcastle Documentation project (ready-to-use documentation is located in the Releases folder)
	* HLF.ContextConfig project : All code required to operate, example config file
	* TestSite project : Single-page website demonstrating usage

## Installation & Usage ##

###ASP.Net Website###
The code can be used in any ASP.Net website by copying the files from 'Releases\Basic Compiled\Version X' to your site, customizing the example .config file, and referencing "HLF.ContextConfig" in your code.

###Umbraco CMS Website###
*Umbraco package coming soon!*

### Created by ###
Heather Floyd
* twitter: @HFloyd
* [www.HeatherFloyd.com](http://www.HeatherFloyd.com)