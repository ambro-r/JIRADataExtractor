using JIRADataExtractor.Constants;
using JIRADataExtractor.Exceptions;
using JIRADataExtractor.Objects;
using JIRADataExtractor.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADataExtractor.Workers
{
    class StoryPointWorker
    {

        private string storyPointsField = "";

        public StoryPointWorker(List<Field> fields)
        {
            foreach(Field field in fields)
            {
                if(field.Name.ToLower().Equals(Fields.STORY_POINTS))
                {
                    storyPointsField = field.Key;
                    break;
                }
            }
            if(string.IsNullOrEmpty(storyPointsField))
            {
                throw new WorkerInitializationException(string.Format("Unable to find key for field \"{0}\"", Fields.STORY_POINTS));
            }
        }


        public void run(ConnectionDetails connectionDetails)
        {
            IssueParser issueParser = new IssueParser(connectionDetails);
        }


    }

    /*
     if (!issue.fields.issuetype.subtask) { 
    return
}

// Get the field ids
def fields = get('/rest/api/3/field')
        .asObject(List)
        .body as List<Map>

// Get the story points custom field to use in the script
def storyPointsField =  fields.find { it.name == "Story point estimate" }.id
logger.info("The id of the Story point estimate field is: $storyPointsField")

// Get the parent issue as a Map
def parent = (issue.fields as Map).parent as Map

// Retrieve all the subtasks of this issue's parent
def parentKey = parent.key
logger.info("Searching for subtasks of ${parentKey}")

// Note:  The search API is limited that to only be able to return a maximum of 50 results
def subTasks = get("/rest/api/3/search")
        .queryString("jql", "parent=${parentKey}")
        .queryString("fields", "parent,$storyPointsField")
        .asObject(Map)
        .body
        .issues as List<Map>
logger.info("Total subtasks for ${parentKey}: ${subTasks.size()}")

// Sum the estimates
def estimate = subTasks.collect { Map subtask ->
    subtask.fields[storyPointsField] ?: 0 
}.sum()
logger.info("Summed estimate: ${estimate}")


// Now update the parent issue
def result = put("/rest/api/3/issue/${parentKey}")
        .header('Content-Type', 'application/json')
        .body([
            fields: [
                (storyPointsField): estimate
            ]
        ])
        .asString()

// check that updating the parent issue worked
assert result.status >= 200 && result.status < 300
     */
}
