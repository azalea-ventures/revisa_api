function getInboundDocuments() {

    // hard coded because we wanted to limit the scope 
    // of the manager for now without needing a huge input param for the function
    var subjects = [{ name: "Rla", grades: ["G2"], cycles: ["Cycle 1"] }]
    // look in client shared folder for updates
    var folder = DriveApp.getFolderById("13eDy1peg67kdTScD4eGyX6coQFdJYHl6");
  
    // iterate thru our approved subjects
    subjects.forEach(subject => {
  
      console.log(subject.name);
      var clientSubject = folder.getFoldersByName(subject.name).next();
  
      // ****call api for db records on this subject****
      var storedSubjectContentInfo = getContentInfoBySubject(subject.name);
  
      // iterate thru this subject's approved grades
      subject.grades.forEach(grade => {
        var clientGrade = clientSubject.getFoldersByName(grade).next();
  
        // iterate thru this grade's approved cycles
        subject.cycles.forEach(cycle => {
          var clientCycleWeeks = clientGrade.getFoldersByName(cycle).next().getFolders();
  
          while (clientCycleWeeks.hasNext()) {
            var clientWeekLessons = clientCycleWeeks.next().getFiles();
            preprocessInboundDocuments(clientWeekLessons, storedSubjectContentInfo);
          }
  
        })
      })
    })
  }
  
  
  function preprocessInboundDocuments(files, subjectContentInfo) {
  
    while (files.hasNext()) {
  
      var fileMaster = files.next();
      // now, we have this week's lessons. 
      // we will check to see if they've been processed previously
      subjectContentInfo.forEach((contentInfo) => {
        // if so, continue to next record
        if (fileMaster.getId() != contentInfo.info.file.source_file_id) {
          preprocessInboundDocument(fileMaster)
          return;
        }
        console.info("Content data for " + content.info.subject + ", " + content.info.grade + ", " + content.info.delivery_date + " has been recorded. Moving to next record...");
      })
    }
  }
  
  function preprocessInboundDocument(fileMaster) {
  
    var copyDestFolderId = "10eShvnJw8zdPjtdpZeu3f6LdLZhBwMVB"
  
    // move file master to processed folder
    var workingFileCopy = DriveApp.getFileById(postSlide(fileMaster.getId(), copyDestFolderId));
  
    var name_parts = workingFileCopy.getName().split("_")
    var gradeSubject = name_parts[0].split("")
    var grade = gradeSubject[0] == "G" ? gradeSubject[1] : ""
    var subject = grade != "" ? gradeSubject.splice(2).join("") : name_parts[0]
    var teks = name_parts.splice(3)
  
    var fileMetaData = {
      "info": {
        "client": "HOUSTON-ISD",
        "grade": grade,
        "subject": subject,
        "delivery_date": name_parts[2],
        "teks": teks,
        "file": {
          "file_id": workingFileCopy.getId(),
          "file_name": workingFileCopy.getName(),
          "source_file_id": fileMaster.getId(),
          "current_folder_id": workingFileCopy.getParents().next().getId(),
          "outbound_folder_id": fileMaster.getParents().next().getId(),
          "created_at": workingFileCopy.getDateCreated()
        },
        "status": "INBOUND",
        "updated_by": { "name": "Revisa", "email": "ling@eduspecialistonline.org" },
        "created_at": new Date()
      }
    }
    //add metadata to file description
    workingFileCopy.setDescription(fileMetaData)
  
    postContentInfo(workingFileCopy.getId(), workingFileCopy, fileMetaData)
  }
  
  function postContentInfo(fileId, file, request_data) {
    var request_data = getSlideContentRequest(fileId, file)
  
    // This outputs a json file for testing the API
    DriveApp.createFile(file.getName() + ".json", request_data, MimeType.PLAIN_TEXT)
  
    var apiUrl = 'https://revisa-api.azurewebsites.net/content/info';
    var headers = {
      'Content-Type': 'application/json',
    };
  
    // Send the file data to the API
    var options = {
      'method': 'post',
      'headers': headers,
      'payload': JSON.stringify(request_data)
    };
  
    // Make the HTTP request
    var response = UrlFetchApp.fetch(apiUrl, options);
  
    // Handle the API response
    var responseData = response.getContentText();
    var json = JSON.parse(responseData);
  
    return json // initial elps supports
  }
  
  // returns new slide's file id
  function postSlide(sourcePresentationId, destinationFolderId) {
    const sourceFile = DriveApp.getFileById(sourcePresentationId);
    const destinationFolder = DriveApp.getFolderById(destinationFolderId);
    return sourceFile.makeCopy(destinationFolder).getId();
  }
  
  // function getFileCopyId(fileMaster) {
  //   var workingFileCopy;
  
  //     // Step 1: Copy the source presentation to the destination folder
  //   const sourceFile = DriveApp.getFileById(fileMaster.getId());
  //   const destinationFolder = DriveApp.getFolderById(copyDestFolderId);
  //   const copiedFile = sourceFile.makeCopy(destinationFolder);
  //   const copiedPresentationId = copiedFile.getId();
  //   // const copiedPresentation = Slides.Presentations.get(copiedPresentationId);
  
  //   return copiedPresentationId;
  // }
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  
  