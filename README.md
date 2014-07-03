# Context Config C# Library#

This C# library allows you to set configuration values dependent upon which web server environment the code is currently running. Environments are defined using the hostname (url) from which the page is operating using a basic XML config file. Multiple hostnames can be set for each environment. You can also add an optional "catch-all" wildcard domain indicating which environment should be assumed if the current domain doesn't match any that have been predefined.

*Compiled against .Net v. 4.5*

## What is included? ##

* Releases : Pre-compiled code ready to use as-is, documentation
* Solution :	Visual Studio .sln file and build output folders
* Source :
	* Documentation : Sandcastle Documentation project (ready-to-use documentation is located in the 'Releases' folder)
	* HLF.ContextConfig project : All code required to operate, example config file
	* TestSite project : Single-page website demonstrating usage

## Installation & Usage ##

###ASP.Net Website###
The code can be used in any ASP.Net website by copying the files from 'Releases\Basic Compiled\Version X' to your site, customizing the example .config file, and referencing "HLF.ContextConfig" in your code.

###Umbraco CMS Website###
*An Umbraco package is available at http://our.umbraco.org/projects/developer-tools/contextconfig

## Created by ##
Heather Floyd
* twitter: @HFloyd
* [www.HeatherFloyd.com](http://www.HeatherFloyd.com)

### Acknowledgements ###
Thank you,  Mike Barlow, for a fantastic, clean way of dealing with xml config files:  [KickStart – C# Custom Configuration](http://bardevblog.wordpress.com/2013/11/17/kickstart-c-custom-configuration/)