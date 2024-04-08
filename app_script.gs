// This function is triggered when a file is added to the monitored folder
function onFileAddedToFolder(e) {
  var folderId = '1fn0EiQ7GCoes41gty0vLzRACkIXsNQOw'; // Replace with the ID of the folder to monitor
  // var fileId = e['id'];
  var fileId = '1oTPrsLNtFQHPY8rsG7t8zXVzaCH2w-Zxj8bM1vHtNl4';
  // Check if the added file is within the monitored folder
  var file = DriveApp.getFileById(fileId);
  var parentFolderId = file.getParents().next().getId();
  if (parentFolderId == folderId && file.getMimeType() == 'application/vnd.google-apps.presentation') {
    sendSlidesTextContent(fileId);
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
function sendSlidesTextContent(fileId) {

  var slidesContent = getSlidesTextContent(fileId);

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
