// This function is triggered when a file is added to the monitored folder
function onFileAddedToFolder(e) {
  var folderId = '1fn0EiQ7GCoes41gty0vLzRACkIXsNQOw';
  // var fileId = e['id'];
  var fileId = '18AwcRcVzAQNnDQUrc-UsytUXO0foUraqLSthPJxNaaA';
  // Check if the added file is within the monitored folder
  var file = DriveApp.getFileById(fileId);
  var parentFolderId = file.getParents().next().getId();
  sendSlidesTextContent(fileId, file);
}

function getMetadata(file) {
  //parse out filename to corresponding folder
  var name_parts = file.getName().replaceAll("MASTER_", "").split("_");

  var users = file.getEditors().map((e) => {return {"name": e.getName(),"email": e.getEmail()}})
  //use meta data obj to find or create correct folder
  return {
    "client": name_parts[0],
    "grade": name_parts[1],
    "subject": name_parts[2],
    "delivery_date": name_parts[3] + '/' + name_parts[4] + '/' + name_parts[5],
    "updated_by": users,
    "created_at": file.getDateCreated()
  }
}

// Maps slide objects to array of each slide's text content
function getSlidesTextContent(fileId) {

  var slideFile = SlidesApp.openById(fileId);
  var slidesContent = slideFile.getSlides().map(function (slide) {
    var pageElements = slide.getPageElements();
    var slideContent = []

    pageElements.forEach(function (element) {
      if (element.getPageElementType() === SlidesApp.PageElementType.SHAPE) {
        var textContent = element.asShape().getText().asString();
        var objectId = element.getObjectId()
        slideContent.push({ "object_id": objectId, "text_content": textContent })
      }
    })
    return slideContent
  })
  return slidesContent;
}

// Function to send a Google Slides file to an external API
function sendSlidesTextContent(fileId, file) {

  var slidesContent = getSlidesTextContent(fileId);
  var request_data = { "info": getMetadata(file), "content": slidesContent }
  DriveApp.createFile(file.getName()+".json", JSON.stringify(request_data), MimeType.PLAIN_TEXT)

  // Set up the API endpoint and request parameters
  var apiUrl = 'YOUR_API_ENDPOINT';
  var headers = {
    'Content-Type': 'application/pdf',
    'Authorization': 'Bearer YOUR_API_TOKEN'
  };

  // Send the file data to the API
  var options = {
    'method': 'post',
    'headers': headers,
    'payload': fileData
  };

  // Make the HTTP request
  var response = UrlFetchApp.fetch(apiUrl, options);

  // Handle the API response
  Logger.log(response.getContentText());
}
