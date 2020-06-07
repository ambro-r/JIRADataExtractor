# JIRA Reporter
An implementation of the Atlassian JIRA REST API in order to extract data from a JIRA cloud instance. 

Presently the following is supported:
* The issue detail contained in the last completed sprint for a particular board. 

This project uses the following packages:
* Microsoft.Extensions.Configuration.Json
* Newtonsoft.Json
* RestSharp
* Serilog
* Serilog.Formatting.Compact
* Serilog.Settings.Configuration
* Serilog.Sinks.Console
* Serilog.Sinks.File

## To Do

Genearlly this would also be in the respective class, but it's here as a reminder as well...

* IssueParser: When seaching, need to find a way to ony return certain fields (i.e. might only want a list of Issue Keys).
* IssueParser: Need to build a proper object to represent the ChangeLog.
* SprintReport: Need to parse through the change log to figure out if the issue had anything to do with the sprint.
* SprintParser: Need to re-think and possibly re-do some of the logic. 

Just some VERY useful tools and API links:
* [JSON Formatter & Validator][1.1]
* [QuickType][1.2] : Convert JSON into gorgeous, typesafe code in any language.
* [Atlassion JIRA Cloud REST API][1.3] (version 3) 
* [Atlassion JIRA Cloud REST Sprint API][1.4] (version 1) 

[1.1]:https://jsonformatter.curiousconcept.com/
[1.2]:https://app.quicktype.io/
[1.3]:https://developer.atlassian.com/cloud/jira/platform/rest/v3
[1.4]:https://developer.atlassian.com/cloud/jira/software/rest/