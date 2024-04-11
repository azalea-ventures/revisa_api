function findOrCreateFolder(fileData) {
  var yearFolder = DriveApp.getFoldersByName(fileData.year).next()
  var lastFolder = yearFolder;
  for (const childFolder in fileData.data) {
    try {
      if (lastFolder.getName() === yearFolder.getName()) {
        lastFolder = yearFolder.getFoldersByName(fileData.data[childFolder]).next()
      }
      else {
        lastFolder = lastFolder.getFoldersByName(fileData.data[childFolder]).next()
      }
    } catch (e) {
      lastFolder = lastFolder.createFolder(fileData.data[childFolder])
    } finally {
      Logger.info(lastFolder.getName())
    }
  }
  return lastFolder
}

function createNewClickUpTask(taskName, taskDescription) {

  var token = 'pk_82322956_M0PHADY6E7T7KDTV8364W7L4BOJO3RFM';
  var endpoint = "https://api.clickup.com/api/v2/list/901402709756/task"

  var headers = {
    'Authorization': token,
    'Content-Type': 'application/json'
  };

  var body = {
    "name": taskName,
    "description": taskDescription,
    "status": "UNASSIGNED",
    "notify_all": true,
  }
  var options = {
    'method': 'post',
    'headers': headers,
    'payload': JSON.stringify(body)
  };

  var response = UrlFetchApp.fetch(endpoint, options);
  var responseData = response.getContentText();
  var json = JSON.parse(responseData);

  // Handle the response data
  // For example:
  Logger.log(json);
}


//finds all files within the 'inbox' folder and moves to appropriate working folders. 
//Will create needed folders
function moveSlides() {
  var folder = DriveApp.getFolderById("1QU9s1yK1EZUMRdkrL7DDYDA_vzU5b1e2");
  var files = folder.getFiles();

  while (files.hasNext()) {
    //make the copy
    var fileMaster = files.next();
    var fileCopy = fileMaster.makeCopy();

    //parse out filename to corresponding folder
    var name_parts = fileMaster.getName().split("_");

    //use meta data obj to find or create correct folder
    var fileMetaData = {
      "year": name_parts[3],
      "data": {
        "client": name_parts[0],
        "grade": name_parts[1],
        "subject": name_parts[2],
        "month": name_parts[4],
        "day": name_parts[5]
      }
    }

    var targetFolder = findOrCreateFolder(fileMetaData)
    var revisionsFolder = targetFolder.createFolder("revisions")
    fileMaster.setName("MASTER_" + fileMaster.getName())
    fileCopy.setName("REV_0_" + fileCopy.getName())

    //move to new folder
    fileMaster.moveTo(targetFolder)
    fileCopy.moveTo(revisionsFolder)
    
    //clickup integration
    //create new task, get task id back, add attachment to new task
    var taskDescription = fileMetaData.data.client + " " + "Grade " + fileMetaData.data.grade + " " + fileMetaData.data.subject + " " + fileMetaData.data.month + fileMetaData.data.day

    var taskName = "Grade " + fileMetaData.data.grade + " " + fileMetaData.data.subject

    createNewClickUpTask(taskName, taskDescription)
  }
}


