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



function updateInventoryRow(fileMetaData, fileUrl) {

  if (!fileMetaData.info['inventory_sheet_row']) {
    insertInventoryRecord(fileMetaData)
  }

  var inventorySheet = SpreadsheetApp.openById(INVENTORY_SPREADSHEET_ID).getSheets()[0];
  var inventoryRow = inventorySheet.getRange(fileMetaData.info.inventory_sheet_row, 1, 1, 11);

  inventoryRow.getCell(1, INVENTORY_COLUMN_NAMES.STATUS).setValue(fileMetaData.status);

  inventoryRow.getCell(1, INVENTORY_COLUMN_NAMES.LAST_STATUS_CHANGE).setValue(new Date(Date.now()).toDateString());
  inventoryRow.getCell(1, INVENTORY_COLUMN_NAMES.INTERNAL_FILENAME).setValue(fileMetaData.info.file.file_name);
  inventoryRow.getCell(1, INVENTORY_COLUMN_NAMES.INTERNAL_LESSON_URL).setValue(fileUrl);
  inventoryRow.getCell(1, INVENTORY_COLUMN_NAMES.CLICKUP_TASK_URL).setValue(fileMetaData.info.clickup ? fileMetaData.info.clickup.taskUrl : "")
}

// Inserts an inventory row for given file's metadata
// returns row number for new record
function insertInventoryRecord(fileMetadata) {
  var inventorySheet = SpreadsheetApp.openById(INVENTORY_SPREADSHEET_ID).getSheets()[0];
  const originalFile = DriveApp.getFileById(fileMetadata.info.file.source_file_id);

  // find header columns
  var headers = inventorySheet.getRange(1, 1, 1, 11);

  var hasNextCell = true;
  var headerNum = 1;
  const startColumn = headers.getColumn();
  const lastColumn = headers.getLastColumn();
  // we'll need the next empty row for the table
  var newInventoryRowNumber = getNewInventoryRowNumber(startColumn, lastColumn, inventorySheet);
  var newInventoryRow = inventorySheet.getRange(newInventoryRowNumber, 1, 1, headers.getLastColumn());

  newInventoryRow.getValues().forEach(v => Logger.log(v))
  do {
    try {
      var headerCell = headers.getCell(1, headerNum)
      var columnName = headerCell.getValue()

      if (columnName === '') {
        hasNextCell = false
        continue;
      }

      var newCell = newInventoryRow.getCell(1, headerNum)

      Logger.log("Setting " + columnName)
      Logger.log("Column Number: " + headerCell.getColumn())
      Logger.log(newCell.getA1Notation())

      setInventoryRowCell(headerCell.getColumn(), newCell, fileMetadata, originalFile)

      headerNum++
    } catch (e) {
      hasNextCell = false
    }

  } while (hasNextCell)

  return newInventoryRowNumber;
}

function getNewInventoryRowNumber(startColumn, endColumn, sheet) {
  var lastRow = sheet.getLastRow();

  var range = sheet.getRange(1, startColumn, lastRow, endColumn - startColumn + 1);

  // Get the values in the range
  var values = range.getValues();

  // Initialize the next empty row as the row after the last one
  var nextEmptyRow = lastRow + 1;

  // Loop through each row in the range
  for (var i = 0; i < values.length; i++) {
    var isRowEmpty = true;
    // Check if all cells in this row are empty
    for (var j = 0; j < values[i].length; j++) {
      if (values[i][j] !== "" && values[i][j] !== null) {
        isRowEmpty = false;
        break;
      }
    }

    // If the row is completely empty, set nextEmptyRow and break
    if (isRowEmpty) {
      nextEmptyRow = i + 1; // +1 because arrays are 0-indexed but rows are 1-indexed
      break;
    }
  }

  Logger.log("The next completely empty row is: " + nextEmptyRow);
  return nextEmptyRow;
}

function setInventoryRowCell(columnNumber, rowCell, metadata, originalFile) {

  switch (columnNumber) {
    case INVENTORY_COLUMN_NAMES.STATUS:
      rowCell.setValue(REVISA_STATUS.IMPORTED);
      break;
    case INVENTORY_COLUMN_NAMES.LAST_STATUS_CHANGE:
      rowCell.setValue(new Date(Date.now()).toDateString())
      break;
    case INVENTORY_COLUMN_NAMES.GRADE:
      rowCell.setValue(metadata.info.grade)
      break;
    case INVENTORY_COLUMN_NAMES.SUBJECT:
      rowCell.setValue(metadata.info.subject)
      break;
    case INVENTORY_COLUMN_NAMES.LESSON_DATE:
      rowCell.setValue(metadata.info.delivery_date)
      break;
    case INVENTORY_COLUMN_NAMES.ORIGINAL_FILENAME:
      rowCell.setValue(originalFile.getName())
      break;
    case INVENTORY_COLUMN_NAMES.ORIGINAL_URL:
      rowCell.setValue(originalFile.getUrl())
      break;
    default:
      Logger.log("No matching column for: " + columnNumber);
      break;
  }

  Logger.log("Inventory Inserted")
}

