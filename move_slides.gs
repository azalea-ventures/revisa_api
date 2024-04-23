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

function createNewClickUpTask(taskName, taskDescription, tags) {

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
    "tags": tags,
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

  return json.id //task id
}


//finds all files within the 'renamed' folder and moves to appropriate working folders. 
//Will create needed folders
function moveSlides() {
  var folder = DriveApp.getFolderById("10eShvnJw8zdPjtdpZeu3f6LdLZhBwMVB");
  var files = folder.getFiles();

  while (files.hasNext()) {
    //make the copy
    var fileMaster = files.next();
    var fileCopy = fileMaster.makeCopy();
    
    //parse out filename to corresponding folder
    var name_parts = fileMaster.getName().replaceAll("MASTER_", "").replaceAll("Copy of ", "").split("_");

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
    var taskDescription = "Curriculum for " + fileMetaData.data.client + ", \nGrade " + fileMetaData.data.grade + " " + fileMetaData.data.subject + "\nLesson Date: " + fileMetaData.data.month + " " + fileMetaData.data.day + "\nGoogle Slides Link: " + fileMaster.getUrl()
    
    var taskName = fileMetaData.data.client + ", \nGrade " + fileMetaData.data.grade + " " + fileMetaData.data.subject

    var tags = [fileMetaData.data.client, fileMetaData.data.grade, fileMetaData.data.subject]

    createNewClickUpTask(taskName, taskDescription, tags)
  }
}

// function attachSlidesToTask(taskId, file){
//   var token = 'pk_82322956_M0PHADY6E7T7KDTV8364W7L4BOJO3RFM';
//   var endpoint =   'https://api.clickup.com/api/v2/task/' + taskId +'/attachment'

//   var fileBlob = DriveApp.getFileById(file.getId()).getUrl();
//   var blob = Utilities.newBlob(fileBlob, "plain/text")
//   // "application/vnd.google-apps.presentation"
//   var form = FetchApp.createFormData(); // Create form data
//   form.append("attachment", blob);

//   var headers = {
//     'Authorization': token,
//   };
  
//   var options = {
//     'method': 'POST',
//     'headers': headers,
//     'body': form
//   };

//   var response = FetchApp.fetch(endpoint, options);
//   var responseData = response.getContentText();
//   var json = JSON.parse(responseData);

//   Logger.log(json)
// }