function getElpsSupportsFromSheet(grade, subject, lessonDate) {

  const elpsSupportSheetName = ELPS_SUPPORTS_SHEET_MAPPING.filter(m => m.grades.includes(grade) && m.subjects.includes(subject))[0].sheet_name;
  var inventorySheet = SpreadsheetApp.openById(ELPS_TRACKER_FILE_ID).getSheetByName(elpsSupportSheetName);

  const dateColumn = inventorySheet.getRange(1, 1, inventorySheet.getDataRange().getNumRows(), 1);
  var lessonDateRowNum;

  const parsedDeliveryDate = new Date(Date.parse(lessonDate));

  var formattedDeliveryDate = new Intl.DateTimeFormat('fr-CA', {
    year: '2-digit',
    month: '2-digit',
    day: '2-digit',
  }).format(parsedDeliveryDate).replaceAll("/", "-");

  dateColumn.getValues().forEach((v, i) => {
    if (lessonDateRowNum)
      return;
    if (v[0].toUpperCase() === formattedDeliveryDate)
      lessonDateRowNum = i + 1;

  });
  const columnCount = inventorySheet.getDataRange().getNumColumns();
  const lessonDateRange = inventorySheet.getRange(lessonDateRowNum, 1, 1, columnCount);

  return {
    "elps_strategy": lessonDateRange.getCell(1, 2).getRichTextValue(),
    "elps_domain_objective": lessonDateRange.getCell(1, 3).getRichTextValue(),
    "elps_domain_objective_color": lessonDateRange.getCell(1, 3).getFontColorObject(),
    "cross_linguistic_connection": lessonDateRange.getCell(1, 4).getRichTextValue()
  }
}

function getRichText() {

  var inventorySheet = SpreadsheetApp.openById(ELPS_TRACKER_FILE_ID).getSheetByName('RW K-2');
  const sampleColumn = inventorySheet.getRange(1, 3, 40, 1);
  const filterStrings = ["elps domains/objectives", "elps domain/objective", "elps domains/objective"]

  sampleColumn.getRichTextValues().forEach(rtv => {
    var exportObject = {}
    rtv[0].getRuns().forEach(r => {
      if (filterStrings.includes(r.getText().toLowerCase().trim().replaceAll(":", ""))){
        return;
      }
      Logger.log("Is Bold: " + r.getTextStyle().isBold() + "\nText: " + r.getText()) 
    })
  })
}


// function updateTimelineSheet(processedFilesData) {


//   var timelineSheets = SpreadsheetApp.openById(TIMELINE_SPREADSHEET_ID);

//   // get grade/subject names for filtering
//   const gradeSubjectRange = timelineSheets.getSheets()[1].getRange(1, 1, 26)
//   const gradeSubject = "G" + processedFilesData.info.grade + " " + processedFilesData.info.subject
//   var gradeSubjectRow = 0;

//   gradeSubjectRange.getValues().forEach((v, i) => {
//     const gradeSubjectCellValue = v[0].toUpperCase()
//     if (gradeSubjectCellValue === gradeSubject.toUpperCase()) {
//       gradeSubjectRow = i + 1; // this is the row we will update
//     }
//   })

//   // if we have a matched row, find matched column
//   if (gradeSubjectRow != 0) {

//     var dateColumn = 0;

//     timelineSheets.getSheets().splice(1).forEach(sheet => {

//       if (dateColumn == 0) {
//         const dateRange = sheet.getRange(DATE_ROW, DATE_COLUMN_START, 1, TOTAL_DATE_COLUMNS)

//         //get and normalize the date for search
//         var dateParts = processedFilesData.info.delivery_date.split(".");

//         dateRange.getValues().forEach((v) => {
//           v.forEach((cellValue, columnIndex) => {
//             if (typeof cellValue != "string") {
//               if (cellValue.getMonth() + 1 === Number(dateParts[0]) && cellValue.getDate() === Number(dateParts[1])) {
//                 dateColumn = columnIndex + 1
//               }
//             }
//           })
//         })

//         if (gradeSubjectRow && dateColumn) {
//           var matchedLessonStatusCell = sheet.getRange(gradeSubjectRow, dateColumn + 2, 1, 1)
//           matchedLessonStatusCell.setValue("Received")
//           var receivedDate = new Date((Date.now()))
//           var matchedNotesCell = sheet.getRange(gradeSubjectRow, dateColumn + 3, 1, 1)
//           matchedNotesCell.setValue("EDU Rvd: " + receivedDate.toLocaleDateString().replaceAll("/", "-").replace("2024", "24"))
//         }
//       }

//     })


//   }
//   else {
//     Logger.log("No matching Grade/Subject row: " + gradeSubject)
//   }


// }

